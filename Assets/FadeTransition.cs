using UnityEngine;

public class FadeTransition : MonoBehaviour
{
    [Header("轉場黑幕設定")]
    public CanvasGroup blackScreenGroup;
    public float fadeSpeed = 1f;

    [Header("文字框設定")]
    public GameObject dialogBox;
    public float delayBeforeDialog = 0.5f;

    // 【新增】對話框在畫面上停留幾秒後消失
    public float dialogDisplayTime = 3f;

    private int step = 0;
    private float timer = 0f;

    void Start()
    {
        if (blackScreenGroup != null)
        {
            blackScreenGroup.alpha = 1f;
            blackScreenGroup.blocksRaycasts = true;
        }
        if (dialogBox != null)
        {
            dialogBox.SetActive(false);
        }
    }

    void Update()
    {
        // 階段 0：黑幕慢慢變透明
        if (step == 0)
        {
            if (blackScreenGroup != null)
            {
                blackScreenGroup.alpha -= Time.deltaTime * fadeSpeed;

                if (blackScreenGroup.alpha <= 0f)
                {
                    blackScreenGroup.alpha = 0f;
                    blackScreenGroup.blocksRaycasts = false;

                    step = 1;
                    timer = 0f; // 重置計時器，準備算出場延遲
                }
            }
        }
        // 階段 1：黑幕退完，等待幾秒後彈出文字
        else if (step == 1)
        {
            timer += Time.deltaTime;

            if (timer >= delayBeforeDialog)
            {
                if (dialogBox != null) dialogBox.SetActive(true);

                step = 2;
                timer = 0f; // 重置計時器，準備算文字停留時間
            }
        }
        // --- 【新增階段 2】：時間一到，自動隱藏文字框 ---
        else if (step == 2)
        {
            timer += Time.deltaTime;

            if (timer >= dialogDisplayTime)
            {
                if (dialogBox != null) dialogBox.SetActive(false);
                step = 3; // 徹底完工，腳本正式休息
            }
        }
    }
}