using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;


public class GameManager : MonoBehaviour
{


    public static GameManager GAME = null;
    
    [SerializeField]
    private PlayerController player;
    
    [SerializeField]
    private Bank BANK;
    
    [SerializeField]
    private LevelController lvlController;
    
    [SerializeField]
    private CinemachineFreeLook camera;

    [SerializeField]
    private UIController UI_Controller;
    // Start is called before the first frame update
    
    private void Awake()
    {
        
        if (GAME == null)
        {
            GAME = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
    }
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
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
