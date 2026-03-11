using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

public class EndGameUI : UIPanel
{
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _continueButton;

    [SerializeField] private TextMeshProUGUI _headerText;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _maxScoreText;

    public static event Action RestartAction;
    public static event Action ContinueAction;

    private void OnEnable()
    {
        GameController.EndGameAction += Show;

        _restartButton.onClick.AddListener(Restart);
        _continueButton.onClick.AddListener(Continue);
    }

    private void OnDisable()
    {
        GameController.EndGameAction -= Show;

        _restartButton.onClick.RemoveListener(Restart);
        _continueButton.onClick.RemoveListener(Continue);
    }

    protected override async void OnShow()
    {
        _headerText.text = await LocalizationService.GetLocalizedStringAsync("youSurvive", LocalizationTable.UI, new {value = SaveManager.PlayerData.Day});
        _scoreText.text = SaveManager.PlayerData.Score.ToString();
        _maxScoreText.text = SaveManager.PlayerData.MaxScore.ToString();
    }

    private void Restart()
    {
        RestartAction.Invoke();
    }

    private void Continue()
    {
        AdManager.ShowRewarded(onRewarded: () =>
        {
            Hide();
            ContinueAction.Invoke();
        });
    }
}
