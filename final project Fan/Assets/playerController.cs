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
        if (!Physics.Raycast(playerCamera.position, playerCamera.forward, out RaycastHit hit, interactDistance))
            return;

        // —— 手上没东西：尝试捡起或生成 —— 
        if (heldItem == null)
        {
            if (hit.collider.TryGetComponent<LemonSpawner>(out var sp))
            {
                sp.GiveLemonToPlayer();
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
            // 1) 交单：只销毁被点中的那位顾客
            var cust = hit.collider.GetComponentInParent<Customer>();
            if (cust != null)
            {
                // 把类型先存下来
                ItemType deliveredType = heldItemData.type;
                bool correct = cust.orderType == deliveredType;

                Destroy(cust.gameObject);
                Destroy(heldItem);
                heldItem = null;
                heldItemData = null;

                // 传入本次交单的类型和对错
                FindObjectOfType<GameManager>()
                    .ServeOrder(deliveredType, correct);
                return;
            }

            // 2) 放到柜台
            var counter = hit.collider.GetComponentInParent<CounterBase>();
            if (counter != null && counter.holdPoint != null)
            {
                heldItem.transform.SetParent(counter.holdPoint);
                heldItem.transform.localPosition = Vector3.zero;
                heldItem.transform.localRotation = Quaternion.identity;
                heldItem.GetComponent<Rigidbody>().isKinematic = true;

                heldItem = null;
                heldItemData = null;
                return;
            }

            // 3) 其它情况，继续拿在手上
            heldItem.transform.SetParent(holdPosition);
            heldItem.transform.localPosition = Vector3.zero;
            heldItem.transform.localRotation = Quaternion.identity;
        }
    }

    void HandleInteraction()
    {
        if (!Physics.Raycast(playerCamera.position, playerCamera.forward, out RaycastHit hit, interactDistance))
            return;

        var cb = hit.collider.GetComponentInParent<CuttingBoard>();
        if (cb != null) { cb.TryCut(); return; }
        var tc = hit.collider.GetComponentInParent<TeaCounter>();
        if (tc != null) { tc.TryAddTea(); return; }
        // ClearCounter 不需要空格交互
    }

    public bool HoldingNothing() => heldItem == null;
    public void PickUpItem(GameObject item)
    {
        heldItem = item;
        heldItemData = item.GetComponent<ItemData>();
    }
}
