using UnityEngine;

public class TeaCounter : CounterBase
{
    public GameObject lemonTeaPrefab; // ��������ɵ����ʲ�Prefab

    public void TryAddTea()
    {

        if (holdPoint.childCount == 0)
        {
            Debug.Log("");
            return;
        }

        GameObject itemOnBoard = holdPoint.GetChild(0).gameObject;
        ItemData data = itemOnBoard.GetComponent<ItemData>();

        if (data != null && data.type == ItemType.LemonCup)
        {
            Destroy(itemOnBoard);
            Instantiate(lemonTeaPrefab, holdPoint.position, holdPoint.rotation);
            Debug.Log("cut");
        }
        else
        {
            Debug.Log("cant cut");
        }
    }
}
