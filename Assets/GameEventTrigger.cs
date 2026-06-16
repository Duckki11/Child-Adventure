using UnityEngine;

public class GameEventTrigger : MonoBehaviour
{
    [Header("教學設定")]
    public GameObject tutorialPanel;
    public bool showOnlyOnce = true;
    private bool hasShown = false;

    [Header("玩家控制權設定")]
    public MonoBehaviour playerMovementScript; // 拖入你的 SimplePlayerMovement

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && (!showOnlyOnce || !hasShown))
        {
            TriggerEvent();
        }
    }

    void TriggerEvent()
    {
        // 1. 顯示介面
        if (tutorialPanel != null) tutorialPanel.SetActive(true);

        // 2. 透過開關腳本取代 Time.timeScale (防止與轉場打架)
        if (playerMovementScript != null) playerMovementScript.enabled = false;

        // 3. 釋放滑鼠游標，讓玩家可以點擊按鈕
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        hasShown = true;
    }

    public void CloseTutorial()
    {
        if (tutorialPanel != null) tutorialPanel.SetActive(false);

        // 恢復遊戲
        if (playerMovementScript != null) playerMovementScript.enabled = true;

        // 鎖定滑鼠
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}