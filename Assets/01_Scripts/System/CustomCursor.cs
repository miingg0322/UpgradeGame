using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    List<GameObject> currentPointerEnterObjects = new List<GameObject>();
    List<GameObject> previousPointerEnterObjects = new List<GameObject>();

    void Awake()
    {
        // ������ Canvas Ž���� �θ�� ����
        canvas = FindCanvas();       
        gameObject.transform.SetParent(canvas.transform);
        Cursor.visible = false; // �⺻ Ŀ�� �����
        canvasRectTransform = canvas.GetComponent<RectTransform>(); // Canvas�� RectTransform ��������             
    }
    void Start()
    {
        sensitivity = 60.0f;
        LoadSensitivity(); // ����� ���� �� �ҷ�����  
        SetCursorToCenter(); // �� ���۽� Ŀ���� �߾ӿ� ��ġ
    }

    // Update is called once per frame
    void Update()
    {
        Cursor.lockState = CursorLockMode.Locked;
        // ���콺 ������ �Է� �ޱ�
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        // Ŀ�� ��ġ ������Ʈ
        cursorPosition += new Vector2(mouseX, mouseY);

        // ĵ���� ������ Ŀ�� ��ġ ����
        Vector2 minPosition = Vector2.zero;
        Vector2 maxPosition = canvasRectTransform.rect.size;

        cursorPosition.x = Mathf.Clamp(cursorPosition.x, minPosition.x, maxPosition.x);
        cursorPosition.y = Mathf.Clamp(cursorPosition.y, minPosition.y, maxPosition.y);

        // ȭ�� ��ǥ�� ��ȯ
        RectTransformUtility.ScreenPointToWorldPointInRectangle(canvasRectTransform, cursorPosition, canvas.worldCamera, out Vector3 worldPosition);

        transform.position = worldPosition;

        // ������ ��ġ�� ���� �̺�Ʈ üũ
        CheckPointerEvents(worldPosition);

        // ���콺 Ŭ���� �巡�� ����
        if (Input.GetMouseButtonDown(0))
        {
            SimulateClick(worldPosition, -1); // ���� Ŭ��
            MouseDragStart(worldPosition); // �巡�� ����
        }
        else if (Input.GetMouseButton(0))
        {
            MouseDragUpdate(worldPosition); // �巡��
        }
        else if (Input.GetMouseButtonUp(0))
        {
            MouseDragEnd(worldPosition); // �巡�� ����
        }
        else if(Input.GetMouseButtonDown(1))
        {
            SimulateClick(worldPosition, -2); // ������ Ŭ��
        }

        // ���콺 �� ��ũ�� ����
        float scroll = Input.GetAxis("Mouse ScrollWheel") * 10;
        if (scroll != 0)
        {
            HandleScroll(worldPosition, scroll);
        }       
    }

    public void LoadSensitivity()
    {
        sensitivity = 60f;
        // ���� �������� �ƴҶ� ����� ���� �� �Ҵ�
        if (!isSetting)
        {
            float saveMagnification = PlayerPrefs.GetInt("cursorSensitivity", 50) / 100f;
            // �������� 0�ϰ�� �������� �ʰ� �Ǵ� �ּҰ� ����
            if(saveMagnification < 0.1f)
            {
                saveMagnification = 0.1f;
            }
            magnification = saveMagnification;
        }
            
        sensitivity *= magnification;
        isSetting = false;
    }

    // ������Ʈ�� Ŀ���� ������� pointerEnter, pointerExit ����
    void CheckPointerEvents(Vector2 position)
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = position
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        currentPointerEnterObjects.Clear();

        foreach (RaycastResult result in results)
        {
            currentPointerEnterObjects.Add(result.gameObject);
            if (result.gameObject != previousPointerEnterObjects.Find(obj => obj == result.gameObject))
            {
                ExecuteEvents.Execute(result.gameObject, pointerData, ExecuteEvents.pointerEnterHandler);
            }
        }

        foreach (GameObject prevObj in previousPointerEnterObjects)
        {
            if (!currentPointerEnterObjects.Contains(prevObj))
            {
                ExecuteEvents.Execute(prevObj, pointerData, ExecuteEvents.pointerExitHandler);
            }
        }

        previousPointerEnterObjects.Clear();
        previousPointerEnterObjects.AddRange(currentPointerEnterObjects);
    }

    void SimulateClick(Vector2 position, int pointerId)
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current);
        pointerData.position = position;
        pointerData.pointerId = pointerId;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        foreach (RaycastResult result in results)
        {
            Button button = result.gameObject.GetComponent<Button>();
            IPointerClickHandler clickHandler = result.gameObject.GetComponent<IPointerClickHandler>();
            if (button != null)
            {
                button.onClick.Invoke();              
            }
            else
            {
                if(clickHandler != null)
                {
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
                if (result.gameObject.GetComponent<TMP_InputField>() != null)
                    SetCaretPosition(result.gameObject.GetComponent<TMP_InputField>(), position);

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
            ExecuteEvents.Execute(currentDraggable, dragPointerData, ExecuteEvents.dragHandler); // �巡�� ������Ʈ �̺�Ʈ �߰�
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
                // ScrollRect�� ��ũ�� �̺�Ʈ ����
                scrollRect.OnScroll(pointerData);
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

            // ȭ�� ��ǥ�� ��ȯ
            RectTransformUtility.ScreenPointToWorldPointInRectangle(canvasRectTransform, cursorPosition, canvas.worldCamera, out Vector3 worldPosition);
            transform.position = worldPosition;
        }
    }

    void SetCaretPosition(TMP_InputField inputField, Vector2 position)
    {
        int charIndex = GetCharacterIndexFromPosition(inputField, position);

        inputField.caretPosition = charIndex;
        inputField.selectionAnchorPosition = charIndex;
        inputField.selectionFocusPosition = charIndex;
    }

    int GetCharacterIndexFromPosition(TMP_InputField inputField, Vector2 localMousePosition)
    {
        int charIndex = TMP_TextUtilities.GetCursorIndexFromPosition(inputField.textComponent, localMousePosition, null);
        return charIndex;
    }
}
