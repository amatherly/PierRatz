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


    void Start()
    {
        InitializePins();
    }

    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PinCollider"))
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

    public void Pause()
    {
    }

    public void Resume()
    {
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene("game");
    }
    
    public void NextLevel()
    {
  
    }

    public void InitializePins()
    {
    }
}