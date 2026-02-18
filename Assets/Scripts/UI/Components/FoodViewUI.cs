using UnityEngine;
using UnityEngine.UI;

public class FoodViewUI : MonoBehaviour
{
    [SerializeField] private Image _image;

    private Food _food;

    public void SetFood(Food food)
    {
        _food = food;
        _image.sprite = _food.Sprite;
    }
}
