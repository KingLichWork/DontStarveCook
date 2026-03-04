using DG.Tweening;
using FindTheDifference.Audio;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField] private Button _extractButton;

    [SerializeField] private Button _changeSoundButton;

    [SerializeField] private TimerSliderUI _hungerTimer;
    [SerializeField] private TimerSliderUI _healthTimer;

    [SerializeField] private Slider _extractSlider;

    [SerializeField] private Image _dayImage;
    [SerializeField] private Image _clockArrowImage;
    [SerializeField] private Image _backGroundImage;

    [SerializeField] private TextMeshProUGUI _dayNumber;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _extractText;

    [SerializeField] private Color _dayColor;
    [SerializeField] private Color _eveningColor;
    [SerializeField] private Color _nightColor;

    [SerializeField] private Sprite[] _soundChangeSprite = new Sprite[2];

    private Tween _scoreTween;

    public event Action ExtractAction;
    public event Action<bool> ChangeSoundAction;

    private void OnEnable()
    {
        HungerTimer.ChangeTimerAction += _hungerTimer.ChangeTimer;
        Health.HealthChangeAction += _healthTimer.ChangeTimer;
        GameTime.ChangeTimeAction += ChangeClockArrow;
        GameTime.ChangeDayAction += ChangeDay;
        GameTime.ChangeDayPhaseAction += ChangeDayPhase;
        ScoreManager.ChangeScoreAction += ChangeScore;
        GameSpawner.ExtractAction += ChangeExtract;

        _extractButton.onClick.AddListener(Extract);
        _changeSoundButton.onClick.AddListener(ChangeSound);

        SetDay();
    }

    private void OnDisable()
    {
        HungerTimer.ChangeTimerAction -= _hungerTimer.ChangeTimer;
        Health.HealthChangeAction -= _healthTimer.ChangeTimer;
        GameTime.ChangeTimeAction -= ChangeClockArrow;
        GameTime.ChangeDayAction -= ChangeDay;
        GameTime.ChangeDayPhaseAction -= ChangeDayPhase;
        ScoreManager.ChangeScoreAction -= ChangeScore;
        GameSpawner.ExtractAction -= ChangeExtract;

        _extractButton.onClick.RemoveAllListeners();
        _changeSoundButton.onClick.RemoveAllListeners();
    }

    private void Extract()
    {
        ExtractAction.Invoke();
    }

    private void ChangeExtract(int value, int maxValue)
    {
        _extractSlider.DOKill();
        _extractSlider.DOValue((float)value / maxValue, 1f);
        _extractText.text = $"{value}/{maxValue}";
    }

    private void ChangeSound()
    {
        ChangeSoundAction.Invoke(!AudioManager.IsVolumeActive);
        _changeSoundButton.image.sprite = AudioManager.IsVolumeActive ? _soundChangeSprite[0] : _soundChangeSprite[1];
    }

    private void SetDay()
    {
        ChangeDay(SaveManager.PlayerData.Day);
    }

    private void ChangeScore(int targetScore)
    {
        _scoreTween?.Kill();

        int current = int.Parse(_scoreText.text);

        _scoreTween = DOTween.To(
            () => current,
            x =>
            {
                current = x;
                _scoreText.text = current.ToString();
            },
            targetScore,
            0.5f
        ).SetEase(Ease.OutQuad);
    }

    private void ChangeClockArrow(float time, float maxTime)
    {
        float progress = Mathf.Clamp01(time / maxTime);
        _clockArrowImage.rectTransform.rotation = Quaternion.Euler(0, 0, progress * -360f);
    }

    private async void ChangeDay(int number)
    {
        _dayNumber.text = await LocalizationService.GetLocalizedStringAsync("Day", LocalizationTable.UI) + " " + number.ToString();
    }

    private void ChangeDayPhase(DayPhase dayPhase)
    {
        switch (dayPhase)
        {
            case DayPhase.Day:
                _backGroundImage.color = _dayColor;
                break;
            case DayPhase.Evening:
                _backGroundImage.color = _eveningColor;
                break;
            case DayPhase.Night:
                _backGroundImage.color = _nightColor;
                break;
        }
    }
}
