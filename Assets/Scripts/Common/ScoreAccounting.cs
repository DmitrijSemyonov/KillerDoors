using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreAccounting : MonoBehaviour
{
    private Door[] _doors;
    private LosingZone _losingZone;
    private Inventory _inventory;
    private GameModeManager _gameModeManager;
    private PlayerDataLoader _dataLoader;

    private int _comboCount = 0;
    private int _pointsCount;
    private int _nextCoinOn = 2;
    private int _coinsCount = -1; 
    public int CoinsCount { get { return _coinsCount; }
        private set {
            _dataLoader.playerData.scoreCoins = value;
            _dataLoader.SaveData();
            _coinsCount = value;
            OnCoinsChanged?.Invoke(_coinsCount);
        } 
    }
    public int PointsCount { get { return _pointsCount; } private set { _pointsCount = value; OnPointsChanged?.Invoke(_pointsCount); } }
    private int _pointsAtLastGame;

    public event Action<int> OnCoinsChanged;
    public event Action<int> OnPointsChanged;
    public event Action<Vector3, int> OnComboChanged;
    void Start()
    {
        _doors = FindObjectsOfType<Door>();
        _losingZone = FindObjectOfType<LosingZone>();
        _inventory = FindObjectOfType<Inventory>();
        _dataLoader = PlayerDataLoader.Instance;
        _dataLoader.OnUpdate += InitCoinsCount;
        _gameModeManager = GetComponent<GameModeManager>();
        _gameModeManager.EndDoorGame += EndDoorGame;

        for (int i =0; i < _doors.Length; i++)
        {
            _doors[i].PersonKilled += PersonKilled;
        }

        _losingZone.LosePerson += PersonLosed;

        InitCoinsCount();
    }
    private void InitCoinsCount()
    {
        _coinsCount = _dataLoader.playerData.scoreCoins;
        OnCoinsChanged?.Invoke(_coinsCount);
    }
    public void PersonKilled(Person person)
    {
        _comboCount++;
        PointsCount += _comboCount;
        OnComboChanged?.Invoke(person.transform.position, _comboCount);
        if(_nextCoinOn <= _pointsCount)
        {
            _inventory.CreateCoin();
            _nextCoinOn *= 2;
            CoinsCount++;
        }
    }
    private void PersonLosed(Vector3 _)
    {
        _comboCount = 0;
    }
    private void EndDoorGame()
    {
        _pointsAtLastGame = PointsCount;
        _comboCount = 0;
        PointsCount = 0;
        _nextCoinOn = 2;
    }
    public void AddMergedCoinToScore(Coin coin)
    {
        CoinsCount += coin.Upgrade;
    }
    public void Spend(int price)
    {
        CoinsCount -= price;
    }
    public void SetMultiplyReward(int multiplier)
    {
        int newCoins = (int)Math.Log(_pointsAtLastGame, 2) * (multiplier - 1);
        CoinsCount += newCoins;
        for(int i =0; i < newCoins; i++)
        {
            _inventory.CreateCoin();
        }
    }
    private void OnDestroy()
    {
        for (int i = 0; i < _doors.Length; i++)
        {
            if(_doors[i] != null)
                _doors[i].PersonKilled -= PersonKilled;
        }
        if(_losingZone != null)
            _losingZone.LosePerson -= PersonLosed;

        if (_gameModeManager)
            _gameModeManager.EndDoorGame -= EndDoorGame;

        if (_dataLoader) 
            _dataLoader.OnUpdate -= InitCoinsCount;
    }
}
