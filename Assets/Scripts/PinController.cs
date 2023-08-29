using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinController : MonoBehaviour
{
    public int PinsDown
    {
        get => pinsDown;
        set => pinsDown = value;
    }

    private static readonly int priceForPins = 1;
    private int totalPins = 10;
    private int discount = 1;
    private int pinsDown = 0;
    [SerializeField]
    private Pin[] pins;
    
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
        foreach(Pin pin in pins)
        {
            Debug.Log("count: "+ count);
            Vector3 eulerAngles = pin.transform.rotation.eulerAngles;
            if (Mathf.Abs(eulerAngles.z) > .1f || Mathf.Abs(eulerAngles.x) > .1f)
            {
                pinsDown++;
            } 
        }
        Debug.Log("pins down: "+ pinsDown);
        CountPins();
    }

    public void CountPins()
    {
        GameManager.GAME.Bank.AddToBank(pinsDown * priceForPins);
        GameManager.GAME.UIController.SetBankUI();
        FindObjectOfType<ResultScreen>().InitializeResultScreen(pinsDown, totalPins);
    }
}
