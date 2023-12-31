using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bank : MonoBehaviour
{
    public float Money => money;
    public object Sounds { get; }

    private int money = 0;
    
    public void AddToBank(int value)
    {
        money += value;
    }

    public bool Buy(UpgradableItem item)
    {
        int tempMoney = money - item.UpgradeCosts[item.Level];
        
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
