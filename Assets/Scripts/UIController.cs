using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    [SerializeField] private TMP_Text bankMoney;

    // [SerializeField] private HUD _HUD;
    
    private AudioSource audioSource;
    
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
