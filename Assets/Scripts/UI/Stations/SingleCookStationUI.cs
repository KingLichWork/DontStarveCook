using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class SingleCookStationUI : CookStationUI
{
    private SingleCookStation _cookingStation;

    [Inject]
    public void Construct(SingleCookStation cookingStation)
    {
        _cookingStation = cookingStation;
    }

    private void OnEnable()
    {
        _cookingStation.OnCookStartAction += StartCook;
        _cookingStation.CookInProgressAction += ChangeCookTimer;
        _cookingStation.OnCookCompleteAction += ChangeCookStage;

        ClearCook();
    }

    private void OnDisable()
    {
        _cookingStation.OnCookStartAction -= StartCook;
        _cookingStation.CookInProgressAction -= ChangeCookTimer;
        _cookingStation.OnCookCompleteAction -= ChangeCookStage;
    }
}
