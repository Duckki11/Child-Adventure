using UnityEngine;
using UnityEngine.SceneManagement; // 控制場景切換必備

public class GameOverAction : MonoBehaviour
{
    // 如果你想讓玩家「重新開始這一關」，就呼叫這個
    public void RestartLevel()
    {
        Time.timeScale = 1f; // 確保遊戲時間恢復正常（以免你在失敗時暫停了遊戲）

        // 自動重新讀取目前所在的關卡（第一關）
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // 如果你想讓玩家「回到主選單」，就呼叫這個
    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;

        // 確保括號裡的名字跟你的主選單場景名稱一模一樣
        SceneManager.LoadScene("MainMenu");
    }
}