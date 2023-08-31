using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinController : MonoBehaviour
{


    private static readonly int priceForPins = 1;
    private int totalPins = 10;
    private static int discount = 1;
    private int pinsDown = 0;
    private Vector3[] originalPositions = new Vector3[10];
    [SerializeField]
    private Pin[] pins;
    
    
    public static PinController Instance { get; private set; }

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

    public void Start()
    {
        for (int i = 0; i < totalPins; i++)
        {
            originalPositions[i] = pins[i].transform.position;
        }
    }

    public bool HavePinsStopped()
    {
        foreach (Pin pin in pins)
        {
            Rigidbody pinRb = pin.GetComponent<Rigidbody>();
            if (pinRb.velocity.magnitude > .1f || pinRb.angularVelocity.magnitude > .1f)
            {
                return false;
            }
        }
        return true;
    }
    
    public void CheckPins()
    {
        int count = 0;
        for(int i = 0 ; i < totalPins; i++)
        {
            if (Math.Abs(pins[i].transform.position.x - originalPositions[i].x) > .1f)
            {
                pinsDown++;
                count++;
            }
        }
        Debug.Log("pins down: "+ pinsDown);
        CountPins();
    }

    public void CountPins()
    {
        GameManager.GAME.Bank.AddToBank(pinsDown * priceForPins);
        GameManager.GAME.UIController.SetBankUI();
        GameManager.GAME.Bank.SaveMoney();
        FindObjectOfType<ResultScreen>().InitializeResultScreen(pinsDown, totalPins);
    }
    
    public int PinsDown
    {
        get => pinsDown;
        set => pinsDown = value;
    }
}
