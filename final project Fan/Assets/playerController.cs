using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    private CharacterController cc;
    public float speed;
    private Vector3 playerMovement;

    public float mouseSensitivity;
    public Transform playerCamera;
    private float yRotation = 0;
    private float xRotation = 0;

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
        HandlePickUpAndDrop();
        HandleInteraction();
    }

    void MovePlayer()
    {
        float inputMoveX = Input.GetAxis("Horizontal");
        float inputMoveZ = Input.GetAxis("Vertical");
        Vector3 move = (transform.forward * inputMoveZ) + (transform.right * inputMoveX);

        cc.Move(move * speed * Time.deltaTime);
    }

    void CameraLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        yRotation += mouseX;
        transform.rotation = Quaternion.Euler(0f, yRotation, 0);

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90, 90);

        playerCamera.localRotation = Quaternion.Euler(xRotation, 0, 0);
    }

    void HandlePickUpAndDrop()
    {
        if (Input.GetMouseButtonDown(0)) // 左键
        {
            if (heldItem == null)
            {
                // 手上没拿东西，准备拿
                RaycastHit hit;
                if (Physics.Raycast(playerCamera.position, playerCamera.forward, out hit, interactDistance))
                {
                    if (hit.collider.GetComponent<LemonSpawner>())
                    {
                        hit.collider.GetComponent<LemonSpawner>().GiveLemonToPlayer();
                    }
                    else if (hit.collider.CompareTag("Item"))
                    {
                        heldItem = hit.collider.gameObject;
                        heldItemData = heldItem.GetComponent<ItemData>();
                        heldItem.GetComponent<Rigidbody>().isKinematic = true;
                        heldItem.transform.SetParent(holdPosition); // 临时拿在手上
                        heldItem.transform.localPosition = Vector3.zero;
                        heldItem.transform.localRotation = Quaternion.identity;
                    }
                }
            }
            else
            {
                // 手上有东西，准备放
                RaycastHit hit;
                if (Physics.Raycast(playerCamera.position, playerCamera.forward, out hit, interactDistance))
                {
                    var counter = hit.collider.GetComponentInParent<CounterBase>();
                    if (counter != null && counter.holdPoint != null)
                    {
                        // 物品放置在柜台的holdPoint上！
                        heldItem.transform.SetParent(counter.holdPoint);
                        heldItem.transform.localPosition = Vector3.zero;  // 物品归零位置
                        heldItem.transform.localRotation = Quaternion.identity;  // 物品归零旋转
                        heldItem.GetComponent<Rigidbody>().isKinematic = true;  // 防止掉落

                        // 清除heldItem
                        heldItem = null;
                        heldItemData = null;
                        return;
                    }
                }

                // 如果没对着柜台，物品仍然停在手上（不掉落到地上）
                heldItem.transform.SetParent(holdPosition);
                heldItem.transform.localPosition = Vector3.zero;
                heldItem.transform.localRotation = Quaternion.identity;
            }
        }
    }




    void HandleInteraction()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // 确保是按下空格
        {
            RaycastHit hit;
            if (Physics.Raycast(playerCamera.position, playerCamera.forward, out hit, interactDistance))
            {
                Debug.Log("空格打到了：" + hit.collider.gameObject.name);  // 打印 Raycast 打到的物体名字

                CuttingBoard cuttingBoard = hit.collider.GetComponentInParent<CuttingBoard>();
                if (cuttingBoard != null)
                {
                    Debug.Log("成功打到 CuttingBoard，调用 TryCut！");
                    cuttingBoard.TryCut();  // 正确调用 TryCut
                    return;
                }

                TeaCounter teaCounter = hit.collider.GetComponentInParent<TeaCounter>();
                if (teaCounter != null)
                {
                    if (heldItem != null)
                    {
                        teaCounter.TryAddTea(heldItem);
                    }
                    return;
                }

                ClearCounter clearCounter = hit.collider.GetComponentInParent<ClearCounter>();
                if (clearCounter != null)
                {
                    Debug.Log("ClearCounter 不需要空格交互。");
                    return;
                }
            }
        }
    }

    public bool HoldingNothing()
    {
        return heldItem == null;
    }

    public void PickUpItem(GameObject item)
    {
        heldItem = item;
        heldItemData = heldItem.GetComponent<ItemData>();
    }
}
