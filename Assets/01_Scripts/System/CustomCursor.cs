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

    private void Awake()
    {
        canvas = FindCanvas();       
        gameObject.transform.SetParent(canvas.transform);
        Cursor.visible = false; // 기본 커서 숨기기
        canvasRectTransform = canvas.GetComponent<RectTransform>(); // Canvas의 RectTransform 가져오기             
    }
    void Start()
    {
        sensitivity = 60.0f;
        LoadSensitivity(); // 저장된 감도 값 불러오기  
        SetCursorToCenter();
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

        if (Input.GetMouseButtonDown(0))
        {
            MouseDrag(worldPosition);
            SimulateClick(worldPosition);
        }
        
        float scroll = Input.GetAxis("Mouse ScrollWheel") * 10;
        if (scroll != 0)
        {
            HandleScroll(worldPosition, scroll);
        }

        if (isDragging)
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current);
            pointerData.position = cursorPosition;
            ExecuteEvents.Execute(currentDraggable, pointerData, ExecuteEvents.dragHandler);
        }

        if (Input.GetMouseButtonUp(0))
        {
            MouseDragEnd(worldPosition);
        }
    }

    public void LoadSensitivity()
    {
        sensitivity = 60f;
        if (!isSetting)
        {
            float saveMagnification = PlayerPrefs.GetInt("cursorSensitivity", 50) / 100f;
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
    void MouseDrag(Vector2 position)
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current);
        pointerData.position = position;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.GetComponent<IDragHandler>() != null)
            {
                currentDraggable = result.gameObject;
                ExecuteEvents.Execute(result.gameObject, pointerData, ExecuteEvents.pointerDownHandler);
                isDragging = true;
                break;
            }
        }
    }

    void MouseDragEnd(Vector2 position)
    {
        if (isDragging)
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current);
            pointerData.position = position;
            ExecuteEvents.Execute(currentDraggable, pointerData, ExecuteEvents.pointerUpHandler);
            isDragging = false;
            currentDraggable = null;
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
}
