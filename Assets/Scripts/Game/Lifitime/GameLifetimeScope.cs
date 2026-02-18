using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    [SerializeField] private CookingStation _cookingStation;
    [SerializeField] private SingleCookStationUI _singleCookStationUI;
    [SerializeField] private GameController _gameController;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterComponent(_singleCookStationUI);
        builder.RegisterComponent(_cookingStation);
        builder.RegisterComponent(_gameController);
    }
}
