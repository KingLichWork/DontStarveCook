using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopUpgrade : MonoBehaviour
{
    [SerializeField] private Image _upgradeImage;

    [SerializeField] private Button _buyButton;
    [SerializeField] private GameObject _allBuyed;

    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _buyCostText;
    [SerializeField] private TextMeshProUGUI _progressText;

    private Upgrade _upgrade;

    private int _number;
    private int _level;

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
        _progressText.text = $"{_level}/{_upgrade.UpgradeInfo.Length}";
        _buyCostText.text = _upgrade.UpgradeInfo[_level].UpgradeCost.ToString();
        _upgradeImage.sprite = _upgrade.UpgradeInfo[_level].UpgradeSprite;

        _allBuyed.SetActive(SaveManager.PlayerData.Upgrades[_number] >= _upgrade.UpgradeInfo.Length);
    }

    private void Buy()
    {
        int cost = _upgrade.UpgradeInfo[_level].UpgradeCost;

        if (SaveManager.PlayerData.Gold >= cost)
        {
            SaveManager.PlayerData.Gold -= cost;
            _level = SaveManager.PlayerData.Upgrades[_number]++;
        }
    }
}
