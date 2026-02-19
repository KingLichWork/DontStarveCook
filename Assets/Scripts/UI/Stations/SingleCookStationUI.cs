using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class SingleCookStationUI : MonoBehaviour
{
    [SerializeField] private Image _timerImage;
    [SerializeField] private Transform _viewParent;

    [SerializeField] private GameObject _holder;

    public Transform ViewParent => _viewParent;
    private SingleCookingStation _cookingStation;

    [Inject]
    public void Construct(SingleCookingStation cookingStation)
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
        _timerImage.color = new Color(0,1,0,0.3f);
        ClearCook();
    }

    private void ChangeCookTimer(float value)
    {
        _timerImage.fillAmount = value;
    }

    private void ChangeCookStage()
    {
        _timerImage.color = new Color(1,0,0,0.3f);
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
