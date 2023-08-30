using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bank : MonoBehaviour
{
    
    private static int money = 0;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        LoadMoney();
    }
    
    
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
    

    public void SaveMoney()
    {
        PlayerPrefs.SetInt("Money", money);
    }

    public void LoadMoney()
    {
        money = PlayerPrefs.GetInt("Money", 0);
    }
    
    public float Money => money;
    public object Sounds { get; }
}
