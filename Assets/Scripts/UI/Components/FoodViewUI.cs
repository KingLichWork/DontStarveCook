using UnityEngine;
using UnityEngine.UI;

public class FoodViewUI : FoodView
{
    [SerializeField] private Image _image;
    [SerializeField] private RectTransform _rect;

    private Station _station;
    private Transform _stationParent;
    private Camera _mainCamera;
    public override void SetFood(Food food)
    {
        base.SetFood(food);
        _image.sprite = _food.Sprite;
    }

    public override void StartDrag(Vector2 pointerPos)
    {
        if(_mainCamera == null)
            _mainCamera = Camera.main;

        transform.SetAsLastSibling();

        if (_station != null)
            _station.ClearStationCell(this);
    }

    public override void Drag(Vector2 pointerPos)
    {
        _rect.position = _mainCamera.WorldToScreenPoint(pointerPos);
    }

    public override void Eat()
    {
        base.Eat();
        Destroy(gameObject);
    }

    public override void ReturnToStartPosition()
    {
        transform.position = _stationParent.transform.position;
        _station.SetFood(this);
    }

    public void UsedForStation(Station station)
    {
        _station = station;
        _stationParent = transform.parent;
    }

    public override Vector3 GetDescriptionPosition()
    {
        RectTransform rect = (RectTransform)transform;

        return rect.position;
    }
}
