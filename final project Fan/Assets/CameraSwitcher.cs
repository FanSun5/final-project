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
        // 确保默认相机所在 GameObject 是 Active
        if (defaultCamera) defaultCamera.gameObject.SetActive(true);
        // 确保备用相机所在 GameObject 是 Inactive
        if (triggerCamera) triggerCamera.gameObject.SetActive(false);
    }

    void Update()
    {
        if (isPlayerIn && Input.GetKeyDown(KeyCode.Space))
        {
            isUsingTriggerCam = !isUsingTriggerCam;

            // 切换整个相机对象的 Active 状态
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
            // 出区时强制切回默认相机
            if (isUsingTriggerCam)
            {
                isUsingTriggerCam = false;
                if (defaultCamera) defaultCamera.gameObject.SetActive(true);
                if (triggerCamera) triggerCamera.gameObject.SetActive(false);
            }
        }
    }
}
