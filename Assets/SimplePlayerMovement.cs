using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class SimplePlayerMovement : MonoBehaviour
{
    public UIManager uiManager;
    public CharacterController controller;
    public Transform cam;
    public float speed = 5.0f;
    public float moveSpeedMultiplier = 1.0f;

    [Header("跳躍與物理設定")]
    public float jumpHeight = 1.5f;
    public float gravity = -9.81f;
    public float playerMass = 15f;

    private Vector3 velocity;
    private bool isGrounded;

    [Header("體力系統 (不可回復)")]
    // 【關鍵修改】這裡從 Slider 變成了 Image！
    public Image staminaBar;
    public float maxStamina = 20f;
    public float currentStamina;

    public static bool isRotatingObject = false;

    void Start()
    {
        currentStamina = maxStamina;
        if (staminaBar != null)
        {
            // Image 的 FillAmount 是 0 到 1 之間的小數，所以我們用「當前體力 / 最大體力」來算出比例
            staminaBar.fillAmount = currentStamina / maxStamina;
        }
    }

    public void ConsumeStamina(float amount)
    {
        currentStamina -= amount;
        currentStamina = Mathf.Max(currentStamina, 0);

        if (staminaBar != null)
        {
            // 每次扣體力時，重新計算比例並更新圖片的顯示進度
            staminaBar.fillAmount = currentStamina / maxStamina;
        }

        if (currentStamina <= 0 && uiManager != null)
        {
            uiManager.ShowGameOverScreen();
        }
    }

    void Update()
    {
        if (isRotatingObject) return;

        // --- 1. 處理水平移動 (WASD) ---
        float x = 0;
        float z = 0;
        if (Keyboard.current.aKey.isPressed) x -= 1;
        if (Keyboard.current.dKey.isPressed) x += 1;
        if (Keyboard.current.wKey.isPressed) z += 1;
        if (Keyboard.current.sKey.isPressed) z -= 1;

        Vector3 inputDir = new Vector3(x, 0, z).normalized;

        if (inputDir.magnitude > 0.1f)
        {
            float targetAngle = Mathf.Atan2(inputDir.x, inputDir.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            controller.Move(moveDir.normalized * (speed * moveSpeedMultiplier) * Time.deltaTime);
        }

        // --- 2. 處理重力與跳躍 ---
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    // --- 3. 物理互動：賦予男孩體重 ---
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;

        if (body == null || body.isKinematic)
        {
            return;
        }

        if (hit.moveDirection.y < -0.3f)
        {
            Vector3 force = Vector3.down * playerMass * Mathf.Abs(gravity);
            body.AddForceAtPosition(force, hit.point);
        }
    }
}