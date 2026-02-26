using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameUI : MonoBehaviour
{
    [SerializeField] private Slider _timerSlider;
    [SerializeField] private ParticleSystem _timerParticles;

    [SerializeField] private TextMeshProUGUI _timerText;

    private void OnEnable()
    {
        GameTimer.ChangeTimerAction += ChangeTimer;
    }

    private void OnDisable()
    {
        GameTimer.ChangeTimerAction -= ChangeTimer;      
    }

    private void ChangeTimer(int value, int maxValue)
    {
        _timerSlider.DOKill();
        _timerSlider.DOValue((float)value / maxValue, 1f);
        _timerText.text = value.ToString();
    }
}
