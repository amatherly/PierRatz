using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager GAME = null;
    
    [Header("Game Components")]
    [SerializeField] private Bank bank;
    [SerializeField] private LevelController lvlController;
    [SerializeField] private UIController uiController;
    [SerializeField] private SoundManager soundManager;
    
    private void Awake()
    {
        if (GAME == null)
        {
            GAME = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (GAME != this)
        {
            Destroy(this.gameObject);
        }
    }
    
    
    public void Start()
    {
 

    }

    private void InitializeGameComponents()
    {
        
        if (bank == null)
        {
            bank = FindObjectOfType<Bank>();
        }

        if (lvlController == null)
        {
            lvlController = FindObjectOfType<LevelController>();
        }

        if (uiController == null)
        {
            uiController = FindObjectOfType<UIController>();
        }

        if (soundManager == null)
        {
            soundManager = FindObjectOfType<SoundManager>();
        }
    }

    public void LoadNextSceneWithLoadingScreen()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        Debug.Log("Unloading current scene with index: " + SceneManager.GetActiveScene().buildIndex);
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);

        Debug.Log("Loading next scene with index: " + nextSceneIndex);
        SceneManager.LoadScene(nextSceneIndex, LoadSceneMode.Additive);
        InitializeGameComponents();
    }

    public void ReloadGame()
    {
        int sceneToLoad = (SceneManager.GetActiveScene().buildIndex);
        LoadingScreenController.LoadScene(sceneToLoad);
    }


    public void Pause()
    {
    }

    public void Resume()
    {
    }

    public SoundManager SoundManager => soundManager;
    public Bank Bank => bank;
    public UIController UIController => uiController;
}