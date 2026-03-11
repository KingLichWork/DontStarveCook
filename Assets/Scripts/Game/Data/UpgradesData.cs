using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "UpgradesData", menuName = "Game/UpgradesData")]
public class UpgradesData : ScriptableObject
{
    [SerializeField] private List<Upgrade> _upgrades = new();

    public IReadOnlyList<Upgrade> Upgrades => _upgrades;

    public int GetUpgradeValue(UpgradeType type)
    {
        foreach (var upgrade in _upgrades)
        {
            if(upgrade.Type == type)
                return upgrade.ValuePerLevel * SaveManager.PlayerData.Upgrades[(int)type];
        }

        return 0;
    }
}

[Serializable]
public class Upgrade
{
    [SerializeField] private string _name;
    [SerializeField] private UpgradeType _type;
    [SerializeField] private int _valuePerLevel;
    [SerializeField] private int _upgradeCost;

    [SerializeField] private UpgradeInfo[] _upgradeInfo = new UpgradeInfo[4];

    public string Name => _name;
    public UpgradeType Type => _type;
    public int ValuePerLevel => _valuePerLevel;
    public int UpgradeCost => _upgradeCost;

    public UpgradeInfo[] UpgradeInfo => _upgradeInfo;
}

[Serializable]
public class UpgradeInfo
{
    [SerializeField] private Sprite _upgradeSprite;

    public Sprite UpgradeSprite => _upgradeSprite;
}

public enum UpgradeType
{
    ExtractCount,
    AutoExtract, 
    MaxHunger,
    MaxHealth

    //CookingSpeed
    //PassiveGold
    //OvercookingTime+
}
