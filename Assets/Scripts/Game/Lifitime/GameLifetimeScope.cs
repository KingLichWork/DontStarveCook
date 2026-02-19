using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    [SerializeField] private SingleCookingStation _cookingStation;
    [SerializeField] private SingleCookStationUI _singleCookStationUI;
    [SerializeField] private GameController _gameController;
    [SerializeField] private GameSpawner _spawner;
    [SerializeField] private InputController _inputController;


    [SerializeField] private FoodViewGame _gamePrefab;
    [SerializeField] private FoodViewUI _uiPrefab;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterInstance(_gamePrefab);
        builder.RegisterInstance(_uiPrefab);

        builder.RegisterComponent(_singleCookStationUI);
        builder.RegisterComponent(_cookingStation);
        builder.RegisterComponent(_gameController);
        builder.RegisterComponent(_spawner);
        builder.RegisterComponent(_inputController);

        builder.Register<FoodViewFactory>(Lifetime.Singleton);
    }
}
