using UnityEngine;
using VContainer;

public class GameController : MonoBehaviour
{
    private GameSpawner _spawner;
    private FoodViewFactory _foodViewFactory;
    private SingleCookStationUI _singleCookStationUI;
    private MultiCookStationUI _multiCookStationUI;

    private GameTimer _gameTimer = new GameTimer(100);

    [Inject]
    public void Construct(GameSpawner spawner, FoodViewFactory foodViewFactory, SingleCookStationUI singleCookStationUI, MultiCookStationUI multiCookStationUI)
    {
        _spawner = spawner;
        _foodViewFactory = foodViewFactory;
        _singleCookStationUI = singleCookStationUI;
        _multiCookStationUI = multiCookStationUI;
    }

    private void OnEnable()
    {
        FoodView.EatFoodAction += EatFood;
        InputController.DropAction += HandleDrop;
    }

    private void OnDisable()
    {
        FoodView.EatFoodAction -= EatFood;
        InputController.DropAction -= HandleDrop;
    }

    private void Start()
    {
        Game();

        for (int i = 0; i < 10; i++)
            _spawner.SpawnFood();
    }

    private void EatFood(FoodView foodView)
    {
        _gameTimer.ChangeTimer(foodView.Food.FoodValue);
    }

    private void HandleDrop(FoodView view, Vector2 worldPos)
    {
        if (view == null)
            return;

        RaycastHit2D[] hits = Physics2D.RaycastAll(worldPos, Vector2.zero);

        foreach (var hit in hits)
        {
            switch (hit.collider.tag)
            {
                case "Block":
                    view.ReturnToStartPosition();
                    return;

                case "SingleCook":
                case "MultiCook":
                    TryDropOnStation(view, hit.collider);
                    return;
            }
        }

        _foodViewFactory.CreateGameView(view.Food, worldPos);
        Destroy(view.gameObject);
    }

    private void TryDropOnStation(FoodView view, Collider2D collider)
    {
        Station station = collider.GetComponent<Station>();

        if (station == null || station.IsBusy)
        {
            view.ReturnToStartPosition();
            return;
        }

        Food food = (Food)view.Food;
        Destroy(view.gameObject);

        FoodViewUI newView = _foodViewFactory.CreateUIView(food, (station is SingleCookStation) ? _singleCookStationUI.ViewParent 
            : _multiCookStationUI.ViewParent);

        station.SetFood(newView);
    }

    public void Game()
    {
        _gameTimer.StartTimer();
        //_spawner.StartSpawn(1f);
    }
}
