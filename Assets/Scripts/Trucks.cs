using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trucks : UpgradableItem
{
    private static float INCREMENT = 1f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            Upgrade(INCREMENT);
            AdjustTightness(Level);
        }
    }

    void  AdjustTightness(float value)
    {
        Level = value;
        Player.TruckTightness = value;
        Debug.Log("Trucks: " + Player.TruckTightness);
    }
}