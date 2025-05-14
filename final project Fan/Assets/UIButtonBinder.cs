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
        // 一进场景就解锁并显示鼠标
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (MenuManager.Instance == null)
        {
            Debug.LogError("UIManager: 找不到 MenuManager 单例！");
            return;
        }

        // —— 主菜单场景 —— 
        if (playButton != null)
            playButton.onClick.AddListener(MenuManager.Instance.PlayGame);

        if (controlButton != null)
            controlButton.onClick.AddListener(MenuManager.Instance.ShowControls);

        // —— 控制界面 —— 
        if (backFromControlButton != null)
            backFromControlButton.onClick.AddListener(MenuManager.Instance.BackToMenu);

        // —— 结束界面（胜利/失败都用） —— 
        if (backFromEndButton != null)
            backFromEndButton.onClick.AddListener(MenuManager.Instance.BackToMenu);
    }
}