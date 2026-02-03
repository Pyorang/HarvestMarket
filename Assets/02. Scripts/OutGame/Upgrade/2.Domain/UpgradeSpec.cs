using System;

[Serializable]
public class UpgradeSpec
{
    public UpgradeType Type;
    public string Name;
    public string Description;
    public int MaxLevel;
    public float BaseCost;
    public float CostMultiplier;
    public float BaseValue;
    public float ValueMultiplier;
}
