using TMPro;
using UnityEngine;

public class FoodViewDescription : MonoBehaviour
{
    [SerializeField] private RectTransform _rectTransform;

    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _valueText;

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

    public void Show(FoodView foodView)
    {
        if (foodView == null) 
            return;

        Vector3 screenPos = foodView.GetDescriptionPosition();

        _rectTransform.position = screenPos + Vector3.up * 200;

        _nameText.text = foodView.Food.Name;
        _valueText.text = foodView.Food.FoodValue.ToString();

        _holder.SetActive(true);
    }

    public void Hide(FoodView food)
    {
        _holder.SetActive(false);
    }
}
