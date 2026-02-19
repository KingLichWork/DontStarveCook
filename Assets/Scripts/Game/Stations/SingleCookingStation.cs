using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

public class SingleCookingStation : Station
{
    [SerializeField] private Food _ash;

    private float _cookingTime = 5f;
    private float _overCookingTime = 10f;

    private bool _isBusy;

    private CancellationTokenSource _cts;

    private FoodViewUI _foodView;

    public bool IsBusy => _isBusy;

    public event Action OnCookStartAction;
    public event Action<float> CookInProgressAction;
    public event Action OnCookCompleteAction;

    public override void SetFood(FoodViewUI foodView)
    {
        _foodView = foodView;
        _foodView.UsedForStation(this);

        StartCook();
    }

    public override void StartCook()
    {
        _cts = new CancellationTokenSource();
        Cook(_cts.Token).Forget();
    }

    public override void ClearStation()
    {
        _foodView = null;
        _isBusy = false;

        _cts?.Cancel();
        _cts?.Dispose();
        _cts = null;
    }

    private async UniTask Cook(CancellationToken token)
    {
        _isBusy = true;
        OnCookStartAction?.Invoke();

        async UniTask CookPhase(float duration, Food resultFood)
        {
            float elapsed = 0f;

            while (elapsed < duration)
            {
                await UniTask.Yield(PlayerLoopTiming.Update, token);
                elapsed += Time.deltaTime;
                CookInProgressAction?.Invoke(Mathf.Clamp01(elapsed / duration));
            }

            _foodView.SetFood(resultFood);
            OnCookCompleteAction?.Invoke();
        }

        await CookPhase(_cookingTime, _foodView.Food.HasCookedVersion ? _foodView.Food.CookedVersion : _ash);

        await CookPhase(_overCookingTime, _ash);
    }
}
