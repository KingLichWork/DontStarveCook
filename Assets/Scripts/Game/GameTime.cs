using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

public class GameTime
{
    private float _maxDayTime;
    private float _currentDayTime;
    private int _dayCount;

    private DayPhase _dayPhase;
    private DayCycleData _data;

    private CancellationTokenSource _cts;

    public float CurrentDayTime => _currentDayTime;
    public DayPhase DayPhase => _dayPhase;

    public static event Action<int> ChangeDayAction;
    public static event Action<float, float> ChangeTimeAction;
    public static event Action<DayPhase> ChangeDayPhaseAction;

    public GameTime(DayCycleData data)
    {
        _maxDayTime = data.MaxDayTime;
        _data = data;
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

            if(_currentDayTime >= _maxDayTime)
            {
                _currentDayTime = 0;
                _dayCount++;
                ChangeDayAction.Invoke(_dayCount);
            }

            ChangeDayPhase(_currentDayTime);
            ChangeTimeAction.Invoke(_currentDayTime, _maxDayTime);
        }
    }

    private void ChangeDayPhase(float time)
    {
        var newPhase = GetCurrentPhase(time);

        if (newPhase != _dayPhase)
        {
            _dayPhase = newPhase;
            ChangeDayPhaseAction?.Invoke(newPhase);
        }
    }

    private DayPhase GetCurrentPhase(float time)
    {
        float currentTime = 0f;

        foreach (var phase in _data.Phases)
        {
            float phaseDuration = _data.MaxDayTime * (phase.Percent / 100f);

            if (time >= currentTime && time < currentTime + phaseDuration)
            {
                return phase.Phase;
            }

            currentTime += phaseDuration;
        }

        return DayPhase.Night;
    }
}

public enum DayPhase
{
    Day,
    Evening,
    Night
}
