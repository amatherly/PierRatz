using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradableItem : MonoBehaviour
{

    private static int level = 1;
    private static int minLevel = 1;
    private static int maxLevel = 10;
    private static int[] upgradeCosts;
    private static int[] upgrades;

    private bool IsMaxedOut => level == maxLevel;
    private bool CanUpgrade => !IsMaxedOut;
    [SerializeField]
    private PlayerController player;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }

    public virtual void Upgrade()
    {
        if (CanUpgrade)
        {
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
    
    public static int MinLevel
    {
        get => minLevel;
        set => minLevel = value;
    }
    
    public static int[] Upgrades
    {
        get => upgrades;
        set => upgrades = value;
    }
}
