using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Shop : MonoBehaviour //Upgrades doors in DoorUpgrade classes aside
{
    [SerializeField] private int[] _protectionPrices = new int[5] {6, 25, 100, 400, 1600 };

    private int _nextProtectionPrice;
    public int NextProtectionPrice { get { return _nextProtectionPrice; } 
        private set {
            _nextProtectionPrice = value;
            ProtectionPriceChanged?.Invoke(value);
        } }
    public event Action<int> ProtectionPriceChanged;

    [SerializeField] private int[] _dinamitPrices = new int[5] { 4, 16, 64, 256, 1024 };
    private int _dinamitCount = -1;
    public int DinamitCount { get { return _dinamitCount; } 
        set
        {
            PlayerDataLoader.Instance.playerData.dinamit = value;
            PlayerDataLoader.Instance.SaveData();
            _dinamitCount = value;
            DinamitCountChanged?.Invoke(value);
            ConfigureNextDinamitPrice(value);
        }
    }
    public event Action<int> DinamitCountChanged;
    private int _nextDinamitPrice;
    public int NextDinamitPrice { get { return _nextDinamitPrice; }
       private set
        {
            _nextDinamitPrice = value;
            DinamitPriceChanged?.Invoke(value);
        }
    }
    public event Action<int> DinamitPriceChanged;


    private ScoreAccounting _scoreAccounting;

    private LosingZone _losingZone;


    private void Start()
    {
        _losingZone = FindObjectOfType<LosingZone>();
        _scoreAccounting = GetComponent<ScoreAccounting>();
        _losingZone.ProtectionChanged += ConfigureNextProtectionPrice;
        PlayerDataLoader.Instance.OnUpdate += InitDinamitCount;
        Invoke("InitDinamitCount", 0.1f);
    }
    private void InitDinamitCount()
    {
        _dinamitCount = PlayerDataLoader.Instance.playerData.dinamit;
        ConfigureNextDinamitPrice(_dinamitCount);
        DinamitCountChanged?.Invoke(_dinamitCount);
    }
    private void ConfigureNextProtectionPrice(int protection)
    {
        if(protection >= _protectionPrices.Length)
        {
            NextProtectionPrice = _protectionPrices[_protectionPrices.Length - 1];
        }
        else
        {
            NextProtectionPrice = _protectionPrices[protection];
        }
    }
    private void ConfigureNextDinamitPrice(int dinamitCount)
    {
        if (dinamitCount >= _protectionPrices.Length)
        {
            NextDinamitPrice = _dinamitPrices[_dinamitPrices.Length - 1];
        }
        else
        {
            NextDinamitPrice = _protectionPrices[dinamitCount];
        }
    }
    public void BuyProtection()
    {
        if (_scoreAccounting.CoinsCount < _nextProtectionPrice) return;

        _scoreAccounting.Spend(_nextProtectionPrice);
        _losingZone.Protection++;
    }
    public void BuyDinamit()
    {
        if (_scoreAccounting.CoinsCount < _nextDinamitPrice) return;

        _scoreAccounting.Spend(_nextDinamitPrice);
        DinamitCount++;
    }
    public void ExploseAll()
    {
        if (_dinamitCount <= 0) return;

        Person[] persons = FindObjectsOfType<Person>();
        if (persons.Length > 0)
            DinamitCount--;

        for (int i = 0; i < persons.Length; i++)
        {
            persons[i].Kill();
            _scoreAccounting.PersonKilled(persons[i]);
        }
    }
    private void OnDestroy()
    {
        if(_losingZone)
            _losingZone.ProtectionChanged -= ConfigureNextProtectionPrice;
        if(PlayerDataLoader.Instance != null)
            PlayerDataLoader.Instance.OnUpdate -= InitDinamitCount;
    }
}
