/*using UnityEngine;

public class WeightPuzzle : MonoBehaviour
{
    [Header("石頭吸附點")]
    public Transform dropPoint;
    public float targetWeight = 10f;
    private float currentWeight = 0f;

    private void OnTriggerEnter(Collider other)
    {
        // 只針對石頭觸發
        if (other.CompareTag("Rock"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();

            // 只有當石頭處於放下狀態 (isKinematic 為 false) 時才吸附
            if (rb != null && !rb.isKinematic)
            {
                // 1. 強制石化，關閉物理：這是防止抖動的關鍵
                rb.isKinematic = true;
                other.GetComponent<Collider>().enabled = false; // 關鍵：關閉碰撞避免彈飛小男孩

                // 2. 完美定位
                other.transform.position = dropPoint.position;
                other.transform.rotation = dropPoint.rotation;
                other.transform.SetParent(this.transform); // 確保石頭跟著木板，避免相對位移產生抖動

                // 3. 重量累加
                currentWeight += rb.mass;
                Debug.Log("目前重量: " + currentWeight);

                if (currentWeight >= targetWeight)
                {
                    Debug.Log("重量達標，機關觸發！");
                }
            }
        }
    }
}
*/