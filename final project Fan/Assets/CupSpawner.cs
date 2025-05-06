using UnityEngine;

public class CupSpawner : CounterBase
{
    public GameObject cupPrefab;
    private playerController player;

    private void Start()
    {
        player = FindObjectOfType<playerController>();
    }

    public void GiveCupToPlayer()
    {
        if (player != null && player.HoldingNothing())
        {
            GameObject newCup = Instantiate(cupPrefab, holdPoint.position, holdPoint.rotation);
            newCup.GetComponent<Rigidbody>().isKinematic = true;
            newCup.transform.SetParent(player.holdPosition);
            newCup.transform.localPosition = Vector3.zero;
            newCup.transform.localRotation = Quaternion.identity;
            player.PickUpItem(newCup);
        }
    }
}
