using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverAction : MonoBehaviour
{
    // 如果你想讓玩家「重新開始這一關」
    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // 回到主畫面
    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;

        // 【檢查點】請確認這裡的名字跟你 Unity 場景資料夾裡的主選單檔案名稱完全一致
        // 假設你的主選單場景檔案叫做 "MainMenu"
        SceneManager.LoadScene("MainMenu");
    }
}