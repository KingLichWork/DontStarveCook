using UnityEngine;
using VContainer;

public class GameController : MonoBehaviour
{
    private GameSpawner _spawner;
    private FoodViewFactory _foodViewFactory;
    private SingleCookStationUI _singleCookStationUI;
    private GameTimer _gameTimer = new GameTimer(100);

    [Inject]
    public void Construct(GameSpawner spawner, FoodViewFactory foodViewFactory, SingleCookStationUI singleCookStationUI)
    {
        _spawner = spawner;
        _foodViewFactory = foodViewFactory;
        _singleCookStationUI = singleCookStationUI;
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

        view.EndDrag(worldPos);
    }

    private void TryDropOnStation(FoodView view, Collider2D collider)
    {
        CookingStation station = collider.GetComponent<CookingStation>();

        if (station == null || !station.TryCook(view))
        {
            view.ReturnToStartPosition();
            return;
        }

        var food = view.Food;

        Destroy(view.gameObject);

        _foodViewFactory.CreateUIView(food, _singleCookStationUI.ViewParent);
    }

    public void Game()
    {
        _gameTimer.StartTimer();
        //_spawner.StartSpawn(1f);
    }
}
