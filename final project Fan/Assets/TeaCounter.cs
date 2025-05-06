using UnityEngine;

public class TeaCounter : CounterBase
{
    public GameObject lemonTeaPrefab;      // ���ʲ�
    public GameObject icedTeaPrefab;       // ����
    public GameObject bobaMilkTeaPrefab;   // �����̲�

    public void TryAddTea()
    {
        if (holdPoint.childCount == 0)
            return;

        GameObject item = holdPoint.GetChild(0).gameObject;
        ItemData data = item.GetComponent<ItemData>();
        if (data == null)
            return;

        switch (data.type)
        {
            case ItemType.LemonCup:
                Make(lemonTeaPrefab);
                break;

            case ItemType.IceCup:
                Make(icedTeaPrefab);
                break;

            case ItemType.BobaMilkCup:
                Make(bobaMilkTeaPrefab);
                break;

            default:
                Debug.Log("�����ڲ�̨������" + data.type);
                break;
        }
    }

    private void Make(GameObject prefab)
    {
        Destroy(holdPoint.GetChild(0).gameObject);
        Instantiate(prefab, holdPoint.position, holdPoint.rotation);
    }
}
