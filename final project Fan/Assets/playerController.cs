using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]
public class playerController : MonoBehaviour
{
    private CharacterController cc;

   
    public float speed = 4.5f;
    public float gravity = -9.81f;
    private float verticalVelocity = 0f;

    
    public float mouseSensitivity = 200f;
    public Transform playerCamera;
    private float yRotation = 0f;
    private float xRotation = 0f;

   
    public Transform holdPosition;
    public float interactDistance = 3f;
    private GameObject heldItem = null;
    private ItemData heldItemData = null;

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
        // 重新开始当前场景（可选）
        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        if (!isHiding)
            HandleMovement();

        HandleLook();

        if (Input.GetMouseButtonDown(0))
            HandlePickUpAndDrop();

        if (Input.GetKeyDown(KeyCode.Space))
            HandleInteraction();
    }

    void HandleMovement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;

        // 重力处理
        if (cc.isGrounded && verticalVelocity < 0)
            verticalVelocity = -1f;  // 保持贴地
        verticalVelocity += gravity * Time.deltaTime;
        move.y = verticalVelocity;

        cc.Move(move * speed * Time.deltaTime);
    }

    void HandleLook()
    {
        float mx = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float my = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        yRotation += mx;
        transform.rotation = Quaternion.Euler(0f, yRotation, 0f);

        xRotation = Mathf.Clamp(xRotation - my, -90f, 90f);
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    // Trigger 检测：玩家身上的 Trigger Collider 触发此方法
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Caught by enemy!");
            FindObjectOfType<GameManager>().NotifyPlayerDead();
        }
    }

    void HandlePickUpAndDrop()
    {
        if (isHiding) return;
        if (!Physics.Raycast(playerCamera.position, playerCamera.forward, out var hit, interactDistance))
            return;

        if (heldItem == null)
        {
            if (hit.collider.TryGetComponent<LemonSpawner>(out var lemonSp))
            {
                lemonSp.GiveLemonToPlayer();
            }
            else if (hit.collider.TryGetComponent<CupSpawner>(out var cupSp))
            {
                cupSp.GiveCupToPlayer();
            }
            else if (hit.collider.CompareTag("Item"))
            {
                heldItem = hit.collider.gameObject;
                heldItemData = heldItem.GetComponent<ItemData>();
                var rb = heldItem.GetComponent<Rigidbody>();
                if (rb) rb.isKinematic = true;
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
                bool correct = cust.orderType == heldItemData.type;
                Destroy(cust.gameObject);
                Destroy(heldItem);
                heldItem = null;
                heldItemData = null;
                FindObjectOfType<GameManager>().ServeOrder(cust.orderType, correct);
                return;
            }

            var counter = hit.collider.GetComponentInParent<CounterBase>();
            if (counter != null && counter.holdPoint != null)
            {
                if (!(counter is ClearCounter) && counter.HasItem) return;

                heldItem.transform.SetParent(counter.holdPoint);
                heldItem.transform.localPosition = Vector3.zero;
                heldItem.transform.localRotation = Quaternion.identity;
                var rb2 = heldItem.GetComponent<Rigidbody>();
                if (rb2) rb2.isKinematic = true;

                heldItem = null;
                heldItemData = null;
                return;
            }

            // 放回手上
            heldItem.transform.SetParent(holdPosition);
            heldItem.transform.localPosition = Vector3.zero;
            heldItem.transform.localRotation = Quaternion.identity;
        }
    }

    void HandleInteraction()
    {
        // 如果正在藏，按空格就出来
        if (isHiding)
        {
            // 先打开 CharacterController
            cc.enabled = true;

            transform.position = lastPosition;
            isHiding = false;
            return;
        }

        if (!Physics.Raycast(playerCamera.position, playerCamera.forward, out RaycastHit hit, interactDistance))
            return;

        // 如果射线打到 Locker
        if (hit.collider.GetComponentInParent<Locker>() is Locker locker)
        {
            // 记录上次位置
            lastPosition = transform.position;

            // 禁用 CharacterController，防止被“挤”出去
            cc.enabled = false;

            // 立刻瞬移到柜子里的隐藏点
            transform.position = locker.hidePosition.position;

            isHiding = true;
            return;
        }



        if (hit.collider.GetComponentInParent<CuttingBoard>() is CuttingBoard cb)
        {
            cb.TryCut();
            return;
        }
        if (hit.collider.GetComponentInParent<TeaCounter>() is TeaCounter tc)
        {
            tc.TryAddTea();
            return;
        }
        if (hit.collider.GetComponentInParent<IceCounter>() is IceCounter ic)
        {
            ic.TryAddIce();
            return;
        }
        if (hit.collider.GetComponentInParent<BobaCounter>() is BobaCounter bc)
        {
            bc.TryAddBoba();
            return;
        }
        if (hit.collider.GetComponentInParent<MilkCounter>() is MilkCounter mc)
        {
            mc.TryAddMilk();
            return;
        }
    }

    public bool HoldingNothing() => heldItem == null;

    public void PickUpItem(GameObject item)
    {
        heldItem = item;
        heldItemData = item.GetComponent<ItemData>();
    }

    public string GetHeldItemName()
    {
        return heldItemData != null
            ? heldItemData.type.ToString()
            : "None";
    }
}