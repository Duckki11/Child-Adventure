using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class SimplePlayerMovement : MonoBehaviour
{
    public UIManager uiManager;
    public CharacterController controller;
    public Transform cam;

    // 【新增】用來控制動畫的變數
    public Animator animator;

    public float speed = 5.0f;
    public float moveSpeedMultiplier = 1.0f;

    [Header("跳躍與物理設定")]
    public float jumpHeight = 1.5f;
    public float gravity = -9.81f;
    public float playerMass = 15f;

    private Vector3 velocity;
    private bool isGrounded;

    [Header("體力系統 (不可回復)")]
    public Image staminaBar;
    public float maxStamina = 20f;
    public float currentStamina;

    public static bool isRotatingObject = false;

    void Start()
    {
        currentStamina = maxStamina;
        if (staminaBar != null)
        {
            staminaBar.fillAmount = currentStamina / maxStamina;
        }

        // 【新增】遊戲開始時，自動往下層尋找 Animator 動畫機
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }
    }

    public void ConsumeStamina(float amount)
    {
        currentStamina -= amount;
        currentStamina = Mathf.Max(currentStamina, 0);

        if (staminaBar != null)
        {
            staminaBar.fillAmount = currentStamina / maxStamina;
        }

        if (currentStamina <= 0 && uiManager != null)
        {
            uiManager.ShowGameOverScreen();
        }
    }

    void Update()
    {
        if (isRotatingObject)
        {
            // 如果正在原地旋轉物品，強制讓角色維持站立動畫
            if (animator != null) animator.SetFloat("Speed", 0f);
            return;
        }

        // --- 1. 處理水平移動 (WASD) ---
        float x = 0;
        float z = 0;
        if (Keyboard.current.aKey.isPressed) x -= 1;
        if (Keyboard.current.dKey.isPressed) x += 1;
        if (Keyboard.current.wKey.isPressed) z += 1;
        if (Keyboard.current.sKey.isPressed) z -= 1;

        Vector3 inputDir = new Vector3(x, 0, z).normalized;

        // 【關鍵新增】計算當前的移動強度 (0 到 1)，並傳送給 Animator 的 Speed 變數！
        if (animator != null)
        {
            animator.SetFloat("Speed", inputDir.magnitude);
        }

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

        // 當小男孩踩在物件上時 (Y軸向下)
        if (hit.moveDirection.y < -0.3f)
        {
            // 【全新寫法：溫和下壓力】
            // 不再使用破百的暴力數值，改用一個可以手動微調的平滑力道
            float stepForce = 5f;

            // 讓踩下去的力道會跟著木板本身的重量成正比，避免太重踩翻、太輕踩不動
            Vector3 force = Vector3.down * stepForce * body.mass;

            // 平滑地施加在腳踩到的位置上
            body.AddForceAtPosition(force, hit.point);
        }
    }
}