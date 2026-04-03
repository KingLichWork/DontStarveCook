using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using VContainer;

public class ShopUI : UIPanel
{
    [SerializeField] private Button _hideButton;
    [SerializeField] private Button _rewardGoldButton;
    [SerializeField] private Button _rewardFoodButton;

    [SerializeField] private TextMeshProUGUI _gold;

    [SerializeField] private Transform _upgradeParent;
    [SerializeField] private ShopUpgrade _upgradePrefab;

    [SerializeField] private List<ShopUpgrade> _upgrades;

    private UpgradesData _upgradesData;

    public event Action AddFoodRewardAction;

    [Inject]
    private void Construct(UpgradesData upgradesData)
    {
        _upgradesData = upgradesData;
    }

    private void OnEnable()
    {
        ResourcesWallet.OnResourcesCountChanged += SetGold;

        //_hideButton.onClick.AddListener(Hide);

        _rewardGoldButton.onClick.AddListener(AddGoldReward);
        _rewardFoodButton.onClick.AddListener(AddFoodReward);

        Init();
    }

    private void OnDisable()
    {
        _rewardGoldButton.onClick.RemoveListener(AddGoldReward);
        _rewardFoodButton.onClick.RemoveListener(AddFoodReward);

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

    private void AddGoldReward()
    {
        AdManager.ShowRewarded(onRewarded: () => ResourcesWallet.AddResource(ResourcesType.Gold, 50));
    }

    private void AddFoodReward()
    {
        AdManager.ShowRewarded(onRewarded: () => AddFoodRewardAction.Invoke());
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
