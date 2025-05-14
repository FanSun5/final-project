using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

   
    public string gameSceneName;
   
    public string controlSceneName;
    
    public string menuSceneName;
    
    public string winEndSceneName;
    
    public string loseEndSceneName;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        // 按 ESC 直接退出程序（只有打包后生效）
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("按下 ESC，退出游戏");
            Application.Quit();
        }
    }

    public void PlayGame()
    {
        if (string.IsNullOrEmpty(gameSceneName))
        {
            Debug.LogWarning("MenuManager: gameSceneName 未设置，加载第一个场景");
            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(gameSceneName);
        }
    }

    public void ShowControls()
    {
        if (string.IsNullOrEmpty(controlSceneName))
        {
            Debug.LogWarning("MenuManager: controlSceneName 未设置，加载第一个场景");
            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(controlSceneName);
        }
    }

    public void BackToMenu()
    {
        if (string.IsNullOrEmpty(menuSceneName))
        {
            Debug.LogWarning("MenuManager: menuSceneName 未设置，加载第一个场景");
            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(menuSceneName);
        }
    }

    public void GoToWinEndScene()
    {
        if (string.IsNullOrEmpty(winEndSceneName))
        {
            Debug.LogWarning("MenuManager: winEndSceneName 未设置，加载第一个场景");
            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(winEndSceneName);
        }
    }

    public void GoToLoseEndScene()
    {
        if (string.IsNullOrEmpty(loseEndSceneName))
        {
            Debug.LogWarning("MenuManager: loseEndSceneName 未设置，加载第一个场景");
            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(loseEndSceneName);
        }
    }
}
