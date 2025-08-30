using UnityEngine;
using UnityEngine.EventSystems;

public class TouchLook : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [Header("设置")]
    [Tooltip("旋转敏感度")]
    [SerializeField]
    private float lookSensitivity = 0.5f;

    [Tooltip("是否启用垂直轴 (俯仰) 限制")]
    [SerializeField]
    private bool clampVertical = true;

    [Tooltip("垂直俯仰的最小角度 (向下)")]
    [SerializeField]
    private float minVerticalAngle = -60f;

    [Tooltip("垂直俯仰的最大角度 (向上)")]
    [SerializeField]
    private float maxVerticalAngle = 60f;

    /// <summary>
    /// 公开的视角输入增量 (X:水平旋转量, Y:垂直旋转量)
    /// </summary>
    public Vector2 LookInputDelta { get; private set; }

    /// <summary>
    /// 当前的垂直俯仰累积角度（用于钳制）
    /// </summary>
    public float CurrentVerticalAngle { get; private set; } = 0f;

    private Vector2 _lastPointerPosition; // 记录上一帧的指针位置
    private bool _isDragging = false;     // 标记是否正在拖拽

    // 用于适配 PC 鼠标输入，防止鼠标在点击时跳跃
    public void OnPointerDown(PointerEventData eventData)
    {
        _isDragging = true;
        _lastPointerPosition = eventData.position;
        // 重置增量，防止继承OnPointerDown之前的LookInputDelta
        LookInputDelta = Vector2.zero; 
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!_isDragging) return;

        // 1. 计算当前帧相对于上一帧的位移
        Vector2 currentPointerPosition = eventData.position;
        Vector2 delta = currentPointerPosition - _lastPointerPosition;

        // 2. 将位移转换为旋转增量
        // delta.x 影响水平旋转 (Yaw)
        // delta.y 影响垂直旋转 (Pitch)
        float horizontalDelta = delta.x * lookSensitivity;
        float verticalDelta = delta.y * lookSensitivity;

        // 3. 累积垂直角度并进行钳制 (Clamping)
        float newVerticalAngle = CurrentVerticalAngle - verticalDelta;

        if (clampVertical)
        {
            // 将累积角度限制在定义的范围内
            newVerticalAngle = Mathf.Clamp(newVerticalAngle, minVerticalAngle, maxVerticalAngle);
        }

        // 计算实际应用的垂直增量（考虑钳制后的角度变化）
        verticalDelta = CurrentVerticalAngle - newVerticalAngle;
        CurrentVerticalAngle = newVerticalAngle;

        // 4. 设置输出向量
        LookInputDelta = new Vector2(horizontalDelta, verticalDelta);

        // 5. 更新上一帧的位置
        _lastPointerPosition = currentPointerPosition;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _isDragging = false;
        // 抬起时，将增量归零，防止外部脚本继续应用旋转
        LookInputDelta = Vector2.zero; 
    }
}