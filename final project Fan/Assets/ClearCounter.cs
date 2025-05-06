using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : CounterBase
{
    public GameObject lemonCupPrefab;
    private List<GameObject> placedItems = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Item")) return;

        var go = other.gameObject;
        if (placedItems.Contains(go)) return;

        placedItems.Add(go);
        CheckCombination();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Item"))
            placedItems.Remove(other.gameObject);
    }

    private void CheckCombination()
    {
        GameObject cup = null, lemon = null;
        foreach (var item in placedItems)
        {
            var data = item.GetComponent<ItemData>();
            if (data == null) continue;
            if (data.type == ItemType.Cup) cup = item;
            else if (data.type == ItemType.LemonSlice) lemon = item;
        }

        if (cup != null && lemon != null)
        {
            Vector3 spawnPos = (cup.transform.position + lemon.transform.position) / 2f;
            Destroy(cup);
            Destroy(lemon);
            placedItems.Clear();
            Instantiate(lemonCupPrefab, spawnPos, Quaternion.identity);
        }
    }
}
