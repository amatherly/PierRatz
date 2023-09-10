using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    [SerializeField] private TMP_Text bankMoney;

    private AudioSource audioSource;

    private static UIController Instance = null;
    
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
        audioSource = GetComponent<AudioSource>();
        SetBankUI();
        ShowHUD();
    }

    void Update()
    {
        
    }

    public void LoadNextLevel()
    {
        Debug.Log("Loading level: " + SceneManager.GetActiveScene().buildIndex + 1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void SetBankUI()
    {
        bankMoney.SetText("$" +  GameManager.GAME.Bank.Money);
    }

    public void ShowHUD()
    {
        
    }
    

}
