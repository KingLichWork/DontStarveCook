using UnityEngine;
using UnityEngine.UI;

public class FoodViewUI : FoodView
{
    [SerializeField] private Image _image;
    [SerializeField] private RectTransform _rect;

    private Station _station;
    private Transform _stationParent;
    private Camera _mainCamera;
    private bool _isRecipe;
    private bool _isResultCooking;

    private FoodViewGame _foodViewGame;

    public bool IsRecipe => _isRecipe;
    public bool IsResultCooking => _isResultCooking;

    public override void SetFood(FoodBase food)
    {
        base.SetFood(food);
        _image.sprite = _food.Sprite;
    }

    public void SetFood(FoodBase food, bool isRecipe)
    {
        _food = food;
        _image.sprite = _food.Sprite;
        _isRecipe = isRecipe;
    }

    public void Init(FoodViewGame viewGame)
    {
        _foodViewGame = viewGame;
    }

    public override void StartDrag(Vector2 pointerPos)
    {
        if(_mainCamera == null)
            _mainCamera = Camera.main;

        transform.SetAsLastSibling();

        if (_station != null)
            _station.ClearStationCell(this);

        if (_isResultCooking)
            _station.StopCooking();
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
        if(_station != null)
        {
            transform.position = _stationParent.transform.position;
            _station.SetFood(this);
        }
        else
        {
            _foodViewGame.Show();
            _foodViewGame = null;

            Destroy(gameObject);
        }
    }

    public void UsedForStation(Station station)
    {
        _station = station;
        _stationParent = transform.parent;
    }

    public void SetResult(Station station)
    {
        _station = station;
        _isResultCooking = true;
    }

    public override Vector3 GetDescriptionPosition()
    {
        RectTransform rect = (RectTransform)transform;

        return rect.position;
    }

    private void OnDestroy()
    {
        if(_foodViewGame != null)
            Destroy(_foodViewGame.gameObject);
    }
}
