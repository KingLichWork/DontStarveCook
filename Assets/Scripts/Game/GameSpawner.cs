using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using VContainer;

public class GameSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private Transform _spawnPoint;

    [SerializeField] private FoodData _foodData;

    private CancellationTokenSource _cts;
    private GameUI _gameUI;

    private float _spawnTime;
    private float _toNextSpawnTime;

    private int _currentExtractValue; 
    private int _extractValue = 5;

    private UpgradesData _upgradeData;

    public int ExtractValue => _extractValue;

    public static event Action<int,int> ExtractAction;

    [Inject]
    public void Construct(GameUI gameUI, UpgradesData upgradeData)
    {
        _gameUI = gameUI;
        _upgradeData = upgradeData;
    }

    public void Init()
    {
        _extractValue = SaveManager.PlayerData.ExtractValue;
        ExtractAction.Invoke(_currentExtractValue, _extractValue);
    }

    private void OnEnable()
    {
        _gameUI.ExtractAction += Extract;
    }

    private void OnDisable()
    {
        _gameUI.ExtractAction -= Extract;
    }

    public void SpawnStartFood(int count)
    {
        for (int i = 0; i < count; i++)
            SpawnFood();
    }

    public void SpawnFood()
    {
        Food food = _foodData.GetRandomFood();

        FoodViewGame prefab = Instantiate(_prefab, _spawnPoint).GetComponent<FoodViewGame>();
        prefab.SetFood(food);
    }

    private void StopAuto()
    {
        _cts?.Cancel();
        _cts?.Dispose();
        _cts = null;
    }

    private void StartAuto()
    {
        _cts = new CancellationTokenSource();
        AutoExtract(_cts.Token).Forget();
    }

    private async UniTaskVoid AutoExtract(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            await UniTask.WaitForSeconds(1);

            _currentExtractValue += _upgradeData.GetUpgradeValue(UpgradeType.AutoExtract);
            CheckExtract();
        }
    }

    private void Extract()
    {
        _currentExtractValue += _upgradeData.GetUpgradeValue(UpgradeType.ExtractCount);
        CheckExtract();
    }

    private void CheckExtract()
    {
        if (_currentExtractValue >= _extractValue)
        {
            SpawnFood();

            _extractValue++;
            _currentExtractValue = 0;
        }

        ExtractAction.Invoke(_currentExtractValue, _extractValue);
    }
}
