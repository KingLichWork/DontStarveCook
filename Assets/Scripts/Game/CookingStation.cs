using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public class CookingStation : MonoBehaviour
{
    [SerializeField] private Food _ash;

    private float _cookingTime = 5f;
    private float _overCookingTime = 10f;

    private bool _isBusy;

    public event Action OnCookStartAction;
    public event Action<float> CookInProgressAction;
    public event Action OnCookCompleteAction;

    public bool TryCook(FoodView foodView)
    {
        if (_isBusy)
            return false;

        Cook(foodView).Forget();
        return true;
    }

    public void ClearStation()
    {
        _isBusy = false;
    }

    private async UniTaskVoid Cook(FoodView foodView)
    {
        _isBusy = true;
        OnCookStartAction?.Invoke();

        float elapsed = 0f;

        while (elapsed < _cookingTime)
        {
            await UniTask.Yield(PlayerLoopTiming.Update);

            elapsed += Time.deltaTime;

            float progress = Mathf.Clamp01(elapsed / _cookingTime);
            CookInProgressAction?.Invoke(progress);
        }

        foodView.SetFood(
            foodView.Food.HasCookedVersion
            ? foodView.Food.CookedVersion
            : _ash
        );

        OnCookCompleteAction?.Invoke();

        elapsed = 0f;
        while (elapsed < _overCookingTime)
        {
            await UniTask.Yield(PlayerLoopTiming.Update);

            elapsed += Time.deltaTime;

            float progress = Mathf.Clamp01(elapsed / _overCookingTime);
            CookInProgressAction?.Invoke(progress);
        }

        foodView.SetFood(_ash);
    }
}
