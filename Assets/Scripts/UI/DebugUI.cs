using System;
using UnityEngine;
using UnityEngine.UI;

public class DebugUI : MonoBehaviour
{
    [SerializeField] private GameObject _gameObject;

    [SerializeField] private Button _clearSavesButton;
    [SerializeField] private Button _endGameButton;

    public static event Action DebugEndGameAction;

    private void OnEnable()
    {
        _clearSavesButton.onClick.AddListener(ClearSaves);
        _endGameButton.onClick.AddListener(EndGame);
    }

    private void OnDisable()
    {
        _clearSavesButton.onClick.RemoveListener(ClearSaves);
        _endGameButton.onClick.RemoveListener(EndGame);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.D) && Input.GetKey(KeyCode.CapsLock) && Input.GetKey(KeyCode.LeftShift))
        {
            _gameObject.SetActive(!_gameObject.activeInHierarchy);
        }
    }

    private void ClearSaves()
    {
        SaveManager.DeleteAllSave();
    }

    private void EndGame()
    {
        DebugEndGameAction.Invoke();
    }
}
