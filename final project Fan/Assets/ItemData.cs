using UnityEngine;

public enum ItemType
{
    Lemon,
    LemonSlice,
    Cup,
    LemonCup,
    Tea,
    LemonTea,
    // 以后还可以加更多，比如：
    // Milk,
    // Boba,
    // Sugar,
}

public class ItemData : MonoBehaviour
{
    public ItemType type; // 这个物品是什么类型
}
