using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private Transform _itemsParent;
    [SerializeField] private Transform _dragParent;
    [SerializeField] private Coin _coinPrefab;
    private PlayerDataLoader _dataLoader;
    public event Action<Coin> CoinCreated;
    private MergeController _mergeController;
    private PlayerDataManager _dataManager;
    void Start()
    {
        _mergeController = FindObjectOfType<MergeController>();
        _dataLoader = PlayerDataLoader.Instance;
        _dataManager = _dataLoader.GetComponent<PlayerDataManager>();
        ClearAndCreateCoinsFromData();
        _dataLoader.OnUpdate += ClearAndCreateCoinsFromData;
    }
    private void ClearAndCreateCoinsFromData()
    {
        CheckAndDestroyExistingCoins();

        List<int> ListUpgradeCoins = _dataLoader.playerData.upgradesCoins;
        if (ListUpgradeCoins.Count > 0)
        {
            for (int i = 0; i < ListUpgradeCoins.Count; i++)
            {
                Coin coin = Instantiate(_coinPrefab, _itemsParent);
                coin.Upgrade = ListUpgradeCoins[i];
                coin.Init(_dataManager, _itemsParent, _dragParent, _mergeController);
                CoinCreated?.Invoke(coin);
            }
        }
    }

    private void CheckAndDestroyExistingCoins()
    {
        Coin[] coins = FindObjectsOfType<Coin>();
        for (int i = 0; i < coins.Length; i++)
        {
            Destroy(coins[i].gameObject);
        }
    }

    public Coin CreateCoin()
    {
        Coin coin = Instantiate(_coinPrefab, _itemsParent);
        coin.Init(_dataManager, _itemsParent, _dragParent, _mergeController);
        _dataManager.AddDataCoin(coin);
        CoinCreated?.Invoke(coin);
        return coin;
    }

    private void OnDestroy()
    {
        if(_dataLoader)
            _dataLoader.OnUpdate -= ClearAndCreateCoinsFromData;
    }
}
