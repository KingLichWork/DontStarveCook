using UnityEngine;
using VContainer;

public class GameController : MonoBehaviour
{
    private GameSpawner _spawner;
    private FoodViewFactory _foodViewFactory;
    private SingleCookStationUI _singleCookStationUI;
    private MultiCookStationUI _multiCookStationUI;

    private HungerTimer _hungerTimer = new HungerTimer(10);
    private Health _health = new Health(100);

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
        _hungerTimer.StarvingAction += StarvingDamage;
    }

    private void OnDisable()
    {
        FoodView.EatFoodAction -= EatFood;
        InputController.DropAction -= HandleDrop;
        _hungerTimer.StarvingAction -= StarvingDamage;
    }

    private void Start()
    {
        Game();
    }

    private void StarvingDamage()
    {
        _health.ChangeHealth(1);
    }

    private void EatFood(FoodView foodView)
    {
        _hungerTimer.ChangeTimer(foodView.Food.FoodValue);
    }

    private void HandleDrop(FoodView view, Vector2 worldPos)
    {
        if (view == null)
            return;

        RaycastHit2D[] hits = Physics2D.RaycastAll(worldPos, Vector2.zero);

        bool isBlock = false;

        foreach (var hit in hits)
        {
            switch (hit.collider.tag)
            {
                case "Block":
                    isBlock = true;
                    break;

                case "SingleCook":
                case "MultiCook":
                    TryDropOnStation(view, hit.collider);
                    return;
            }
        }

        if(isBlock)
            view.ReturnToStartPosition();

        _foodViewFactory.CreateGameView(view, worldPos);
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

    private void Game()
    {
        _hungerTimer.StartTimer();
        _spawner.SpawnStartFood(5);
        //_spawner.StartSpawn(2f);
    }
}
