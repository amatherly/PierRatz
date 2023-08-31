using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager GAME { get; private set; }

    [SerializeField] private PlayerController player;
    [SerializeField] private Bank bank;
    [SerializeField] private LevelController lvlController;
    [SerializeField] private CinemachineFreeLook camera;
    [SerializeField] private UIController uiController;
    [SerializeField] private SoundManager soundManager;

    private void Awake()
    {
        if (GAME == null)
        {
            GAME = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void LoadNextSceneWithLoadingScreen()
    {
        int sceneToLoad = (SceneManager.GetActiveScene().buildIndex + 1);
        LoadingScreenController.LoadScene(sceneToLoad);
    }

    public void ReloadGame()
    {
        // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
    public PlayerController Player => player;
    public Bank Bank => bank;
    public LevelController LevelController => lvlController;
    public CinemachineFreeLook Camera => camera;
    public UIController UIController => uiController;
}