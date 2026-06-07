using UnityEngine;

public class LeverBoardSnap : MonoBehaviour
{
    [Header("木板要釘死的位置 (請設定在石頭上方一點)")]
    public Transform boardTargetPoint;

    [Header("蹺蹺板的旋轉軸 (決定它是上下翹，還是左右翹)")]
    public Vector3 hingeAxis = new Vector3(0, 0, 1);

    private void OnTriggerEnter(Collider other)
    {
        // 確認掉進來的是木板
        if (other.CompareTag("WoodBoard"))
        {
            // 1. 強制移動到支點上方並轉正
            other.transform.position = boardTargetPoint.position;
            other.transform.rotation = boardTargetPoint.rotation;

            // 2. 確保物理引擎有在運作 (不能被凍結)
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null) rb.isKinematic = false;

            // 3. 【關鍵魔法】加入鉸鏈關節，把它釘在空間中！
            HingeJoint hinge = other.gameObject.GetComponent<HingeJoint>();
            if (hinge == null)
            {
                hinge = other.gameObject.AddComponent<HingeJoint>();
            }
            hinge.axis = hingeAxis; // 設定旋轉方向

            // 4. 關閉這個感應區，避免重複觸發
            GetComponent<Collider>().enabled = false;
        }
    }
}