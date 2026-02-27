using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

public class HungerTimer
{
    private int _maxTimerValue;
    private float _currentValue;

    private float _timerSpeed = 1f;
    private float _difficultyTimer = 0f;
    private float _changeDiffucultyTime = 10f;
    private float _starvingTickTime = 1f;
    private float _starvingTimer;

    private CancellationTokenSource _cts;

    public int ValueTimer => Mathf.CeilToInt(_currentValue);

    public event Action StarvingAction;
    public static event Action<int, int> ChangeTimerAction;

    public HungerTimer(int maxValue = 100)
    {
        _maxTimerValue = maxValue;
    }

    public void StartTimer()
    {
        StopTimer();
        ChangeTimerAction?.Invoke(ValueTimer, _maxTimerValue);

        _cts = new CancellationTokenSource();
        RunTimer(_cts.Token).Forget();
    }

    public void StopTimer()
    {
        ResetTimer();

        _cts?.Cancel();
        _cts?.Dispose();
        _cts = null;
    }

    public void ChangeTimer(int value)
    {
        _currentValue = Mathf.Clamp(_currentValue + value, 0, _maxTimerValue);
        ChangeTimerAction?.Invoke(ValueTimer, _maxTimerValue);
    }

    private async UniTaskVoid RunTimer(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            await UniTask.Yield(PlayerLoopTiming.Update);

            float delta = Time.deltaTime;

            _currentValue = Mathf.Max(0, _currentValue - delta * _timerSpeed);

            _difficultyTimer += delta;
            if (_difficultyTimer >= _changeDiffucultyTime)
            {
                _difficultyTimer -= _changeDiffucultyTime;
                _timerSpeed += 0.1f;
            }

            ChangeTimerAction?.Invoke(ValueTimer, _maxTimerValue);

            if (_currentValue <= 0)
            {
                _starvingTimer += delta;

                if (_starvingTimer >= _starvingTickTime)
                {
                    _starvingTimer -= _starvingTickTime;
                    StarvingAction?.Invoke();
                }
            }
            else
                _starvingTimer = 0f;
        }
    }

    private void ResetTimer()
    {
        _currentValue = _maxTimerValue;
        _timerSpeed = 1f;
        _difficultyTimer = 0f;
    }
}
