using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[Serializable]
public class TimerSliderUI
{
    [SerializeField] private Slider _timerSlider;
    [SerializeField] private ParticleSystem _timerParticles;
    [SerializeField] private TextMeshProUGUI _timerText;

    public void ChangeTimer(int value, int maxValue)
    {
        _timerSlider.DOKill();
        _timerSlider.DOValue((float)value / maxValue, 1f);
        _timerText.text = value.ToString();
    }
}
