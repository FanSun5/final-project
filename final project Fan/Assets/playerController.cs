// playerController.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    private CharacterController cc;
    public float speed;
    public float mouseSensitivity;
    public Transform playerCamera;
    private float yRotation = 0f;
    private float xRotation = 0f;

    private GameObject heldItem = null;
    private ItemData heldItemData = null;
    public Transform holdPosition;
    public float interactDistance = 3f;

    public bool isHiding = false;
    private Vector3 lastPosition;

    void Start()
    {
        cc = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        MovePlayer();
        CameraLook();

        if (Input.GetMouseButtonDown(0))
            HandlePickUpAndDrop();

        if (Input.GetKeyDown(KeyCode.Space))
            HandleInteraction();
    }

    void MovePlayer()
    {
        if (isHiding) return;
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 dir = transform.forward * z + transform.right * x;
        cc.Move(dir * speed * Time.deltaTime);
    }

    void CameraLook()
    {
        float mx = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float my = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        yRotation += mx;
        transform.rotation = Quaternion.Euler(0f, yRotation, 0f);
        xRotation = Mathf.Clamp(xRotation - my, -90f, 90f);
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    void HandlePickUpAndDrop()
    {
        if (isHiding) return;
        if (!Physics.Raycast(playerCamera.position, playerCamera.forward, out RaycastHit hit, interactDistance))
            return;

        if (heldItem == null)
        {
            if (hit.collider.TryGetComponent<LemonSpawner>(out var lemonSp))
            {
                lemonSp.GiveLemonToPlayer();
                return;
            }
            else if (hit.collider.TryGetComponent<CupSpawner>(out var cupSp))
            {
                cupSp.GiveCupToPlayer();
                return;
            }
            else if (hit.collider.CompareTag("Item"))
            {
                heldItem = hit.collider.gameObject;
                heldItemData = heldItem.GetComponent<ItemData>();
                heldItem.GetComponent<Rigidbody>().isKinematic = true;
                heldItem.transform.SetParent(holdPosition);
                heldItem.transform.localPosition = Vector3.zero;
                heldItem.transform.localRotation = Quaternion.identity;
            }
        }
        else
        {
            if (hit.collider.TryGetComponent<TrashBin>(out var trash))
            {
                Destroy(heldItem);
                heldItem = null;
                heldItemData = null;
                return;
            }

            var cust = hit.collider.GetComponentInParent<Customer>();
            if (cust != null)
            {
                ItemType deliveredType = heldItemData.type;
                bool correct = cust.orderType == deliveredType;
                Destroy(cust.gameObject);
                Destroy(heldItem);
                heldItem = null;
                heldItemData = null;
                FindObjectOfType<GameManager>().ServeOrder(deliveredType, correct);
                return;
            }

            var counter = hit.collider.GetComponentInParent<CounterBase>();
            if (counter != null && counter.holdPoint != null)
            {
                if (!(counter is ClearCounter) && counter.HasItem)
                    return;

                heldItem.transform.SetParent(counter.holdPoint);
                heldItem.transform.localPosition = Vector3.zero;
                heldItem.transform.localRotation = Quaternion.identity;
                heldItem.GetComponent<Rigidbody>().isKinematic = true;

                heldItem = null;
                heldItemData = null;
                return;
            }

            heldItem.transform.SetParent(holdPosition);
            heldItem.transform.localPosition = Vector3.zero;
            heldItem.transform.localRotation = Quaternion.identity;
        }
    }

    void HandleInteraction()
    {
        if (isHiding)
        {
            transform.position = lastPosition;
            isHiding = false;
            return;
        }

        if (!Physics.Raycast(playerCamera.position, playerCamera.forward, out RaycastHit hit, interactDistance))
            return;

        if (hit.collider.GetComponentInParent<Locker>() is Locker locker)
        {
            lastPosition = transform.position;
            transform.position = locker.hidePosition.position;
            isHiding = true;
            return;
        }
        if (hit.collider.GetComponentInParent<CuttingBoard>() is CuttingBoard cb) { cb.TryCut(); return; }
        if (hit.collider.GetComponentInParent<TeaCounter>() is TeaCounter tc) { tc.TryAddTea(); return; }
        if (hit.collider.GetComponentInParent<IceCounter>() is IceCounter ic) { ic.TryAddIce(); return; }
        if (hit.collider.GetComponentInParent<BobaCounter>() is BobaCounter bc) { bc.TryAddBoba(); return; }
        if (hit.collider.GetComponentInParent<MilkCounter>() is MilkCounter mc) { mc.TryAddMilk(); return; }
    }

    public bool HoldingNothing() => heldItem == null;
    public void PickUpItem(GameObject item)
    {
        heldItem = item;
        heldItemData = item.GetComponent<ItemData>();
    }
}
