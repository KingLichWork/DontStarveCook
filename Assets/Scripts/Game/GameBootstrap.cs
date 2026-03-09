using FindTheDifference.Audio;
using Kimicu.YandexGames;
using System;
using UnityEngine;
using UnityEngine.InputSystem.XInput;
using VContainer.Unity;

public class GameBootstrap : IInitializable
{
    private GameController _gameController;
    private AudioManager _audioManager;
    private GameUI _gameUI;
    private InputController _inputController;

    public GameBootstrap(GameController gameController, AudioManager audioManager, GameUI gameUI, InputController inputController)
    {
        _gameController = gameController;
        _audioManager = audioManager;
        _gameUI = gameUI;
        _inputController = inputController;
    }

    public void Initialize()
    {
        try
        {
            SaveManager.LoadAll();

            _gameUI.Init();
            _inputController.Init();
            _gameController.Init();
            _audioManager.Init();

            YandexGamesSdk.GameReady();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }
}
