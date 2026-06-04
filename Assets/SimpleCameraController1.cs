using UnityEngine;
using UnityEngine.InputSystem;

public class SimpleCameraController : MonoBehaviour // 【修正】確保 Class 名字跟檔名一模一樣都有 1
{
    [Header("目標設定")]
    public Transform target;
    public Vector3 targetOffset = new Vector3(0, 1.5f, 0); // 往上偏移看著肩膀/頭

    [Header("視角設定")]
    public float distance = 5f;       // 相機距離
    public float sensitivity = 0.3f;  // 滑鼠靈敏度

    [Header("角度限制 (防翻轉與撞地)")]
    public float minYAngle = -10f; // 最低角度 
    public float maxYAngle = 70f;  // 最高角度 

    private float currentX = 0f;
    private float currentY = 20f;

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        currentX = angles.y;
        currentY = angles.x;
    }

    void LateUpdate()
    {
        if (target == null) return;

        if (Mouse.current.rightButton.isPressed)
        {
            Vector2 mouseDelta = Mouse.current.delta.ReadValue();
            currentX += mouseDelta.x * sensitivity;
            currentY -= mouseDelta.y * sensitivity;

            currentY = Mathf.Clamp(currentY, minYAngle, maxYAngle);
        }

        // 計算相機最終的位置與旋轉
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        Vector3 lookPosition = target.position + targetOffset;

        // 【關鍵修改】最標準的第三人稱距離公式！這樣 distance 才會完美發揮作用
        transform.position = lookPosition - (rotation * Vector3.forward * distance);
        transform.LookAt(lookPosition);
    }
}