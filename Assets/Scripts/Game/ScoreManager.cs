using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private int _score;

    public int Score => _score;

    public static event Action<int> ChangeScoreAction;

    private void OnEnable()
    {
        GameTime.ChangeTimeScoreAction += TimeScore;
        GameTime.ChangeDayScoreAction += DayScore;
    }

    private void OnDisable()
    {
        GameTime.ChangeTimeScoreAction -= TimeScore;
        GameTime.ChangeDayScoreAction -= DayScore;
    }

    public void Init()
    {
        _score = SaveManager.PlayerData.Score;
        ChangeScoreAction?.Invoke(_score);
    }

    public void GetScore(int value)
    {
        _score += value;
        ChangeScoreAction?.Invoke(_score);
    }

    private void TimeScore()
    {
        GetScore(1);
    }

    private void DayScore()
    {
        GetScore(10);
    }
}
