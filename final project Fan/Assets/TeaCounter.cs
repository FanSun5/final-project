using UnityEngine;

public class TeaCounter : CounterBase
{
    public GameObject lemonTeaPrefab;      // 柠檬茶
    public GameObject icedTeaPrefab;       // 冰茶
    public GameObject bobaMilkTeaPrefab;   // 波霸奶茶

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
                Debug.Log("不能在茶台制作：" + data.type);
                break;
        }
    }

    private void Make(GameObject prefab)
    {
        // 销毁旧物件
        if (holdPoint.childCount > 0)
            Destroy(holdPoint.GetChild(0).gameObject);

        // 先生成一个实例，然后挂到 holdPoint 下，保留世界坐标
        GameObject newItem = Instantiate(prefab);
        newItem.transform.SetParent(holdPoint, worldPositionStays: true);

        // 重置位置与旋转
        newItem.transform.localPosition = Vector3.zero;
        newItem.transform.localRotation = Quaternion.identity;

        // 拿到 holdPoint 在世界空间的缩放
        Vector3 worldScale = holdPoint.lossyScale;
        // 抵消父级缩放
        newItem.transform.localScale = new Vector3(
            1f / worldScale.x,
            1f / worldScale.y,
            1f / worldScale.z
        );
    }

}
