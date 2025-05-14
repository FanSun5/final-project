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
        // �� ESC ֱ���˳�����ֻ�д������Ч��
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("���� ESC���˳���Ϸ");
            Application.Quit();
        }
    }

    public void PlayGame()
    {
        if (string.IsNullOrEmpty(gameSceneName))
        {
            Debug.LogWarning("MenuManager: gameSceneName δ���ã����ص�һ������");
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
            Debug.LogWarning("MenuManager: controlSceneName δ���ã����ص�һ������");
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
            Debug.LogWarning("MenuManager: menuSceneName δ���ã����ص�һ������");
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
            Debug.LogWarning("MenuManager: winEndSceneName δ���ã����ص�һ������");
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
            Debug.LogWarning("MenuManager: loseEndSceneName δ���ã����ص�һ������");
            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(loseEndSceneName);
        }
    }
}
