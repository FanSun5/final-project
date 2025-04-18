using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class interaction : MonoBehaviour
{
    public float interactionDistance = 5f;
    private GameObject currentTarget;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void HandleInteraction()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hitinfo, 2f))
        {
            if (hitinfo.transform.TryGetComponent<ClearCounter>(out ClearCounter counter))
            {
                counter.Interact();
            }


        }

    }
}
