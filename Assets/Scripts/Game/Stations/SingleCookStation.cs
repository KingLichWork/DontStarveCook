using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

public class SingleCookStation : Station
{
    private FoodViewUI _foodView;

    protected override float _cookingTime => 5f;
    protected override float _overCookingTime => 10f;

    public override void SetFood(FoodViewUI foodView)
    {
        _foodView = foodView;
        _foodView.UsedForStation(this);

        StartCook();
    }

    public override void ClearStationCell(FoodViewUI foodView)
    {
        _foodView = null;
        _isBusy = false;

        _cts?.Cancel();
        _cts?.Dispose();
        _cts = null;
    }

    protected async override UniTask Cook(CancellationToken token)
    {
        _isBusy = true;
        OnCookStartAction?.Invoke();

        await CookPhase(_cookingTime, ((Food)_foodView.Food).HasCookedVersion ? ((Food)_foodView.Food).CookedVersion : _ash, token);

        await CookPhase(_overCookingTime, _ash, token);
    }

    private async UniTask CookPhase(float duration, Food resultFood, CancellationToken token)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            token.ThrowIfCancellationRequested();

            await UniTask.Yield(PlayerLoopTiming.Update, token);
            elapsed += Time.deltaTime;
            CookInProgressAction?.Invoke(elapsed / duration);
        }

        _foodView.SetFood(resultFood);
        OnCookCompleteAction?.Invoke();
    }
}
