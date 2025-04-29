using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CameraSwitcher : MonoBehaviour
{
    
    public Camera defaultCamera;
    
    public Camera triggerCamera;

    private bool isPlayerIn = false;
    private bool isUsingTriggerCam = false;

    void Start()
    {
        // ȷ��Ĭ��������� GameObject �� Active
        if (defaultCamera) defaultCamera.gameObject.SetActive(true);
        // ȷ������������� GameObject �� Inactive
        if (triggerCamera) triggerCamera.gameObject.SetActive(false);
    }

    void Update()
    {
        if (isPlayerIn && Input.GetKeyDown(KeyCode.Space))
        {
            isUsingTriggerCam = !isUsingTriggerCam;

            // �л������������� Active ״̬
            if (defaultCamera) defaultCamera.gameObject.SetActive(!isUsingTriggerCam);
            if (triggerCamera) triggerCamera.gameObject.SetActive(isUsingTriggerCam);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            isPlayerIn = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerIn = false;
            // ����ʱǿ���л�Ĭ�����
            if (isUsingTriggerCam)
            {
                isUsingTriggerCam = false;
                if (defaultCamera) defaultCamera.gameObject.SetActive(true);
                if (triggerCamera) triggerCamera.gameObject.SetActive(false);
            }
        }
    }
}
