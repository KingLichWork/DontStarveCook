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
        StartPosition();
    }

    private void OnDisable()
    {
        _cookingStation.OnCookStartAction -= StartCook;
        _cookingStation.CookInProgressAction -= ChangeCookTimer;
        _cookingStation.OnCookCompleteAction -= ChangeCookStage;
    }

    private void StartPosition()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(_cookingStation.transform.position);
        GetComponent<RectTransform>().position = screenPos;
    }
}
