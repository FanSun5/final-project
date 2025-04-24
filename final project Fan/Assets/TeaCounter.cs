using UnityEngine;

public class TeaCounter : CounterBase
{
    public GameObject lemonTeaPrefab; // 加完茶生成的柠檬茶Prefab
    public GameObject BobaTeaPrefab;
    public GameObject TeaPrefab;

    public void TryAddTea()
    {

        if (holdPoint.childCount == 0)
        {
            Debug.Log("");
            return;
        }

        GameObject itemOnBoard = holdPoint.GetChild(0).gameObject;
        ItemData data = itemOnBoard.GetComponent<ItemData>();

        if (data != null && data.type == ItemType.LemonCup)
        {
            Destroy(itemOnBoard);
            Instantiate(lemonTeaPrefab, holdPoint.position, holdPoint.rotation);
            Debug.Log("cut");
        }
        else
        {
            Debug.Log("cant cut");
        }
    }

    public void makeBobaTea()
    {
        GameObject itemOnBoard = holdPoint.GetChild(0).gameObject;
        ItemData data = itemOnBoard.GetComponent<ItemData>();

        if (data != null && data.type == ItemType.BobaCup)
        {
            Destroy(itemOnBoard);
            Instantiate(BobaTeaPrefab, holdPoint.position, holdPoint.rotation);
            Debug.Log("bobaTea");
        }
        else
        {
            Debug.Log("cant cut");
        }
    }

    public void makeTea()
    {
        GameObject itemOnBoard = holdPoint.GetChild(0).gameObject;
        ItemData data = itemOnBoard.GetComponent<ItemData>();

        if (data != null && data.type == ItemType.Cup)
        {
            Destroy(itemOnBoard);
            Instantiate(TeaPrefab, holdPoint.position, holdPoint.rotation);
            Debug.Log("bobaTea");
        }
        else
        {
            Debug.Log("cant cut");
        }
    }
}
