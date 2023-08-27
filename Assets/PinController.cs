using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinController : MonoBehaviour
{
    private static readonly int priceForPins = 1;
    private int discount = 1;
    private int pinsDown = 0;
    [SerializeField]
    private Pin[] pins;

    public void CheckPins()
    {
        int count = 0;
        foreach(Pin pin in pins)
        {
            Debug.Log("count: "+ count);
            // Debug.Log("Checking pin: " + pin.name + "Rotation: " + pin.transform.rotation.z);
            Vector3 eulerAngles = pin.transform.rotation.eulerAngles;
            if (Mathf.Abs(eulerAngles.z) > 1f || Mathf.Abs(eulerAngles.x) > 1f)
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
    }
}
