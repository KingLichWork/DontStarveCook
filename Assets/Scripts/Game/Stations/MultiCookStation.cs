using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class MultiCookStation : Station
{
    [SerializeField] private Food _gruel;

    private CookMergeController _cookController = new CookMergeController();
    private FoodViewFactory _foodViewFactory;

    private FoodViewUI[] _usedFoods = new FoodViewUI[4];

    protected override float _cookingTime => 10f;
    protected override float _overCookingTime => 10f;

    public event Action<FoodViewUI> CreateFoodViewAction;

    public void Construct(FoodViewFactory foodViewFactory)
    {
        _foodViewFactory = foodViewFactory;
    }

    public override void ClearStationCell(FoodViewUI foodView)
    {
        for (int i = 0; i < _usedFoods.Length; i++)
        {
            if (_usedFoods[i] == foodView)
            {
                ClearCell(i);
                return;
            }
        }

        _isBusy = false;

        _cts?.Cancel();
        _cts?.Dispose();
        _cts = null;
    }

    public override void SetFood(FoodViewUI foodView)
    {
        for (int i = 0; i < _usedFoods.Length; i++)
        {
            if (_usedFoods[i] == null)
            {
                _usedFoods[i] = foodView;
                _usedFoods[i].UsedForStation(this);
                return;
            }
        }
    }

    protected async override UniTask Cook(CancellationToken token)
    {
        _isBusy = true;
        OnCookStartAction?.Invoke();

        Food[] ingredients = new Food[4];

        for (int i = 0; i < 4; i++)
        {
            ingredients[i] = _usedFoods[i].Food;
        }

        await CookPhase(_cookingTime, _cookController.Cook(ingredients), token);

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
            CookInProgressAction?.Invoke(Mathf.Clamp01(elapsed / duration));
        }

        FoodViewUI resultFoodView = _foodViewFactory.CreateUIView(resultFood, null);
        CreateFoodViewAction?.Invoke(resultFoodView);
        OnCookCompleteAction?.Invoke();
    }

    public void ClearCell(int number)
    {
        _usedFoods[number] = null;
    }
}
