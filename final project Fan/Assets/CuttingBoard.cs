using UnityEngine;

public class CuttingBoard : CounterBase
{
    public GameObject slicedLemonPrefab;

    public void TryCut()
    {
        if (holdPoint.childCount == 0)
        {
            Debug.Log("�в˰���û�п����е���Ʒ��");
            return;
        }

        GameObject itemOnBoard = holdPoint.GetChild(0).gameObject;
        ItemData data = itemOnBoard.GetComponent<ItemData>();

        if (data != null && data.type == ItemType.Lemon)
        {
            Destroy(itemOnBoard);
            Instantiate(slicedLemonPrefab, holdPoint.position, holdPoint.rotation);
            Debug.Log("�����г�������Ƭ��");
        }
        else
        {
            Debug.Log("�в˰��ϲ������ʣ�");
        }
    }
}
