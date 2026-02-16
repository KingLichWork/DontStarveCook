using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

public class GameTimer
{
    private int _maxTimerValue;
    private float _currentValue;

    private float _timerSpeed = 1f;
    private float _difficultyTimer = 0f;

    private CancellationTokenSource _cts;

    public int ValueTimer => Mathf.CeilToInt(_currentValue);

    public static event Action<int, int> ChangeTimerAction;
    public static event Action GameOverAction;

    public GameTimer(int maxValue = 100)
    {
        _maxTimerValue = maxValue;
    }

    public void StartTimer()
    {
        StopTimer();

        ResetTimer();
        ChangeTimerAction?.Invoke(ValueTimer, _maxTimerValue);

        _cts = new CancellationTokenSource();
        RunTimer(_cts.Token).Forget();
    }

    public void StopTimer()
    {
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

            _currentValue -= delta * _timerSpeed;

            _difficultyTimer += delta;
            if (_difficultyTimer >= 10f)
            {
                _difficultyTimer = 0f;
                _timerSpeed += 0.1f;
            }

            ChangeTimerAction?.Invoke(ValueTimer, _maxTimerValue);

            if (_currentValue <= 0)
            {
                GameOverAction?.Invoke();
                StopTimer();
                return;
            }
        }
    }

    private void ResetTimer()
    {
        _currentValue = _maxTimerValue;
        _timerSpeed = 1f;
        _difficultyTimer = 0f;
    }
}
