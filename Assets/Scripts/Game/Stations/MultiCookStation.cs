using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using VContainer;

public class MultiCookStation : Station
{
    [SerializeField] private Recipe _gruel;
    [SerializeField] private RecipesData _recipes;

    private CookMergeController _cookController;
    private FoodViewFactory _foodViewFactory;

    private FoodViewUI[] _usedFoods = new FoodViewUI[4];

    protected override float _cookingTime => 10f;
    protected override float _overCookingTime => 10f;

    public event Action<FoodViewUI> CreateFoodViewAction;

    [Inject]
    public void Construct(FoodViewFactory foodViewFactory)
    {
        _foodViewFactory = foodViewFactory;
        _cookController = new CookMergeController(_recipes, _gruel);
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

    public override void StopCooking()
    {
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

                if (i == _usedFoods.Length - 1)
                    StartCook();

                return;
            }
        }
    }

    public void ClearCell(int number)
    {
        _usedFoods[number] = null;
    }

    protected async override UniTask Cook(CancellationToken token)
    {
        _isBusy = true;
        OnCookStartAction?.Invoke();

        Food[] ingredients = new Food[4];

        for (int i = 0; i < 4; i++)
        {
            ingredients[i] = (Food)_usedFoods[i].Food;
        }

        await CookPhase(_cookingTime, _cookController.Cook(ingredients), token);
        DestroyIngredients();

        await CookPhase(_overCookingTime, _ash, token);
    }

    private async UniTask CookPhase(float duration, FoodBase resultFood, CancellationToken token)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            token.ThrowIfCancellationRequested();

            await UniTask.Yield(PlayerLoopTiming.Update, token);
            elapsed += Time.deltaTime;
            CookInProgressAction?.Invoke(Mathf.Clamp01(elapsed / duration));
        }

        DestroyIngredients();
        FoodViewUI resultFoodView = _foodViewFactory.CreateUIView(resultFood, null, true);
        resultFoodView.SetResult();
        CreateFoodViewAction?.Invoke(resultFoodView);
        OnCookCompleteAction?.Invoke();
    }

    private void DestroyIngredients()
    {
        int lenght = _usedFoods.Length;

        for (int i = 0; i < lenght; i++)
        {
            if (_usedFoods[i] != null)
                Destroy(_usedFoods[i].gameObject);
        }
    }
}
