/*using UnityEngine;

public class WeightPuzzlePlus : MonoBehaviour
{
    [Header("支點設定 (木板會吸附到這裡)")]
    public Transform pivotPoint;

    [Header("機關設定")]
    public float targetWeight = 10f;
    public float currentWeight = 0f;
    public bool isSolved = false;

    private void OnTriggerStay(Collider other)
    {
        // 1. 如果是木板進來，把它釘在支點上
        if (other.CompareTag("WoodBoard"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null && !rb.isKinematic && other.GetComponent<HingeJoint>() == null)
            {
                // 瞬間吸附並釘住
                other.transform.position = pivotPoint.position;
                other.transform.rotation = pivotPoint.rotation;

                // 關閉物理，像釘子一樣釘住
                rb.isKinematic = true;
                other.GetComponent<Collider>().enabled = false;

                Debug.Log("木板已固定在支點上！");
            }
        }

        // 2. 如果是石頭進來，計算重量
        if (other.CompareTag("Rock") && !isSolved)
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            // 石頭一進來就釘在木板上，不再製造碰撞 (防全壘打)
            if (rb != null && !rb.isKinematic)
            {
                rb.isKinematic = true;
                other.GetComponent<Collider>().enabled = false;
                other.transform.position = pivotPoint.position + new Vector3(0, 0.5f, 0); // 堆疊在木板上

                currentWeight += rb.mass;
                CheckWeight();
            }
        }
    }

    void CheckWeight()
    {
        if (currentWeight >= targetWeight)
        {
            isSolved = true;
            Debug.Log("🎉 蹺蹺板重量平衡，機關開啟！");
            // 這裡呼叫你的開門邏輯
        }
    }
}*/