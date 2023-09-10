using System.Collections;
using Cinemachine;
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
    
    public static LevelController Instance = null;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (Instance != this)
        {
            Destroy(this.gameObject);
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
            FindObjectOfType<CinemachineFreeLook>().Follow = null;
            FindObjectOfType<CinemachineFreeLook>().m_Orbits[1].m_Height = 10;
            FindObjectOfType<PlayerController>().IsLevelFinished = true;
            FindObjectOfType<PlayerController>().CarryOn();
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