using UnityEngine;

public class LeverLogic : MonoBehaviour
{
    [Header("蹺蹺板兩端感應區")]
    public GameObject zoneA;
    public GameObject zoneB;

    [Header("目標石頭重量 (例如: 10)")]
    public float targetWeight = 10f;
    private float weightA = 0f;
    private float weightB = 0f;

    // 我們不再移動石頭，只是在旁邊紀錄石頭的存在
    public void UpdateWeight(string side, float weight, bool isAdding)
    {
        if (side == "A") weightA = isAdding ? weight : 0f;
        if (side == "B") weightB = isAdding ? weight : 0f;

        CheckBalance();
    }

    void CheckBalance()
    {
        // 這是你的機關邏輯：例如兩邊重量相差達到某個程度，或是總重量達標
        if (weightA >= targetWeight)
        {
            Debug.Log("蹺蹺板 A 端被壓下！機關開啟！");
            // 這裡呼叫你的機關動畫或成功介面
        }
    }
}