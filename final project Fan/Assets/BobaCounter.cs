using UnityEngine;

public class BobaCounter : CounterBase
{
    public GameObject bobaCupPrefab;
    public void TryAddBoba()
    {
        if (holdPoint.childCount == 0) return;
        var item = holdPoint.GetChild(0).gameObject;
        if (item.GetComponent<ItemData>()?.type == ItemType.Cup)
        {
            Destroy(item);
            Instantiate(bobaCupPrefab, holdPoint.position, holdPoint.rotation);
        }
    }
}
