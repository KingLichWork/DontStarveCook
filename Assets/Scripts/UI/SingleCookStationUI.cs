using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class SingleCookStationUI : MonoBehaviour
{
    [SerializeField] private Image _timerImage;

    private CookingStation _cookingStation;

    [Inject]
    public void Construct(CookingStation cookingStation)
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

    private void StartCook()
    {
        _timerImage.color = Color.green;
        ClearCook();
    }

    private void ChangeCookTimer(float value)
    {
        _timerImage.fillAmount = value;
    }

    private void ChangeCookStage()
    {
        _timerImage.color = Color.red;
        ClearCook();
    }

    private void ClearCook()
    {
        _timerImage.fillAmount = 0;
    }

    private void StartPosition()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(_cookingStation.transform.position);
        GetComponent<RectTransform>().position = screenPos;
    }
}
