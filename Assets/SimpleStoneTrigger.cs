using UnityEngine;

public class SimpleStoneTrigger : MonoBehaviour
{
    [Header("石頭要被釘住的目標點")]
    public Transform snapPoint;

    private void OnTriggerEnter(Collider other)
    {
        // 1. 確定掉進來的是石頭
        if (other.CompareTag("Rock"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();

            // 2. 確保石頭現在是被玩家拿著的狀態 (如果 isKinematic 為 true，表示正拿在手上)
            // 只有當玩家丟下 (isKinematic 變為 false) 時，我們才觸發吸附
            if (rb != null && !rb.isKinematic)
            {
                // 【關鍵：凍結物理】
                // 關閉重力與碰撞，讓它絕對不會因為碰撞而飛出去
                rb.isKinematic = true;
                other.GetComponent<Collider>().enabled = false;

                // 【關鍵：完美定位】
                // 把石頭瞬間移動到你指定的「釘點」上
                other.transform.position = snapPoint.position;
                other.transform.rotation = snapPoint.rotation;

                // 把石頭變成機關的一部份 (將它釘在木板上)
                other.transform.SetParent(this.transform);

                Debug.Log("石頭已成功釘在位置上！");
            }
        }
    }
}