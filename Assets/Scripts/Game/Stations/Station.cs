using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

public abstract class Station : MonoBehaviour
{
    [SerializeField] protected Food _ash;

    protected bool _isBusy;

    protected CancellationTokenSource _cts;

    protected virtual float _cookingTime => 5f;
    protected virtual float _overCookingTime => 10f;
    public bool IsBusy => _isBusy;


    public Action OnCookStartAction;
    public Action<float> CookInProgressAction;
    public Action OnCookCompleteAction;

    public virtual void StartCook()
    {
        _cts = new CancellationTokenSource();
        Cook(_cts.Token).Forget();
    }

    public virtual void StopCooking() { }

    public abstract void SetFood(FoodViewUI foodView);

    public abstract void ClearStationCell(FoodViewUI foodView);

    protected abstract UniTask Cook(CancellationToken token);
}
