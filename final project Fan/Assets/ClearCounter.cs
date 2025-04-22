using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : CounterBase
{
    public GameObject lemonCupPrefab; // 合成后的柠檬水杯Prefab

    private List<GameObject> placedItems = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            ItemData data = other.GetComponent<ItemData>();
            if (data == null) return;

            if (data.type == ItemType.LemonSlice)
            {
                if (!HasItemType(ItemType.Cup))
                {
                    Debug.Log("需要先放杯子才能放柠檬片！");
                    return;
                }
            }

            placedItems.Add(other.gameObject);
            CheckCombination();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            placedItems.Remove(other.gameObject);
        }
    }

    private void CheckCombination()
    {
        bool hasCup = false;
        bool hasLemonSlice = false;
        GameObject cup = null;
        GameObject lemonSlice = null;

        foreach (GameObject item in placedItems)
        {
            ItemData data = item.GetComponent<ItemData>();
            if (data != null)
            {
                if (data.type == ItemType.Cup)
                {
                    hasCup = true;
                    cup = item;
                }
                else if (data.type == ItemType.LemonSlice)
                {
                    hasLemonSlice = true;
                    lemonSlice = item;
                }
            }
        }

        if (hasCup && hasLemonSlice)
        {
            Vector3 spawnPos = (cup.transform.position + lemonSlice.transform.position) / 2f;
            Destroy(cup);
            Destroy(lemonSlice);
            placedItems.Clear();
            Instantiate(lemonCupPrefab, spawnPos, Quaternion.identity);
        }
    }

    private bool HasItemType(ItemType type)
    {
        foreach (GameObject item in placedItems)
        {
            ItemData data = item.GetComponent<ItemData>();
            if (data != null && data.type == type)
            {
                return true;
            }
        }
        return false;
    }
}
