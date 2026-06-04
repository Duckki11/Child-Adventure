using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class InventorySystem : MonoBehaviour
{
    public SimplePlayerMovement movementScript;
    public float maxStrength = 15f;

    [Header("UI 介面設定")]
    public Text actionText;
    public GameObject actionPanel;
    public Text messageText;
    public GameObject messagePanel;

    public Camera cam;
    public Transform holdPoint;
    public SimpleCameraController cameraController;

    [Header("抓取設定")]
    public float holdDistance = 1.5f;
    public float minHoldDist = 1.0f;
    public float maxHoldDist = 4.0f;
    public float rotateSpeed = 0.2f;

    [Header("落點與視覺優化")]
    public GameObject dropIndicator;
    public Material ghostMaterial;

    private PickableItem currentItem = null;
    private GameObject heldObject = null;
    private float currentDist;

    private Material originalMaterial;
    private MeshRenderer heldRenderer;

    void Start()
    {
        if (actionPanel != null) actionPanel.SetActive(false);
        if (messagePanel != null) messagePanel.SetActive(false);
        if (dropIndicator != null) dropIndicator.SetActive(false);
    }

    void Update()
    {
        if (heldObject != null)
        {
            // 【刪除原本強制隱藏 UI 的錯誤指令，讓提示文字可以留在畫面上】

            Rigidbody rb = heldObject.GetComponent<Rigidbody>();
            if (movementScript != null && rb != null)
            {
                movementScript.moveSpeedMultiplier = Mathf.Clamp(1.0f - (rb.mass / (maxStrength * 2)), 0.4f, 1.0f);
            }

            float scroll = Mouse.current.scroll.ReadValue().y;
            if (scroll > 0.1f) currentDist += 0.5f;
            else if (scroll < -0.1f) currentDist -= 0.5f;
            currentDist = Mathf.Clamp(currentDist, minHoldDist, maxHoldDist);

            Vector3 targetPosition = holdPoint.position + cam.transform.forward * currentDist;
            heldObject.transform.position = targetPosition;

            if (dropIndicator != null)
            {
                if (!dropIndicator.activeSelf) dropIndicator.SetActive(true);
                RaycastHit groundHit;
                if (Physics.Raycast(heldObject.transform.position, Vector3.down, out groundHit, 20f, ~LayerMask.GetMask("Player")))
                {
                    dropIndicator.transform.position = groundHit.point + new Vector3(0, 0.05f, 0);
                    dropIndicator.transform.up = groundHit.normal;
                }
            }

            if (Keyboard.current.rKey.isPressed)
            {
                if (cameraController != null) cameraController.enabled = false;
                SimplePlayerMovement.isRotatingObject = true;
                Vector2 mouseDelta = Mouse.current.delta.ReadValue();
                heldObject.transform.Rotate(cam.transform.up, -mouseDelta.x * rotateSpeed, Space.World);
                heldObject.transform.Rotate(cam.transform.right, mouseDelta.y * rotateSpeed, Space.World);
            }
            else
            {
                if (cameraController != null) cameraController.enabled = true;
                SimplePlayerMovement.isRotatingObject = false;
            }

            if (Keyboard.current.qKey.wasPressedThisFrame)
            {
                if (cameraController != null) cameraController.enabled = true;
                SimplePlayerMovement.isRotatingObject = false;
                DropItem();
            }
        }
        else
        {
            if (movementScript != null) movementScript.moveSpeedMultiplier = 1.0f;
            if (currentItem != null && Keyboard.current.eKey.wasPressedThisFrame)
            {
                GrabItem(currentItem.gameObject);
            }
        }
    }

    void GrabItem(GameObject obj)
    {
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        float staminaCost = (rb != null) ? rb.mass : 1f;

        if (rb != null && rb.mass > maxStrength)
        {
            ShowTemporaryMessage("這太重了，我根本搬不動...");
            return;
        }

        if (movementScript != null && movementScript.currentStamina < staminaCost)
        {
            ShowTemporaryMessage("我太累了，現在沒有力氣搬這個...");
            return;
        }

        if (movementScript != null) movementScript.ConsumeStamina(staminaCost);

        heldObject = obj;
        currentDist = holdDistance;
        heldObject.transform.SetParent(null);
        heldObject.transform.rotation = cam.transform.rotation;

        if (rb != null) rb.isKinematic = true;
        Collider coll = heldObject.GetComponent<Collider>();
        if (coll != null) coll.enabled = false;

        heldRenderer = heldObject.GetComponent<MeshRenderer>();
        if (heldRenderer != null && ghostMaterial != null)
        {
            originalMaterial = heldRenderer.material;
            heldRenderer.material = ghostMaterial;
        }

        // --- 【關鍵修改】拿起物品時，顯示操作提示 ---
        if (actionPanel != null)
        {
            actionPanel.SetActive(true);
            actionText.text = "按 Q 放下 / 按住 R 旋轉";
        }

        currentItem = null;
    }

    void DropItem()
    {
        if (heldRenderer != null && originalMaterial != null)
        {
            heldRenderer.material = originalMaterial;
        }
        heldRenderer = null;
        originalMaterial = null;

        Collider coll = heldObject.GetComponent<Collider>();
        if (coll != null) coll.enabled = true;

        Rigidbody rb = heldObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        heldObject = null;

        // --- 【關鍵修改】放下物品時，清空文字並隱藏 UI ---
        actionText.text = "";
        if (actionPanel != null) actionPanel.SetActive(false);
        if (dropIndicator != null) dropIndicator.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pickable") && heldObject == null)
        {
            currentItem = other.GetComponent<PickableItem>();
            actionText.text = currentItem.itemName;
            if (actionPanel != null) actionPanel.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Pickable") && currentItem != null && other.gameObject == currentItem.gameObject)
        {
            currentItem = null;
            actionText.text = "";
            if (actionPanel != null) actionPanel.SetActive(false);
        }
    }

    public void ShowTemporaryMessage(string msg)
    {
        messageText.text = msg;
        messagePanel.SetActive(true);
        CancelInvoke("HideMessage");
        Invoke("HideMessage", 2f);
    }

    void HideMessage()
    {
        messagePanel.SetActive(false);
        messageText.text = "";
    }
}