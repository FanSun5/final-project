// MenuManager.cs
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Tooltip("��Ϸ��������")]
    public string gameSceneName;
    [Tooltip("˵�����泡����")]
    public string controlSceneName;
    [Tooltip("���˵�������")]
    public string menuSceneName;

    /// <summary>��� Play ��ť</summary>
    public void PlayGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    /// <summary>��� Control ��ť</summary>
    public void ShowControls()
    {
        SceneManager.LoadScene(controlSceneName);
    }

    /// <summary>˵�����������ز˵�</summary>
    public void BackToMenu()
    {
        SceneManager.LoadScene(menuSceneName);
    }

    /// <summary>�˳���Ϸ���������Ч��</summary>
    public void QuitGame()
    {
        Application.Quit();
    }
}
