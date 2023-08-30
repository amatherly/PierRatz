using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public SoundManager SoundManager => soundManager;

    public static GameManager GAME = null;

    [SerializeField] private  PlayerController player;
    [SerializeField] private  Bank BANK;
    [SerializeField] private  LevelController lvlController;
    [SerializeField] private  CinemachineFreeLook camera;
    [SerializeField] private  UIController UI_Controller;
    [SerializeField] private  SoundManager soundManager;

    private void Awake()
    {
        if (GAME == null)
        {
            GAME = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ReloadGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public GameManager Game => GAME;
    public PlayerController Player => player;
    public Bank Bank => BANK;
    public LevelController LvlController => lvlController;
    public CinemachineFreeLook Camera => camera;
    public UIController UIController => UI_Controller;
}