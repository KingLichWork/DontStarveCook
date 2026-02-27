using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class GameUI : MonoBehaviour
{
    [SerializeField] private Button _extractButton;

    [SerializeField] private TimerSliderUI _hungerTimer;
    [SerializeField] private TimerSliderUI _healthTimer;

    public static event Action ExtractAction;

    private void OnEnable()
    {
        GameTimer.ChangeTimerAction += _hungerTimer.ChangeTimer;

        _extractButton.onClick.AddListener(Extract);
    }

    private void OnDisable()
    {
        GameTimer.ChangeTimerAction -= _hungerTimer.ChangeTimer;

        _extractButton.onClick.RemoveAllListeners();
    }

    private void Extract()
    {
        ExtractAction.Invoke();
    }
}
