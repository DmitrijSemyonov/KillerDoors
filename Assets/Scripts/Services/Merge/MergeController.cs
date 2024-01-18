using KillerDoors.Services.ProgressSpace;
using KillerDoors.Services.ScoreSpace;
using KillerDoors.Services.StaticDataSpace;
using KillerDoors.UI.InventorySpace;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KillerDoors.Services.Merge
{
    public class MergeController : IMergeService
    {
        public event Action<Vector3, string, Color> MergeResult;

        private Dictionary<Transform, Coin> _coins = new Dictionary<Transform, Coin>();

        private Inventory _inventory;
        private IProgressService _progressService;
        private IScoreService _scoreService;
        private IStaticDataService _staticDataService;

        private float[] _upgradeChancesOnLevels;

        public MergeController(IProgressService progressService, IScoreService scoreService, IStaticDataService staticDataService)
        {
            _progressService = progressService;
            _scoreService = scoreService;
            _staticDataService = staticDataService;
        }
        public void Init(Inventory inventory)
        {
            _upgradeChancesOnLevels = _staticDataService.GetGameData().coinUpgradeChancesOnLevels;

            _inventory = inventory;
            _inventory.CoinCreated += AddCoin;
        }
        public bool MergeOperation(Transform draggedCoinTransform, Transform coin2Transform)
        {
            bool isMerge = _coins[draggedCoinTransform].Upgrade == _coins[coin2Transform].Upgrade;
            if (isMerge)
            {
                int offset = _coins[draggedCoinTransform].IndexPriviousDragging < coin2Transform.GetSiblingIndex() ? -1 : 0;

                _progressService.CoinsDataManager.RemoveDataCoin(_coins[draggedCoinTransform].IndexPriviousDragging);

                bool isSuccess = GetMergeChance(draggedCoinTransform) >= UnityEngine.Random.Range(0f, 100f);

                _coins[coin2Transform].PerfomanceMergeResult(isSuccess);

                if (isSuccess)
                {
                    _progressService.CoinsDataManager.UpdateDataCoin(coin2Transform.GetSiblingIndex() + offset, _coins[coin2Transform].Upgrade);
                    _scoreService.AddMergedCoinToScore(_coins[coin2Transform]);

                    MergeResult?.Invoke(((RectTransform)coin2Transform.transform).position, "SUCCESS", Color.green);
                }
                else
                {
                    _progressService.CoinsDataManager.RemoveDataCoin(coin2Transform.GetSiblingIndex() + offset);
                    MergeResult?.Invoke(((RectTransform)coin2Transform.transform).position, "FAIL", Color.red);
                }
                UnityEngine.Object.Destroy(draggedCoinTransform.gameObject);
            }
            return isMerge;
        }
        public void AddCoin(Coin coin) =>
            _coins.Add(coin.transform, coin);
        private float GetMergeChance(Transform draggedCoin) =>
            _coins[draggedCoin].Upgrade >= _upgradeChancesOnLevels.Length
                ? _upgradeChancesOnLevels[_upgradeChancesOnLevels.Length - 1]
                : _upgradeChancesOnLevels[(int)Math.Log(_coins[draggedCoin].Upgrade, 2)];
    }
}