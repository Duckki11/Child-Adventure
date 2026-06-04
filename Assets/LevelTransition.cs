using UnityEngine;

public class LevelTransition : MonoBehaviour
{
    public UIManager uiManager; // 【新增】綁定我們的 UI 管理器

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 碰到窗戶時，不直接切場景，而是呼叫成功畫面
            if (uiManager != null)
            {
                uiManager.ShowSuccessScreen();
            }
        }
    }
}