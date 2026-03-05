using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : UIPanel
{
    [SerializeField] private Button _hideButton;

    [SerializeField] private TextMeshProUGUI _gold;

    [SerializeField] private Transform _upgradeParent;
    [SerializeField] private ShopUpgrade _upgradePrefab;

    [SerializeField] private List<ShopUpgrade> _upgrades;
    [SerializeField] private UpgradesData _upgradesData;

    private void OnEnable()
    {
        GoldGetter.GetGoldAction += SetGold;

        //_hideButton.onClick.AddListener(Hide);

        Init();
    }

    private void OnDisable()
    {
        GoldGetter.GetGoldAction -= SetGold;

        //_hideButton.onClick.RemoveListener(Hide);
    }

    public void Init()
    {
        SetGold();
        InitUprades();
    }

    private void SetGold()
    {
        _gold.text = SaveManager.PlayerData.Gold.ToString();
    }

    private void InitUprades()
    {
        for (int i = 0; i < _upgradesData.Upgrades.Count; i++)
        {
            ShopUpgrade shopUpgrade = Instantiate(_upgradePrefab, _upgradeParent);
            shopUpgrade.Init(_upgradesData.Upgrades[i], i);
        }
    }
}
