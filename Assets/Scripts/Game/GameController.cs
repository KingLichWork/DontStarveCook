using Kimicu.YandexGames;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VContainer;

public class GameController : MonoBehaviour
{
    private GameSpawner _spawner;
    private FoodViewFactory _foodViewFactory;
    private SingleCookStationUI _singleCookStationUI;
    private MultiCookStationUI _multiCookStationUI;
    private TutorialUI _tutorialUI;
    private GraphicRaycaster _graphicRaycaster;
    private Camera _camera;

    private HungerTimer _hungerTimer;
    private Health _health;

    private ScoreManager _scoreManager;
    private GoldGetterUI _goldGetter;

    private GameTime _gameTime;

    private DayCycleData _dayCycleData;
    private UpgradesData _upgradesData;

    public static event Action EndGameAction;

    [Inject]
    public void Construct(GameSpawner spawner, FoodViewFactory foodViewFactory, SingleCookStationUI singleCookStationUI, MultiCookStationUI multiCookStationUI,
        DayCycleData dayCycleData, GraphicRaycaster graphicRaycaster, ScoreManager scoreManager, GoldGetterUI goldGetter, UpgradesData upgradesData,
        TutorialUI tutorialUI)
    {
        _spawner = spawner;
        _foodViewFactory = foodViewFactory;
        _singleCookStationUI = singleCookStationUI;
        _multiCookStationUI = multiCookStationUI;
        _dayCycleData = dayCycleData;
        _graphicRaycaster = graphicRaycaster;
        _scoreManager = scoreManager;
        _goldGetter = goldGetter;
        _upgradesData = upgradesData;
        _tutorialUI = tutorialUI;
        _camera = Camera.main;

        _gameTime = new GameTime(_dayCycleData);
    }

    private void OnEnable()
    {
        FoodView.EatFoodAction += EatFood;
        InputController.DropAction += HandleDrop;
        GameTime.ChangeDayAction += Save;
        ShopUpgrade.BuyUpgradeAction += Upgrade;
        _hungerTimer.StarvingAction += StarvingDamage;
        _tutorialUI.CompleteTutorialAction += Game;

        Health.GameOverAction += EndGame;
        DebugUI.DebugEndGameAction += EndGame;

        EndGameUI.RestartAction += Restart;
        EndGameUI.ContinueAction += Continue;
    }

    private void OnDisable()
    {
        FoodView.EatFoodAction -= EatFood;
        InputController.DropAction -= HandleDrop;
        GameTime.ChangeDayAction -= Save;
        ShopUpgrade.BuyUpgradeAction -= Upgrade;
        _hungerTimer.StarvingAction -= StarvingDamage;
        _tutorialUI.CompleteTutorialAction -= Game;

        Health.GameOverAction -= EndGame;
        DebugUI.DebugEndGameAction -= EndGame;

        EndGameUI.RestartAction -= Restart;
        EndGameUI.ContinueAction -= Continue;
    }

    public void Init()
    {
        Load();

        _spawner.Init();
        _scoreManager.Init();

        if (SaveManager.PlayerData.Tutorial)
            Game();
        else
            _tutorialUI.Show();
    }

    private void Upgrade(UpgradeType type)
    {
        int value = _upgradesData.GetUpgradeValue(type);

        switch (type)
        {
            case UpgradeType.ExtractCount:
                _spawner.ChangeExtractValue(value);
                break;
            case UpgradeType.AutoExtract:
                _spawner.StartAutoExtract(value);
                break;
            case UpgradeType.MaxHunger:
                _hungerTimer.ChangeMaxValue(value);
                break;
            case UpgradeType.MaxHealth:
                _health.ChangeMaxValue(value);
                break;
        }
    }

    private void Restart()
    {
        SaveManager.PlayerData.ResetSaves();

        SceneManager.LoadScene("Game");
    }

    private void Continue()
    {
        _health.ChangeHealth(_health.MaxHealthValue);
        _hungerTimer.ChangeTimer(_hungerTimer.MaxValueTimer);

        Game();
    }

    private void Save(int day)
    {
        SetScore();

        SaveManager.PlayerData.Hunger = _hungerTimer.ValueTimer;
        SaveManager.PlayerData.Health = _health.HealthValue;
        SaveManager.PlayerData.Day = day;
        SaveManager.PlayerData.MaxExtractValue = _spawner.MaxExtractValue;
    }

    private void SetScore()
    {
        SaveManager.PlayerData.Score = _scoreManager.Score;

        if (SaveManager.PlayerData.MaxScore < SaveManager.PlayerData.Score)
            SaveManager.PlayerData.MaxScore = SaveManager.PlayerData.Score;

        Leaderboard.SetScore(SaveManager.PlayerData.MaxScore);
    }

    private void Load()
    {
        _hungerTimer = new HungerTimer(SaveManager.PlayerData.Hunger, SaveManager.PlayerData.MaxHunger + _upgradesData.GetUpgradeValue(UpgradeType.MaxHunger));
        _health = new Health(SaveManager.PlayerData.Health, SaveManager.PlayerData.MaxHealth + _upgradesData.GetUpgradeValue(UpgradeType.MaxHealth));
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

                case "GoldGetter":
                    DropOnGoldGetter(view);
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
        else
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

    private void DropOnGoldGetter(FoodView view)
    {
        _goldGetter.GetGold(view);
        Destroy(view.gameObject);
    }

    private void Game()
    {
        _gameTime.StartTime();
        _hungerTimer.StartTimer();
        _spawner.StartAutoExtract(SaveManager.PlayerData.Upgrades[(int)UpgradeType.AutoExtract]);
        _spawner.SpawnStartFood(5);
    }

    private void EndGame()
    {
        _gameTime.StopTime();
        _hungerTimer.StopTimer();

        SetScore();

        EndGameAction.Invoke();
    }
}
