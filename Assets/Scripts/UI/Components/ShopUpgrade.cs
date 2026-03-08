using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopUpgrade : MonoBehaviour
{
    [SerializeField] private Image _upgradeImage;

    [SerializeField] private Button _buyButton;
    [SerializeField] private GameObject _allBuyed;

    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _descriptionText;
    [SerializeField] private TextMeshProUGUI _buyCostText;
    [SerializeField] private TextMeshProUGUI _progressText;

    private Upgrade _upgrade;

    private int _number;
    private int _level;

    public static event Action<UpgradeType> BuyUpgradeAction;

    private void OnEnable()
    {
        _buyButton.onClick.AddListener(Buy);
    }

    private void OnDisable()
    {
        _buyButton.onClick.RemoveListener(Buy);
    }

    public void Init(Upgrade upgrade, int number)
    {
        _upgrade = upgrade;
        _number = number;
        _level = SaveManager.PlayerData.Upgrades[_number];

        _nameText.text = LocalizationService.GetLocalizedString(_upgrade.Name, LocalizationTable.UI);
        _descriptionText.text = LocalizationService.GetLocalizedString(_upgrade.Name + "Desc", LocalizationTable.UI, new {value = _upgrade.ValuePerLevel});

        ChangeView();
    }

    private void ChangeView()
    {
        _progressText.text = $"{_level}/{_upgrade.UpgradeInfo.Length}";
        _buyCostText.text = _upgrade.UpgradeInfo[_level].UpgradeCost.ToString();
        _upgradeImage.sprite = _upgrade.UpgradeInfo[_level].UpgradeSprite;

        bool isAllBuyed = SaveManager.PlayerData.Upgrades[_number] >= _upgrade.UpgradeInfo.Length;

        _allBuyed.SetActive(isAllBuyed);
        _buyButton.gameObject.SetActive(!isAllBuyed);
    }

    private void Buy()
    {
        int cost = _upgrade.UpgradeInfo[_level].UpgradeCost;

        if (ResourcesWallet.SpendResource(ResourcesType.Gold, cost))
        {
            _level = SaveManager.PlayerData.Upgrades[_number]++;
            
            BuyUpgradeAction.Invoke(_upgrade.Type);
            ChangeView();
        }
    }
}
