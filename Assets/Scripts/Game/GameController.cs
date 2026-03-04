using Kimicu.YandexGames;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VContainer;

public class GameController : MonoBehaviour
{
    private GameSpawner _spawner;
    private FoodViewFactory _foodViewFactory;
    private SingleCookStationUI _singleCookStationUI;
    private MultiCookStationUI _multiCookStationUI;
    private GraphicRaycaster _graphicRaycaster;
    private Camera _camera;

    private HungerTimer _hungerTimer;
    private Health _health;

    private ScoreManager _scoreManager;

    private GameTime _gameTime;

    private DayCycleData _dayCycleData;

    [Inject]
    public void Construct(GameSpawner spawner, FoodViewFactory foodViewFactory, SingleCookStationUI singleCookStationUI, MultiCookStationUI multiCookStationUI,
        DayCycleData dayCycleData, GraphicRaycaster graphicRaycaster, ScoreManager scoreManager)
    {
        _spawner = spawner;
        _foodViewFactory = foodViewFactory;
        _singleCookStationUI = singleCookStationUI;
        _multiCookStationUI = multiCookStationUI;
        _dayCycleData = dayCycleData;
        _graphicRaycaster = graphicRaycaster;
        _scoreManager = scoreManager;
        _camera = Camera.main;

        _gameTime = new GameTime(_dayCycleData);
    }

    private void OnEnable()
    {
        Init();

        FoodView.EatFoodAction += EatFood;
        InputController.DropAction += HandleDrop;
        GameTime.ChangeDayAction += Save;
        _hungerTimer.StarvingAction += StarvingDamage;
    }

    private void OnDisable()
    {
        FoodView.EatFoodAction -= EatFood;
        InputController.DropAction -= HandleDrop;
        GameTime.ChangeDayAction -= Save;
        _hungerTimer.StarvingAction -= StarvingDamage;
    }

    private void Start()
    {
        Game();
    }

    private void Init()
    {
        _spawner.Init();
        _hungerTimer = new HungerTimer(SaveManager.PlayerData.Hunger, SaveManager.PlayerData.MaxHunger);
        _health = new Health(SaveManager.PlayerData.Health, SaveManager.PlayerData.MaxHealth);
        _scoreManager.Init();
    }

    private void Save(int day)
    {
        SaveManager.PlayerData.Score = _scoreManager.Score;
        SaveManager.PlayerData.Hunger = _hungerTimer.ValueTimer;
        SaveManager.PlayerData.Health = _health.HealthValue;
        SaveManager.PlayerData.Day = day;
        SaveManager.PlayerData.ExtractValue = _spawner.ExtractValue;

        if(SaveManager.PlayerData.MaxScore < SaveManager.PlayerData.Score)
            SaveManager.PlayerData.MaxScore = SaveManager.PlayerData.Score;

        Leaderboard.SetScore(SaveManager.PlayerData.MaxScore);
    }

    private void StarvingDamage()
    {
        _health.ChangeHealth(-1);
    }

    private void NightDamage()
    {
        if (_gameTime.IsNight)
        {
            _health.ChangeHealth(-30);
        }
    }

    private void EatFood(FoodView foodView)
    {
        _hungerTimer.ChangeTimer(foodView.Food.FoodValue);
        _health.ChangeHealth(foodView.Food.HealthValue);
        _scoreManager.GetScore(foodView.Food.FoodValue);
    }

    private void HandleDrop(FoodView view, Vector2 worldPos)
    {
        if (view == null)
            return;

        PointerEventData eventData = new PointerEventData(EventSystem.current)
        {
            position = _camera.WorldToScreenPoint(worldPos)
        };

        var results = new List<RaycastResult>();
        _graphicRaycaster.Raycast(eventData, results);

        foreach (var result in results)
        {
            switch (result.gameObject.tag)
            {
                case "SingleCook":
                case "MultiCook":
                    TryDropOnStation(view, result.gameObject);
                    return;
            }
        }

        RaycastHit2D[] hits = Physics2D.RaycastAll(worldPos, Vector2.zero);

        bool isBlock = false;

        foreach (var hit in hits)
        {
            if(hit.collider.tag == "Block")
            {
                isBlock = true;
                break;
            }
        }

        if(isBlock)
            view.ReturnToStartPosition();

        _foodViewFactory.CreateGameView(view, worldPos);
        Destroy(view.gameObject);
    }

    private void TryDropOnStation(FoodView view, GameObject gameObject)
    {
        Station station = gameObject.GetComponent<Station>();

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
        _gameTime.StartTime();
        _hungerTimer.StartTimer();
        _spawner.SpawnStartFood(5);
        //_spawner.StartSpawn(2f);
    }
}
