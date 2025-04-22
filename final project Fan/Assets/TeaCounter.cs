using UnityEngine;

public class TeaCounter : CounterBase
{
    public GameObject lemonTeaPrefab; // ��������ɵ����ʲ�Prefab

    public void TryAddTea(GameObject heldItem)
    {
        if (heldItem == null) return;

        ItemData data = heldItem.GetComponent<ItemData>();
        if (data != null && data.type == ItemType.LemonCup)
        {
            // ��ȷ���Ӳ裡
            Vector3 spawnPos = holdPoint != null ? holdPoint.position : heldItem.transform.position;
            Quaternion spawnRot = holdPoint != null ? holdPoint.rotation : Quaternion.identity;

            Destroy(heldItem);
            Instantiate(lemonTeaPrefab, spawnPos, spawnRot);

            Debug.Log("�ɹ��Ӳ裬������ʲ裡");
        }
        else
        {
            Debug.Log("������������ˮ�����ܼӲ裡");
        }
    }
}
