using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Trucks : UpgradableItem
{
    private static int[] costs = { 100, 175, 250, 300, 375, 400, 1, 1, 1, 1, 1 };
    [SerializeField] private TMP_Text levelText;

private void Start()
    {
        UpgradeCosts = costs;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            Upgrade();
            AdjustTightness(Level);
            levelText.SetText(Level.ToString());
        }
    }

    void  AdjustTightness(int value)
    {
        Level = value;
        Player.TruckTightness = value;
        Debug.Log("Trucks: " + Player.TruckTightness);
    }
}