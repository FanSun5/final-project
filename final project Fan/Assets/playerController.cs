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
        if (Input.GetMouseButtonDown(0)) // ���
        {
            if (heldItem == null)
            {
                // ����û�ö�����׼����
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
                        heldItem.transform.SetParent(holdPosition); // ��ʱ��������
                        heldItem.transform.localPosition = Vector3.zero;
                        heldItem.transform.localRotation = Quaternion.identity;
                    }
                }
            }
            else
            {
                // �����ж�����׼����
                RaycastHit hit;
                if (Physics.Raycast(playerCamera.position, playerCamera.forward, out hit, interactDistance))
                {
                    var counter = hit.collider.GetComponentInParent<CounterBase>();
                    if (counter != null && counter.holdPoint != null)
                    {
                        // ��Ʒ�����ڹ�̨��holdPoint�ϣ�
                        heldItem.transform.SetParent(counter.holdPoint);
                        heldItem.transform.localPosition = Vector3.zero;  // ��Ʒ����λ��
                        heldItem.transform.localRotation = Quaternion.identity;  // ��Ʒ������ת
                        heldItem.GetComponent<Rigidbody>().isKinematic = true;  // ��ֹ����

                        // ���heldItem
                        heldItem = null;
                        heldItemData = null;
                        return;
                    }
                }

                // ���û���Ź�̨����Ʒ��Ȼͣ�����ϣ������䵽���ϣ�
                heldItem.transform.SetParent(holdPosition);
                heldItem.transform.localPosition = Vector3.zero;
                heldItem.transform.localRotation = Quaternion.identity;
            }
        }
    }




    void HandleInteraction()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // ȷ���ǰ��¿ո�
        {
            RaycastHit hit;
            if (Physics.Raycast(playerCamera.position, playerCamera.forward, out hit, interactDistance))
            {
                Debug.Log("�ո���ˣ�" + hit.collider.gameObject.name);  // ��ӡ Raycast �򵽵���������

                CuttingBoard cuttingBoard = hit.collider.GetComponentInParent<CuttingBoard>();
                if (cuttingBoard != null)
                {
                    Debug.Log("�ɹ��� CuttingBoard������ TryCut��");
                    cuttingBoard.TryCut();  // ��ȷ���� TryCut
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
                    Debug.Log("ClearCounter ����Ҫ�ո񽻻���");
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
