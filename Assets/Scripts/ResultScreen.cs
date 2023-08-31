using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ResultScreen : MonoBehaviour
{
    private TMP_Text totalMoney;
    private TMP_Text totalPins;

    private Button continueButton;
    private Button retryButton;

    private PinController pinController;
    private LevelController levelController;

    private GameObject resultScreen;
    private GameObject zeroStar;
    private GameObject oneStar;
    private GameObject twoStar;
    private GameObject threeStar;

    [SerializeField] private GameObject[] starsView;

    void Start()
    {
        resultScreen = GameObject.Find("ResultScreen");
        totalMoney = GameObject.Find("moneyResult").GetComponent<TMP_Text>();
        totalPins = GameObject.Find("pinResult").GetComponent<TMP_Text>();
        continueButton = GameObject.Find("ContinueButton").GetComponent<Button>();
        retryButton = GameObject.Find("RetryButton").GetComponent<Button>();
        resultScreen.SetActive(false);
        pinController = FindObjectOfType<PinController>();
    }

    public void InitializeResultScreen(int pinCount, int totalPins)
    {
        resultScreen.SetActive(true);
        retryButton.onClick.AddListener(() => GameManager.GAME.ReloadGame());
        continueButton.onClick.AddListener(() => GameManager.GAME.LoadNextSceneWithLoadingScreen());
        float stars = (totalPins - pinCount) / totalPins;

        Debug.Log(stars);

        if (stars > .75)
        {
            starsView[0].SetActive(true);
        }

        if (stars > .50 && stars < .75)
        {
            starsView[1].SetActive(true);
        }

        if (stars < .50 && stars > .25)
        {
            starsView[2].SetActive(true);
        }
        else
        {
            starsView[3].SetActive(true);
        }

        SetMoneyTotal();
        SetPinsTotal();
    }

    void SetPinsTotal()
    {
        totalPins.SetText("Pins downed: " + pinController.PinsDown.ToString());
    }

    void SetMoneyTotal()
    {
        totalMoney.SetText("$" + GameManager.GAME.Bank.Money.ToString());
    }
}