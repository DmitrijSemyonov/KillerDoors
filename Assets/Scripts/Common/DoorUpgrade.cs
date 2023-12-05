using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorUpgrade : MonoBehaviour
{
    [field: SerializeField] public byte indexDoor { get; private set; }

    private const int _maxUpgrade = 24;
    private ScoreAccounting _scoreAccounting;
    private PlayerDataLoader _dataLoader;
    private Door _door;
    private int _upgrade;
    [SerializeField] private float _upgradeStepOpenTime = 0.02f;
    private int _upgradePrice = 2;
    public int UpgradePrice { get { return _upgradePrice; } 
        private set
        {
            _upgradePrice = value;
            UpgradePriceChanged?.Invoke(value);
        }
    }
    public event Action<int> UpgradePriceChanged;
    void Start()
    {
        _dataLoader = PlayerDataLoader.Instance;
        _scoreAccounting = FindObjectOfType<ScoreAccounting>();
        _door = GetComponent<Door>();
        _upgrade = _dataLoader.playerData.upgradeCountsDoors[indexDoor];
        
        for(int i =0; i < _upgrade; i++)
        {
            _door.UpgradeTimeReset(_upgradeStepOpenTime);
        }
        InitUpgradePrice();
    }
    private void InitUpgradePrice()
    {
        UpgradePrice = (int) Math.Pow(2, _upgrade);
    }
    public void UpgradeDoor()
    {
        if (_scoreAccounting.CoinsCount < _upgradePrice) return;
        if (_upgrade > _maxUpgrade) return;

        _upgrade++;
        _scoreAccounting.Spend(_upgradePrice);
        _door.UpgradeTimeReset(_upgradeStepOpenTime);
        UpgradePrice *= 2;
        _dataLoader.playerData.upgradeCountsDoors[indexDoor] = _upgrade;
        _dataLoader.SaveData();
    }
}
