using System;
using UnityEngine;
using UnityEngine.UI;

public class TutorialUI : UIPanel
{
    [SerializeField] private Button CompleteTutorialButton;

    public event Action CompleteTutorialAction;

    private void OnEnable()
    {
        CompleteTutorialButton.onClick.AddListener(CompleteTutorial);
    }

    private void OnDisable()
    {
        CompleteTutorialButton.onClick.RemoveListener(CompleteTutorial);
    }

    private void CompleteTutorial()
    {
        SaveManager.PlayerData.Tutorial = true;
        Hide();
        CompleteTutorialAction.Invoke();
    }
}
