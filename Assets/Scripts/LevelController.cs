using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    [SerializeField] private BoxCollider endPos;

    [SerializeField] private BoxCollider startPos;
    
    [SerializeField] private int totalPins = 10;

    [SerializeField] private PinController pinController;

    [SerializeField] private int waitTime = 3;

    private int count = 0;

    public static LevelController Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
   
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        InitializePins();
    }

    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.GAME.Camera.Follow = null;
            GameManager.GAME.Camera.m_Orbits[1].m_Height = 10;
            GameManager.GAME.Player.IsLevelFinished = true;
            GameManager.GAME.Player.CarryOn();
            GameManager.GAME.SoundManager.PlaySound(1);
            StartCoroutine(WaitAndCheckPins());
        }
    }
    
    
    private IEnumerator WaitAndCheckPins()
    {
        count++;
        Debug.Log("called: " + count + " times");
        yield return new WaitForSeconds(waitTime);
        pinController.CheckPins();
    }
    

    public void NextLevel()
    {
        GameManager.GAME.LoadNextSceneWithLoadingScreen();
    }

    public void InitializePins()
    {
    }
}