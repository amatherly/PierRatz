using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradableItem : MonoBehaviour
{

    private int level = 1;
    private int maxLevel = 10;
    private int[] upgradeCosts;
    
    
    
    private bool IsMaxedOut => level == maxLevel;
    private bool CanUpgrade => !IsMaxedOut;
    
    public void Upgrade()
    {
        if (CanUpgrade)
        {
            level++;
       
            // Remove cost from bank
        }
    }

    public void ApplyNewEffects()
    {
        
    }
}
