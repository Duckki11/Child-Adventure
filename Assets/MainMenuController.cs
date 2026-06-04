using UnityEngine;
using UnityEngine.SceneManagement; // 控制場景切換必備

public class MainMenuController : MonoBehaviour
{
    // 當玩家點擊「Play」時執行
    public void StartGame()
    {
        Time.timeScale = 1f; // 確保遊戲時間是正常流動的

        // 【重要】請確保括號內的名字跟你「第一關」的場景名稱完全一模一樣（例如 SampleScene）
        SceneManager.LoadScene("SampleScene");
    }
}