using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField] private Button _button;

    [SerializeField] private Image _timerImage;

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
        _timerImage.fillAmount = (float)value / maxValue;
        _timerText.text = value.ToString();
    }
}
