// UIManager.cs
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Main Menu Buttons")]
    public Button playButton;
    public Button controlButton;

    [Header("Control Scene Buttons")]
    public Button backFromControlButton;

    [Header("End Scene Buttons")]
    public Button backFromEndButton;

    void Start()
    {
        // һ�������ͽ�������ʾ���
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (MenuManager.Instance == null)
        {
            Debug.LogError("UIManager: �Ҳ��� MenuManager ������");
            return;
        }

        // ���� ���˵����� ���� 
        if (playButton != null)
            playButton.onClick.AddListener(MenuManager.Instance.PlayGame);

        if (controlButton != null)
            controlButton.onClick.AddListener(MenuManager.Instance.ShowControls);

        // ���� ���ƽ��� ���� 
        if (backFromControlButton != null)
            backFromControlButton.onClick.AddListener(MenuManager.Instance.BackToMenu);

        // ���� �������棨ʤ��/ʧ�ܶ��ã� ���� 
        if (backFromEndButton != null)
            backFromEndButton.onClick.AddListener(MenuManager.Instance.BackToMenu);
    }
}