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

    public int ExtractValue => _extractValue;

    public static event Action<int,int> ExtractAction;

    [Inject]
    public void Construct(GameUI gameUI)
    {
        _gameUI = gameUI;
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

    public void StartSpawn(float timeSpawn)
    {
        _spawnTime = timeSpawn;
        _cts = new CancellationTokenSource();
        SpawnFood(_cts.Token).Forget();
    }

    private async UniTaskVoid SpawnFood(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            await UniTask.Yield(PlayerLoopTiming.Update);

            float delta = Time.deltaTime;

            _toNextSpawnTime += delta;

            if(_toNextSpawnTime >= _spawnTime)
            {
                _toNextSpawnTime = 0;
                SpawnFood();
            }
        }
    }

    private void Extract()
    {
        _currentExtractValue++;

        if(_currentExtractValue >= _extractValue)
        {
            SpawnFood();

            _extractValue++;
            _currentExtractValue = 0;
        }

        ExtractAction.Invoke(_currentExtractValue, _extractValue);
    }

    public void StopSpawn()
    {
        _cts?.Cancel();
        _cts?.Dispose();
        _cts = null;
    }

    public void SpawnFood()
    {
        Food food = _foodData.GetRandomFood();

        FoodViewGame prefab = Instantiate(_prefab, _spawnPoint).GetComponent<FoodViewGame>();
        prefab.SetFood(food);
    }
}
