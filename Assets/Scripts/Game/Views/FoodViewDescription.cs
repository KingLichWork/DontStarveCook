using TMPro;
using UnityEngine;

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
        Vector3 targetPos = screenPos + Vector3.up * 50;

        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        Vector2 size = _rectTransform.rect.size;
        Vector2 pivot = _rectTransform.pivot;

        float minX = size.x * pivot.x;
        float maxX = screenWidth - size.x * (1 - pivot.x);

        float minY = size.y * pivot.y;
        float maxY = screenHeight - size.y * (1 - pivot.y);

        float clampedX = Mathf.Clamp(targetPos.x, minX, maxX);
        float clampedY = Mathf.Clamp(targetPos.y, minY, maxY);

        _rectTransform.position = new Vector3(clampedX, clampedY, targetPos.z);

        _nameText.text = await LocalizationService.GetLocalizedStringAsync(
            foodView.Food.Name,
            LocalizationTable.Resources
        );

        _hungerValueText.text = foodView.Food.FoodValue.ToString();
        _healthValueText.text = foodView.Food.HealthValue.ToString();

        _holder.SetActive(true);
    }

    public void Hide(FoodView food)
    {
        _holder.SetActive(false);
    }
}
