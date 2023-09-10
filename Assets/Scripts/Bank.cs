using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bank : MonoBehaviour
{
    private static int money = 0;
    
    public static Bank Instance;

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

    public int Money => money;
    public object Sounds { get; }
}