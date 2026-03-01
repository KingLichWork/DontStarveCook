using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    [SerializeField] private SingleCookStation _cookingStation;
    [SerializeField] private SingleCookStationUI _singleCookStationUI;
    [SerializeField] private MultiCookStation _multiCookingStation;
    [SerializeField] private MultiCookStationUI _multiCookStationUI;

    [SerializeField] private GameController _gameController;
    [SerializeField] private GameSpawner _spawner;
    [SerializeField] private InputController _inputController;
    [SerializeField] private FoodViewDescription _foodViewDescription;
    [SerializeField] private GameUI _gameUI;

    [SerializeField] private FoodViewGame _gamePrefab;
    [SerializeField] private FoodViewUI _uiPrefab;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterInstance(_gamePrefab);
        builder.RegisterInstance(_uiPrefab);

        builder.RegisterComponent(_singleCookStationUI);
        builder.RegisterComponent(_multiCookStationUI);
        builder.RegisterComponent(_cookingStation);
        builder.RegisterComponent(_multiCookingStation);
        builder.RegisterComponent(_gameController);
        builder.RegisterComponent(_spawner);
        builder.RegisterComponent(_inputController);
        builder.RegisterComponent(_foodViewDescription);
        builder.RegisterComponent(_gameUI);

        builder.Register<FoodViewFactory>(Lifetime.Singleton);
        builder.Register<LocalizationService>(Lifetime.Singleton);
    }
}
