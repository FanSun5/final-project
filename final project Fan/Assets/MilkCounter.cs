using UnityEngine;

public class MilkCounter : CounterBase
{
    public GameObject bobaMilkCupPrefab;
    public void TryAddMilk()
    {
        if (holdPoint.childCount == 0) return;
        var item = holdPoint.GetChild(0).gameObject;
        if (item.GetComponent<ItemData>()?.type == ItemType.BobaCup)
        {
            Destroy(item);
            Instantiate(bobaMilkCupPrefab, holdPoint.position, holdPoint.rotation);
        }
    }
}