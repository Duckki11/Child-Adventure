using UnityEngine;

public class SnapToTarget : MonoBehaviour
{
    [Header("要把石頭吸附到哪裡？")]
    public Transform targetPoint;

    private void OnTriggerEnter(Collider other)
    {
        // 檢查掉進這個範圍的是不是石頭？
        if (other.CompareTag("Rock"))
        {
            // 1. 強制把石頭瞬間移動到中心點
            other.transform.position = targetPoint.position;

            // 2. 可選：如果你希望石頭的方向也固定，就把下面這行取消註解
            // other.transform.rotation = targetPoint.rotation;

            // 3. 拔掉它的物理動力，讓它變成「石化狀態」絕對不會滾動！
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
            }
        }
    }
}