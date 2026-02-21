using UnityEngine;
using VContainer;

public class MultiCookStationUI : CookStationUI
{
    [SerializeField] private Transform _resultParent;

    private MultiCookStation _multiCookingStation;

    [Inject]
    public void Construct(MultiCookStation multiCookingStation)
    {
        _multiCookingStation = multiCookingStation;
    }

    private void OnEnable()
    {
        _multiCookingStation.OnCookStartAction += StartCook;
        _multiCookingStation.CookInProgressAction += ChangeCookTimer;
        _multiCookingStation.OnCookCompleteAction += ChangeCookStage;
        _multiCookingStation.CreateFoodViewAction += ViewSetParent;

        StartPosition();
    }

    private void OnDisable()
    {
        _multiCookingStation.OnCookStartAction -= StartCook;
        _multiCookingStation.CookInProgressAction -= ChangeCookTimer;
        _multiCookingStation.OnCookCompleteAction -= ChangeCookStage;
        _multiCookingStation.CreateFoodViewAction -= ViewSetParent;
    }

    private void ViewSetParent(FoodViewUI foodViewUI)
    {
        foodViewUI.transform.parent = _resultParent;
        foodViewUI.transform.position = _resultParent.position;
    }

    private void StartPosition()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(_multiCookingStation.transform.position);
        GetComponent<RectTransform>().position = screenPos;
    }
}
