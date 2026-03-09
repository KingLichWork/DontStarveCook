using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

public class GameTime
{
    private float _maxDayTime;
    private float _currentDayTime;
    private int _dayCount = 1;

    private DayPhase _dayPhase;
    private DayCycleData _data;

    private CancellationTokenSource _cts;

    public float CurrentDayTime => _currentDayTime;
    public DayPhase DayPhase => _dayPhase;

    public bool IsNight => DayPhase == DayPhase.Night;

    public static event Action ChangeTimeScoreAction;
    public static event Action ChangeDayScoreAction;

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
        ResetTime();
        StopTime();
        _cts = new CancellationTokenSource();
        Time(_cts.Token).Forget();
    }

    public void StopTime()
    {
        _cts?.Cancel();
        _cts?.Dispose();
        _cts = null;
    }

    private void ResetTime()
    {
        _currentDayTime = 0;
        _dayCount = 1;
    }

    private async UniTask Time(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            await UniTask.WaitForSeconds(1f);
            _currentDayTime++;

            if(_currentDayTime >= _maxDayTime)
                ChangeDay();

            ChangeDayPhase(_currentDayTime);
            ChangeTimeScoreAction.Invoke();
            ChangeTimeAction.Invoke(_currentDayTime, _maxDayTime);
        }
    }

    private void ChangeDay()
    {
        _currentDayTime = 0;
        _dayCount++;

        ChangeDayScoreAction.Invoke();
        ChangeDayAction.Invoke(_dayCount);
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
