using FindTheDifference.Audio;
using Kimicu.YandexGames;
using System;
using UnityEngine;
using VContainer.Unity;

public class GameBootstrap : IInitializable
{
    private GameController _gameController;
    private AudioManager _audioManager;
    private GameUI _gameUI;

    public GameBootstrap(GameController gameController, AudioManager audioManager, GameUI gameUI)
    {
        _gameController = gameController;
        _audioManager = audioManager;
        _gameUI = gameUI;
    }

    public void Initialize()
    {
        try
        {
            SaveManager.LoadAll();

            _gameUI.Init();
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
