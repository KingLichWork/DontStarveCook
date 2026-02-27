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
        HungerTimer.ChangeTimerAction += _hungerTimer.ChangeTimer;
        Health.HealthChangeAction += _healthTimer.ChangeTimer;

        _extractButton.onClick.AddListener(Extract);
    }

    private void OnDisable()
    {
        HungerTimer.ChangeTimerAction -= _hungerTimer.ChangeTimer;
        Health.HealthChangeAction -= _healthTimer.ChangeTimer;

        _extractButton.onClick.RemoveAllListeners();
    }

    private void Extract()
    {
        ExtractAction.Invoke();
    }
}
