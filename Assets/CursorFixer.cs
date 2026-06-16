using UnityEngine;
using UnityEngine.InputSystem;

public class CursorFixer : MonoBehaviour
{
    void Update()
    {
        // 按下 F1 鍵強制恢復游標 (隨時可以在測試時使用)
        if (Keyboard.current.f1Key.wasPressedThisFrame)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Debug.Log("游標已強制恢復顯示！");
        }
    }
}