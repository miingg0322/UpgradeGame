using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomCursor : MonoBehaviour
{   
    public float sensitivity;
    public RectTransform cursor;
    public bool isSetting;
    public float magnification;

    bool isDragging;
    
    Canvas canvas;
    Vector2 cursorPosition;
    RectTransform canvasRectTransform;
    GameObject currentDraggable;
    PointerEventData dragPointerData;

    void Awake()
    {
        // 씬에서 Canvas 탐색후 부모로 설정
        canvas = FindCanvas();       
        gameObject.transform.SetParent(canvas.transform);
        Cursor.visible = false; // 기본 커서 숨기기
        canvasRectTransform = canvas.GetComponent<RectTransform>(); // Canvas의 RectTransform 가져오기             
    }
    void Start()
    {
        sensitivity = 60.0f;
        LoadSensitivity(); // 저장된 감도 값 불러오기  
        SetCursorToCenter(); // 씬 시작시 커서를 중앙에 위치
    }

    // Update is called once per frame
    void Update()
    {
        Cursor.lockState = CursorLockMode.Locked;
        // 마우스 움직임 입력 받기
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        // 커서 위치 업데이트
        cursorPosition += new Vector2(mouseX, mouseY);

        // 캔버스 내에서 커서 위치 제한
        Vector2 minPosition = Vector2.zero;
        Vector2 maxPosition = canvasRectTransform.rect.size;

        cursorPosition.x = Mathf.Clamp(cursorPosition.x, minPosition.x, maxPosition.x);
        cursorPosition.y = Mathf.Clamp(cursorPosition.y, minPosition.y, maxPosition.y);

        // 화면 좌표로 변환
        RectTransformUtility.ScreenPointToWorldPointInRectangle(canvasRectTransform, cursorPosition, canvas.worldCamera, out Vector3 worldPosition);

        transform.position = worldPosition;

        // 마우스 클릭과 드래그 구현
        if (Input.GetMouseButtonDown(0))
        {
            SimulateClick(worldPosition);
            MouseDragStart(worldPosition);
        }
        else if (Input.GetMouseButton(0))
        {
            MouseDragUpdate(worldPosition);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            MouseDragEnd(worldPosition);
        }

        // 마우스 휠 스크롤 구현
        float scroll = Input.GetAxis("Mouse ScrollWheel") * 10;
        if (scroll != 0)
        {
            HandleScroll(worldPosition, scroll);
        }       
    }

    public void LoadSensitivity()
    {
        sensitivity = 60f;
        // 감도 설정중이 아닐때 저장된 감도 값 할당
        if (!isSetting)
        {
            float saveMagnification = PlayerPrefs.GetInt("cursorSensitivity", 50) / 100f;
            // 감도값이 0일경우 움직이지 않게 되니 최소값 설정
            if(saveMagnification < 0.1f)
            {
                saveMagnification = 0.1f;
            }
            magnification = saveMagnification;
        }
            
        sensitivity *= magnification;
        isSetting = false;
    }

    void SimulateClick(Vector2 position)
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current);
        pointerData.position = position;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        foreach (RaycastResult result in results)
        {
            Button button = result.gameObject.GetComponent<Button>();
            IPointerClickHandler clickHandler = result.gameObject.GetComponent<IPointerClickHandler>();
            if (button != null)
            {
                Debug.Log("버튼 클릭 실행");
                button.onClick.Invoke();              
            }
            else
            {
                if(clickHandler != null)
                {
                    Debug.Log("클릭 이벤트 실행");
                    clickHandler.OnPointerClick(pointerData);
                }
            }
        }
    }

    void MouseDragStart(Vector2 position)
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current);
        pointerData.position = position;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.GetComponent<IDragHandler>() != null)
            {
                //if (result.gameObject.GetComponent<InputField>() != null)
                //    SetCaretPosition(result.gameObject.GetComponent<InputField>(), position);

                currentDraggable = result.gameObject;
                dragPointerData = pointerData;
                ExecuteEvents.Execute(result.gameObject, pointerData, ExecuteEvents.pointerClickHandler);
                ExecuteEvents.Execute(result.gameObject, pointerData, ExecuteEvents.pointerDownHandler);
                isDragging = true;
                break;
            }
        }
    }
    public void MouseDragUpdate(Vector2 position)
    {
        if (isDragging)
        {
            dragPointerData.position = position;
            ExecuteEvents.Execute(currentDraggable, dragPointerData, ExecuteEvents.dragHandler); // 드래그 업데이트 이벤트 추가
        }
    }
    void MouseDragEnd(Vector2 position)
    {
        if (isDragging)
        {
            dragPointerData.position = position;
            ExecuteEvents.Execute(currentDraggable, dragPointerData, ExecuteEvents.pointerUpHandler);
            ExecuteEvents.Execute(currentDraggable, dragPointerData, ExecuteEvents.endDragHandler);
            isDragging = false;
            currentDraggable = null;
        }
    }

    void HandleScroll(Vector2 position, float scrollAmount)
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = position,
            scrollDelta = new Vector2(0, scrollAmount)
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        foreach (RaycastResult result in results)
        {
            ScrollRect scrollRect = result.gameObject.GetComponentInParent<ScrollRect>();
            if (scrollRect != null)
            {
                // ScrollRect에 스크롤 이벤트 전달
                scrollRect.OnScroll(pointerData);
                Debug.Log("Scrolled: " + scrollRect.name);
            }
        }
    }
 
    Canvas FindCanvas()
    {
        Canvas canvas = FindObjectOfType<Canvas>();

        return canvas;
    }

    void SetCursorToCenter()
    {
        if (canvasRectTransform != null)
        {
            Vector2 centerPosition = new Vector2(canvasRectTransform.rect.width / 2, canvasRectTransform.rect.height / 2);
            cursorPosition = centerPosition;

            // 화면 좌표로 변환
            RectTransformUtility.ScreenPointToWorldPointInRectangle(canvasRectTransform, cursorPosition, canvas.worldCamera, out Vector3 worldPosition);
            transform.position = worldPosition;
        }
    }

    //void SetCaretPosition(InputField inputField, Vector2 position)
    //{
    //    TextGenerator textGen = inputField.textComponent.cachedTextGenerator;
    //    int charIndex = GetCharacterIndexFromPosition(inputField, position, textGen);

    //    inputField.caretPosition = charIndex;
    //    inputField.selectionAnchorPosition = charIndex;
    //    inputField.selectionFocusPosition = charIndex;
    //}

    //int GetCharacterIndexFromPosition(InputField inputField, Vector2 localMousePosition, TextGenerator textGen)
    //{
    //    for (int i = 0; i < textGen.characterCountVisible; i++)
    //    {
    //        UICharInfo charInfo = textGen.characters[i];
    //        if (localMousePosition.x < charInfo.cursorPos.x)
    //        {
    //            return i;
    //        }
    //    }
    //    return textGen.characterCountVisible;
    //}
}
