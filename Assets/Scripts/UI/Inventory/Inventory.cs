using KillerDoors.Services.Merge;
using KillerDoors.Services.ProgressSpace;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KillerDoors.UI.InventorySpace
{
    public class Inventory : MonoBehaviour
    {
        [field: SerializeField] public Transform ItemsParent { get; private set; }
        [SerializeField] private Coin _coinPrefab;

        private Transform _dragParent;

        public event Action<Coin> CoinCreated;
        private List<Coin> _allCoins = new List<Coin>();

        private IMergeService _mergeService;
        private IProgressService _progress;
        public void Construct(IMergeService mergeService, IProgressService progress, Transform UIRoot)
        {
            _mergeService = mergeService;
            _progress = progress;
            _dragParent = UIRoot;
        }
        public void Init()
        {
            ClearAndCreateCoinsFromData();
        }
        public Coin CreateBaseCoin()
        {
            Coin coin = CreateCoin(1);
            _progress.CoinsDataManager.AddDataCoin(coin);
            return coin;
        }
        public List<Coin> CreateBaseCoins(int count)
        {
            List<Coin> coins = new List<Coin>();
            for (int i = 0; i < count; i++)
            {
                coins.Add(CreateBaseCoin());
            }
            return coins;
        }
        private Coin CreateCoin(int upgrade)
        {
            Coin coin = Instantiate(_coinPrefab, ItemsParent);
            coin.Upgrade = upgrade;
            coin.Init(ItemsParent, _dragParent, _mergeService, _progress);
            CoinCreated?.Invoke(coin);
            _allCoins.Add(coin);
            return coin;
        }
        private void ClearAndCreateCoinsFromData()
        {
            CheckAndDestroyExistingCoins();

            List<int> ListUpgradeCoins = _progress.CoinsDataManager.GetUpgradesCoinsList();
            
            for (int i = 0; i < ListUpgradeCoins.Count; i++)
                CreateCoin(ListUpgradeCoins[i]);
        }
        private void CheckAndDestroyExistingCoins()
        {
            for (int i = 0; i < _allCoins.Count; i++)
                Destroy(_allCoins[i].gameObject);

            _allCoins.Clear();
        }
    }
}