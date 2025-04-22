using UnityEngine;

public class LemonSpawner : CounterBase
{
    public GameObject lemonPrefab; // 要生成的柠檬Prefab
    private playerController player;

    private void Start()
    {
        player = FindObjectOfType<playerController>();
    }

    public void GiveLemonToPlayer()
    {
        if (player != null && player.HoldingNothing())
        {
            GameObject newLemon = Instantiate(lemonPrefab, holdPoint.position, holdPoint.rotation);
            newLemon.GetComponent<Rigidbody>().isKinematic = true;
            newLemon.transform.SetParent(player.holdPosition);
            newLemon.transform.localPosition = Vector3.zero;
            newLemon.transform.localRotation = Quaternion.identity;
            player.PickUpItem(newLemon);
        }
    }
}
