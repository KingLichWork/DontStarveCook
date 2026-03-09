using FindTheDifference.Audio;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    [SerializeField] private GraphicRaycaster _graphicRaycaster;
    [SerializeField] private SingleCookStationUI _singleCookStationUI;
    [SerializeField] private MultiCookStationUI _multiCookStationUI;
    [SerializeField] private GameUI _gameUI;
    [SerializeField] private FoodViewDescription _foodViewDescription;
    [SerializeField] private ShopUI _shopUI;
    [SerializeField] private TutorialUI _tutorialUI;
    [SerializeField] private FoodViewUI _uiPrefab;

    [Space]

    [SerializeField] private SingleCookStation _cookingStation;
    [SerializeField] private MultiCookStation _multiCookingStation;
    [SerializeField] private GoldGetterUI _goldGetter;
    [SerializeField] private GameController _gameController;
    [SerializeField] private GameSpawner _spawner;
    [SerializeField] private InputController _inputController;
    [SerializeField] private AudioManager _audioManager;
    [SerializeField] private ScoreManager _scoreManager;
    [SerializeField] private FoodViewGame _gamePrefab;

    [Space]

    [SerializeField] private DayCycleData _dayCycleData;
    [SerializeField] private UpgradesData _ugradesData;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterComponent(_singleCookStationUI);
        builder.RegisterComponent(_multiCookStationUI);
        builder.RegisterComponent(_gameUI);
        builder.RegisterComponent(_shopUI);
        builder.RegisterComponent(_tutorialUI);
        builder.RegisterInstance(_gamePrefab);
        builder.RegisterInstance(_uiPrefab);

        builder.RegisterComponent(_graphicRaycaster);
        builder.RegisterComponent(_cookingStation);
        builder.RegisterComponent(_multiCookingStation);
        builder.RegisterComponent(_gameController);
        builder.RegisterComponent(_spawner);
        builder.RegisterComponent(_inputController);
        builder.RegisterComponent(_foodViewDescription);
        builder.RegisterComponent(_audioManager);
        builder.RegisterComponent(_scoreManager);
        builder.RegisterComponent(_goldGetter);

        builder.RegisterInstance(_dayCycleData);
        builder.RegisterComponent(_ugradesData);

        builder.Register<FoodViewFactory>(Lifetime.Singleton);
        builder.RegisterEntryPoint<GameBootstrap>();
    }
}
