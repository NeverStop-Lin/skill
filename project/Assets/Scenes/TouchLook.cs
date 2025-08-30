using UnityEngine;
using UnityEngine.EventSystems;

public class TouchLook : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [Header("设置")]
    [SerializeField]
    private float lookSensitivity = 0.5f;
    [SerializeField]
    private bool clampVertical = true;
    [SerializeField]
    private float minVerticalAngle = -60f;
    [SerializeField]
    private float maxVerticalAngle = 60f;

    /// <summary>
    /// 公开的视角输入增量 (X:水平旋转量, Y:垂直旋转量)。
    /// 这个值只在有拖拽的那一帧有效，下一帧会自动归零。
    /// </summary>
    public Vector2 LookInputDelta { get; private set; }

    public float CurrentVerticalAngle { get; private set; } = 0f;

    private Vector2 _lastPointerPosition;
    private bool _isDragging = false;
    
    // --- 核心改动：用一个私有变量来暂存拖拽数据 ---
    private Vector2 _currentDragDelta; 

    /// <summary>
    /// Update 负责消费和重置输入数据
    /// </summary>
    void Update()
    {
        // 每一帧开始时，都假设没有输入
        LookInputDelta = Vector2.zero;

        // 如果正在拖拽，则处理拖拽数据
        if (_isDragging)
        {
            // --- 步骤 1: 将暂存的拖拽数据转换为旋转增量 ---
            float horizontalDelta = _currentDragDelta.x * lookSensitivity;
            float verticalDelta = _currentDragDelta.y * lookSensitivity;

            // --- 步骤 2: 累积并钳制垂直角度 ---
            float newVerticalAngle = CurrentVerticalAngle - verticalDelta;
            if (clampVertical)
            {
                newVerticalAngle = Mathf.Clamp(newVerticalAngle, minVerticalAngle, maxVerticalAngle);
            }
            verticalDelta = CurrentVerticalAngle - newVerticalAngle; // 重新计算实际应用的增量
            CurrentVerticalAngle = newVerticalAngle;

            // --- 步骤 3: 赋值给公开属性，供外部读取 ---
            LookInputDelta = new Vector2(horizontalDelta, verticalDelta);

            // --- 步骤 4 (关键): 消费完数据后立即清空暂存区 ---
            _currentDragDelta = Vector2.zero;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _isDragging = true;
        _lastPointerPosition = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // OnDrag 只负责产生原始的拖拽数据，并暂存起来
        if (!_isDragging) return;

        Vector2 currentPointerPosition = eventData.position;
        // 累加拖拽位移，而不是直接赋值，以防止在低帧率下丢失输入
        _currentDragDelta += currentPointerPosition - _lastPointerPosition; 
        _lastPointerPosition = currentPointerPosition;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _isDragging = false;
        // 重置暂存区，以防万一
        _currentDragDelta = Vector2.zero;
    }
}