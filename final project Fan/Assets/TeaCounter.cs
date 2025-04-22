using UnityEngine;

public class TeaCounter : CounterBase
{
    public GameObject lemonTeaPrefab; // 加完茶生成的柠檬茶Prefab

    public void TryAddTea(GameObject heldItem)
    {
        if (heldItem == null) return;

        ItemData data = heldItem.GetComponent<ItemData>();
        if (data != null && data.type == ItemType.LemonCup)
        {
            // 正确，加茶！
            Vector3 spawnPos = holdPoint != null ? holdPoint.position : heldItem.transform.position;
            Quaternion spawnRot = holdPoint != null ? holdPoint.rotation : Quaternion.identity;

            Destroy(heldItem);
            Instantiate(lemonTeaPrefab, spawnPos, spawnRot);

            Debug.Log("成功加茶，变成柠檬茶！");
        }
        else
        {
            Debug.Log("必须拿着柠檬水杯才能加茶！");
        }
    }
}
