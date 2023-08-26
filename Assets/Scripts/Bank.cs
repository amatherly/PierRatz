using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bank : MonoBehaviour
{
    private float money = 500;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddToBank(float value)
    {
        money += value;
    }

    public bool Buy(UpgradableItem item)
    {
        float tempMoney = money - item.UpgradeCosts[item.Level];
        
        if (tempMoney >= 0)
        {
            item.Upgrade();
            money = tempMoney;
            Debug.Log("Bought upgrade!");
            return true;
        }

        return false;
    }
}
