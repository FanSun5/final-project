// MenuManager.cs
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Tooltip("游戏主场景名")]
    public string gameSceneName;
    [Tooltip("说明界面场景名")]
    public string controlSceneName;
    [Tooltip("主菜单场景名")]
    public string menuSceneName;

    /// <summary>点击 Play 按钮</summary>
    public void PlayGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    /// <summary>点击 Control 按钮</summary>
    public void ShowControls()
    {
        SceneManager.LoadScene(controlSceneName);
    }

    /// <summary>说明界面点击返回菜单</summary>
    public void BackToMenu()
    {
        SceneManager.LoadScene(menuSceneName);
    }

    /// <summary>退出游戏（打包后生效）</summary>
    public void QuitGame()
    {
        Application.Quit();
    }
}
