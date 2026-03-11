using System;
using System.Threading.Tasks;
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

    public async void Init(Upgrade upgrade, int number)
    {
        _upgrade = upgrade;
        _number = number;
        _level = SaveManager.PlayerData.Upgrades[_number];
 
        _nameText.text = await LocalizationService.GetLocalizedStringAsync(_upgrade.Name, LocalizationTable.UI);

        ChangeView();
    }

    private async void ChangeView()
    {
        _progressText.text = _level.ToString();
        _buyCostText.text = (_upgrade.UpgradeCost * (_level + 1)).ToString();
        _upgradeImage.sprite = _upgrade.UpgradeInfo[Mathf.Clamp(_level, 0, _upgrade.UpgradeInfo.Length - 1)].UpgradeSprite;

        _descriptionText.text = await LocalizationService.GetLocalizedStringAsync(_upgrade.Name + "Desc", LocalizationTable.UI, new {value = _upgrade.ValuePerLevel * (_level + 1)});
    }

    private void Buy()
    {
        int cost = _upgrade.UpgradeCost * (_level + 1);

        if (ResourcesWallet.SpendResource(ResourcesType.Gold, cost))
        {
            SaveManager.PlayerData.Upgrades[_number]++;
            _level = SaveManager.PlayerData.Upgrades[_number];
            
            BuyUpgradeAction.Invoke(_upgrade.Type);
            ChangeView();
        }
    }
}
