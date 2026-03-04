using System.Threading.Tasks;
using TMPro;
using UnityEditor.Localization.Editor;
using UnityEngine;
using VContainer;

public class FoodViewDescription : MonoBehaviour
{
    [SerializeField] private RectTransform _rectTransform;

    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _hungerValueText;
    [SerializeField] private TextMeshProUGUI _healthValueText;

    [SerializeField] private GameObject _holder;

    private void OnEnable()
    {
        InputController.ShowDescriptionAction += Show;
        InputController.HideDescriptionAction += Hide;
            
        FoodView.EatFoodAction += Hide;
    }

    private void OnDisable()
    {
        InputController.ShowDescriptionAction -= Show;
        InputController.HideDescriptionAction -= Hide;

        FoodView.EatFoodAction -= Hide;
    }

    public async void Show(FoodView foodView)
    {
        if (foodView == null) 
            return;

        Vector3 screenPos = foodView.GetDescriptionPosition();

        _rectTransform.position = screenPos + Vector3.up * 200;

        _nameText.text = await LocalizationService.GetLocalizedStringAsync(foodView.Food.Name, LocalizationTable.Resources);
        _hungerValueText.text = foodView.Food.FoodValue.ToString();
        _healthValueText.text = foodView.Food.HealthValue.ToString();

        _holder.SetActive(true);
    }

    public void Hide(FoodView food)
    {
        _holder.SetActive(false);
    }
}
