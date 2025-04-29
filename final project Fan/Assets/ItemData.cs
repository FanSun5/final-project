using UnityEngine;

public enum ItemType
{
    Lemon,
    LemonSlice,
    Cup,
    LemonCup,
    Tea,
    LemonTea,
    Milk,
    Boba,
    ice,
    BobaCup,
    BobaTeaCup,
    MilkTea,
    BobaMilkTea,
    IcedLemonTea,
}

public class ItemData : MonoBehaviour
{
    public ItemType type;
}
