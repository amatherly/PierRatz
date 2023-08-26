using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradableItem : MonoBehaviour
{
    private static float level = 1;
    private static float minLevel = 1;
    private static float maxLevel = 10;
    private static int[] upgradeCosts;
    private bool IsMaxedOut => level == maxLevel;
    private bool CanUpgrade => !IsMaxedOut;
    [SerializeField]
    private PlayerController player;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }

    public virtual void Upgrade(float increment)
    {
        if (CanUpgrade)
        {
            level += increment;
            ApplyNewEffects();
        }
    }

    public virtual void ApplyNewEffects()
    {
        
    }
    
    public float Level
    {
        get => level;
        set => level = value;
    }

    public float MaxLevel
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
    
    public static float MinLevel
    {
        get => minLevel;
        set => minLevel = value;
    }
}
