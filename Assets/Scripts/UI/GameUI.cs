using DG.Tweening;
using System;
using TMPro;
using UnityEditor.Localization.Editor;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class GameUI : MonoBehaviour
{
    [SerializeField] private Button _extractButton;

    [SerializeField] private TimerSliderUI _hungerTimer;
    [SerializeField] private TimerSliderUI _healthTimer;

    [SerializeField] private Image _dayImage;
    [SerializeField] private Image _clockArrowImage;

    [SerializeField] private TextMeshProUGUI _dayNumber;

    private LocalizationService _localization;

    public static event Action ExtractAction;

    [Inject]
    public void Construct(LocalizationService localization)
    {
        _localization = localization;
    }

    private void OnEnable()
    {
        HungerTimer.ChangeTimerAction += _hungerTimer.ChangeTimer;
        Health.HealthChangeAction += _healthTimer.ChangeTimer;
        GameTime.ChangeTimeAction += ChangeClockArrow;
        GameTime.ChangeDayAction += ChangeDay;

        _extractButton.onClick.AddListener(Extract);
        SetDay();
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

    private void SetDay()
    {
        ChangeDay(0);
    }

    private void ChangeClockArrow(float time, float maxTime)
    {
        float progress = Mathf.Clamp01(time / maxTime);
        _clockArrowImage.rectTransform.rotation = Quaternion.Euler(0, 0, progress * -360f);
    }

    private async void ChangeDay(int number)
    {
        _dayNumber.text = await _localization.Get("Day", LocalizationTable.UI) + " " + number.ToString();
    }
}
