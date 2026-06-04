using UnityEngine;

public class SceneMessageTrigger : MonoBehaviour
{
    [Header("場景文字設定")]
    [TextArea] // 這個屬性可以讓你在 Unity 裡有更大的框框可以打字
    public string messageContent = "這裡似乎有什麼東西...";

    [Header("是否只顯示一次？")]
    public bool triggerOnlyOnce = true; // 打勾的話，玩家走過去只會講一次，不會瘋狂洗頻

    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        // 檢查走進感應區的是不是玩家
        if (other.CompareTag("Player") && !hasTriggered)
        {
            // 抓取玩家身上的背包系統
            InventorySystem inv = other.GetComponent<InventorySystem>();
            if (inv != null)
            {
                // 呼叫警告框，把我們打的場景文字傳進去！
                inv.ShowTemporaryMessage(messageContent);

                if (triggerOnlyOnce)
                {
                    hasTriggered = true; // 標記為已觸發，下次就不會再叫了
                }
            }
        }
    }
}