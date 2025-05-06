// IceCounter.cs
using UnityEngine;

public class IceCounter : CounterBase
{
    public GameObject iceCupPrefab;
    public void TryAddIce()
    {
        if (holdPoint.childCount == 0) return;
        var item = holdPoint.GetChild(0).gameObject;
        if (item.GetComponent<ItemData>()?.type == ItemType.Cup)
        {
            Destroy(item);
            Instantiate(iceCupPrefab, holdPoint.position, holdPoint.rotation);
        }
    }
}
