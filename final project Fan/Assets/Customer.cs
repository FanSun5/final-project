using UnityEngine;
using UnityEngine.UI;

public class Customer : MonoBehaviour
{
    
    public ItemType[] possibleOrders = {
        ItemType.IcedTea,
        ItemType.LemonTea,
        ItemType.BobaMilkTea,
    };

    
    public Sprite[] iconSprites;

    
    public Image iconImage;

    
    public ItemType orderType;  // 运行时随机选定的订单

    void Start()
    {
        // 1. 随机从 possibleOrders 里选一个
        if (possibleOrders == null || possibleOrders.Length == 0)
        {
            Debug.LogError("Customer: possibleOrders 数组不能为空！");
            return;
        }
        int idx = Random.Range(0, possibleOrders.Length);
        orderType = possibleOrders[idx];
        Debug.Log("新顾客点单: " + orderType);

        // 2. 给头顶图标赋值
        if (iconSprites == null || iconSprites.Length <= idx)
        {
            Debug.LogWarning("Customer: iconSprites 数组长度不足，无法显示图标");
        }
        else if (iconImage == null)
        {
            Debug.LogWarning("Customer: 未绑定 iconImage，无法显示图标");
        }
        else
        {
            iconImage.sprite = iconSprites[idx];
        }
    }
}
