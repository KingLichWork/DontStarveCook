using Cysharp.Threading.Tasks;
using System;

public class GameTimer
{
    private int _maxTimerValue;
    private int _valueTimer;

    public int ValueTimer () => _valueTimer;

    public static event Action<int, int> ChangeTimerAction;
    public static event Action GameOverAction;

    public GameTimer(int maxValue = 100)
    {
        _maxTimerValue = maxValue;
    }

    public void StartTimer()
    {
        ResetTimer();
        ChangeTimerAction.Invoke(_valueTimer, _maxTimerValue);
        Timer();
    }

    public void ChangeTimer(int value)
    {
        _valueTimer += value;
        ChangeTimerAction.Invoke(_valueTimer, _maxTimerValue);
    }

    private async UniTask Timer()
    {
        while (true) 
        { 
            await UniTask.Delay(1000);

            _valueTimer--;
            ChangeTimerAction.Invoke(_valueTimer, _maxTimerValue);

            if(_valueTimer <= 0)
            {
                GameOverAction.Invoke();     
                return;
            }
        }
    }

    private void ResetTimer()
    {
        _valueTimer = 100;
    }
}
