using UnityEngine;
using UnityEngine.UI;

public class FoodViewUI : FoodView
{
    [SerializeField] private Image _image;
    [SerializeField] private RectTransform _rect;

    public override void SetFood(Food food)
    {
        base.SetFood(food);
        _image.sprite = _food.Sprite;
    }

    public override void StartDrag(Vector2 pointerPos)
    {
        transform.SetAsLastSibling();
    }

    public override void Drag(Vector2 pointerPos)
    {
        _rect.position = pointerPos;
    }
}
