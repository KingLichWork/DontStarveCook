using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndGameUI : UIPanel
{
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _continueButton;

    [SerializeField] private TextMeshProUGUI _headerText;
    [SerializeField] private TextMeshProUGUI _scoreText;

    public static event Action RestartAction;
    public static event Action ContinueAction;

    private void OnEnable()
    {
        Health.GameOverAction += Show;

        _restartButton.onClick.AddListener(Restart);
        _continueButton.onClick.AddListener(Continue);
    }

    private void OnDisable()
    {
        Health.GameOverAction -= Show;

        _restartButton.onClick.RemoveListener(Restart);
        _continueButton.onClick.RemoveListener(Continue);
    }

    protected override void OnShow()
    {
        _headerText.text = LocalizationService.GetLocalizedString("youSurvive", LocalizationTable.UI, new {value = SaveManager.PlayerData.Day});
        _scoreText.text = SaveManager.PlayerData.Score.ToString(); 
    }

    private void Restart()
    {
        RestartAction.Invoke();
    }
    private void Continue()
    {
        ContinueAction.Invoke();
    }
}
