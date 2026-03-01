using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

public class GameTime
{
    private float _dayTime;
    private float _currentDayTime;
    private int _dayCount;
    private DayType _dayType;

    private CancellationTokenSource _cts;

    public float CurrentDayTime => _currentDayTime;
    public DayType DayType => _dayType;

    public static event Action<int> ChangeDayAction;
    public static event Action<float, float> ChangeTimeAction;

    public GameTime(float dayTime)
    {
        _dayTime = dayTime;
    }

    public void StartTime()
    {
        StopTimer();
        _cts = new CancellationTokenSource();
        Time(_cts.Token).Forget();
    }

    public void StopTimer()
    {
        _currentDayTime = 0;
        _dayCount = 0;

        _cts?.Cancel();
        _cts?.Dispose();
        _cts = null;
    }

    private async UniTask Time(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            await UniTask.WaitForSeconds(1f);
            _currentDayTime++;

            if(_currentDayTime >= _dayTime)
            {
                _currentDayTime = 0;
                _dayCount++;
                ChangeDayAction.Invoke(_dayCount);
            }

            ChangeTimeAction.Invoke(_currentDayTime, _dayTime);
        }
    }
}

public enum DayType
{
    Day,
    Evening,
    Night
}
