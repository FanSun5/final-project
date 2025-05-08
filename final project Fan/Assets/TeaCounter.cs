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
        // ���پ����
        if (holdPoint.childCount > 0)
            Destroy(holdPoint.GetChild(0).gameObject);

        // ������һ��ʵ����Ȼ��ҵ� holdPoint �£�������������
        GameObject newItem = Instantiate(prefab);
        newItem.transform.SetParent(holdPoint, worldPositionStays: true);

        // ����λ������ת
        newItem.transform.localPosition = Vector3.zero;
        newItem.transform.localRotation = Quaternion.identity;

        // �õ� holdPoint ������ռ������
        Vector3 worldScale = holdPoint.lossyScale;
        // ������������
        newItem.transform.localScale = new Vector3(
            1f / worldScale.x,
            1f / worldScale.y,
            1f / worldScale.z
        );
    }

}
