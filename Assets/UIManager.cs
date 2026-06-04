using UnityEngine;
using UnityEngine.SceneManagement; // 控制場景必須加這行

public class UIManager : MonoBehaviour
{
    [Header("UI 面板綁定")]
    public GameObject successPanel;   // 逃脫成功的底框
    public GameObject gameOverPanel;  // 體力耗盡的底框

    void Start()
    {
        // 遊戲開始時，確保這兩個結算畫面都是隱藏的
        if (successPanel != null) successPanel.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);

        // 確保遊戲時間是正常流動的
        Time.timeScale = 1f;
    }

    // --- 呼叫顯示介面的方法 ---

    public void ShowSuccessScreen()
    {
        successPanel.SetActive(true);
        Time.timeScale = 0f; // 暫停遊戲
    }

    public void ShowGameOverScreen()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f; // 暫停遊戲
    }

    // --- 給 UI 按鈕點擊用的方法 ---

    public void RestartLevel()
    {
        Time.timeScale = 1f; // 恢復時間
        // 重新載入目前所在的場景 (第一關)
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f; // 恢復時間
        // 載入主選單場景 (請確保名稱跟你建的一模一樣)
        SceneManager.LoadScene("MainMenu");
    }
}