using System.Collections.Generic;
using TMPro;
using UnityEditor.U2D.Aseprite;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class ShopUI : UIPanel
{
    [SerializeField] private Button _hideButton;

    [SerializeField] private TextMeshProUGUI _gold;

    [SerializeField] private Transform _upgradeParent;
    [SerializeField] private ShopUpgrade _upgradePrefab;

    [SerializeField] private List<ShopUpgrade> _upgrades;

    private UpgradesData _upgradesData;

    [Inject]
    private void Cunstruct(UpgradesData upgradesData)
    {
        _upgradesData = upgradesData;
    }

    private void OnEnable()
    {
        ResourcesWallet.OnResourcesCountChanged += SetGold;

        //_hideButton.onClick.AddListener(Hide);

        Init();
    }

    private void OnDisable()
    {
        ResourcesWallet.OnResourcesCountChanged -= SetGold;

        //_hideButton.onClick.RemoveListener(Hide);
    }

    public void Init()
    {
        SetGold(ResourcesType.Gold, SaveManager.PlayerData.Gold);
        InitUprades();
    }

    private void SetGold(ResourcesType resourcesType, int value)
    {
        _gold.text = value.ToString();
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
