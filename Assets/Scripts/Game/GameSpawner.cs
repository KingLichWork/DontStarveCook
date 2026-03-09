using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private Transform _spawnPoint;

    [SerializeField] private FoodData _foodData;

    private CancellationTokenSource _cts;
    private GameUI _gameUI;

    private int _currentExtractValue; 
    private int _maxExtractValue = 5;

    private int _extractValue = 1;
    private int _autoExtractValue;

    private UpgradesData _upgradeData;
    private IObjectResolver _objectResolver;

    public int MaxExtractValue => _maxExtractValue;

    public static event Action<int,int> ExtractAction;

    [Inject]
    public void Construct(GameUI gameUI, UpgradesData upgradeData, IObjectResolver objectResolver)
    {
        _gameUI = gameUI;
        _upgradeData = upgradeData;
        _objectResolver = objectResolver;
    }

    public void Init()
    {
        Clear();

        _maxExtractValue = SaveManager.PlayerData.MaxExtractValue;
    }

    private void OnEnable()
    {
        _gameUI.ExtractAction += Extract;
    }

    private void OnDisable()
    {
        _gameUI.ExtractAction -= Extract;
    }

    public void ChangeExtractValue(int value)
    {
        _extractValue = 1 + value;
    }

    public void SpawnStartFood(int count)
    {
        for (int i = 0; i < count; i++)
            SpawnFood();
    }

    public void SpawnFood()
    {
        Food food = _foodData.GetRandomFood();

        Vector3 random = new Vector3(UnityEngine.Random.Range(-4, 4), 0);

        FoodViewGame prefab = _objectResolver.Instantiate(_prefab, _spawnPoint.position + random, Quaternion.identity).GetComponent<FoodViewGame>();
        prefab.transform.SetParent(_spawnPoint);
        prefab.SetFood(food);
    }

    public void StartAutoExtract(int value)
    {
        if (value == 0)
            return;

        _autoExtractValue = value;

        StopAuto();

        _cts = new CancellationTokenSource();
        AutoExtract(_cts.Token).Forget();
    }

    private void Clear()
    {
        _autoExtractValue = 0;
        _currentExtractValue = 0;
        _extractValue = 1;
        _maxExtractValue = 5;
    }

    private void StopAuto()
    {
        _cts?.Cancel();
        _cts?.Dispose();
        _cts = null;
    }

    private async UniTaskVoid AutoExtract(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            await UniTask.WaitForSeconds(1);

            _currentExtractValue += _autoExtractValue;
            CheckExtract();
        }
    }

    private void Extract()
    {
        _currentExtractValue += _extractValue;
        CheckExtract();
    }

    private void CheckExtract()
    {
        if (_currentExtractValue >= _maxExtractValue)
        {
            SpawnFood();

            _maxExtractValue++;
            _currentExtractValue = 0;
        }

        ExtractAction.Invoke(_currentExtractValue, _maxExtractValue);
    }
}
