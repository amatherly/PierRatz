using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradableItem : MonoBehaviour
{
    protected static int level = 1;
    private int minLevel = 1;
    private int maxLevel = 10;
    private int[] upgradeCosts;
    private int[] upgrades;
    private bool IsMaxedOut => level == maxLevel;
    private bool CanUpgrade => !IsMaxedOut;
    [SerializeField] protected PlayerController player;

    void Start()
    {
        // player = FindObjectOfType<PlayerController>();
    }

    public virtual void Upgrade()
    {
        if (CanUpgrade) 
        {
            Debug.Log("Attempting to upgrade trucks");
            level += upgrades[level];
            ApplyNewEffects();
        }
    }

    public virtual void ApplyNewEffects()
    {
        
    }
    
    public int Level
    {
        get => level;
        set => level = value;
    }

    public int MaxLevel
    {
        get => maxLevel;
        set => maxLevel = value;
    }

    public int[] UpgradeCosts
    {
        get => upgradeCosts;
        set => upgradeCosts = value;
    }
    
    public PlayerController Player
    {
        get => player;
        set => player = value;
    }
    
    public int MinLevel
    {
        get => minLevel;
        set => minLevel = value;
    }
    
    public  int[] Upgrades
    {
        get => upgrades;
        set => upgrades = value;
    }
}
