
using TMPro;
using UnityEngine;

public class Trucks : UpgradableItem
{
    private int[] costs = { 100, 175, 250, 300, 375, 400, 1, 1, 1, 1, 1 };
    private int[] upgrades = { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
    [SerializeField] private TMP_Text levelText;

    private void Start()
    {
        Upgrades = upgrades;
        UpgradeCosts = costs;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            base.Upgrade();
            AdjustTightness(Level);
        }
    }
    public override void ApplyNewEffects()
    {
        Player.TruckTightness++;
        levelText.SetText("Level: " + Level.ToString());
    }
    void AdjustTightness(int value)
    {
        Level = value;
        Player.TruckTightness = value;
        Debug.Log("Trucks: " + Player.TruckTightness);
    }
}