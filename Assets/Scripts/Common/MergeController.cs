using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeController : MonoBehaviour
{
    private Dictionary<Transform, Coin> _coins = new Dictionary<Transform, Coin>();
    private PlayerDataManager _dataManager;
    private Inventory _inventory;
    private ScoreAccounting _scoreAccounting;

    [SerializeField] private float[] _upgradeChancesOnLevels = new float[] {50f, 50f, 40f, 40f, 30f, 30f, 25f, 25f, 20f, 20f, 20f, 15f };

    public event Action<Vector3, string, Color> MergeResult;
    void Awake()
    {
        _scoreAccounting = GetComponent<ScoreAccounting>();
        _dataManager = FindObjectOfType<PlayerDataManager>();
        _inventory = FindObjectOfType<Inventory>();
        _inventory.CoinCreated += AddCoin;
    }
    public bool MergeOperation(Transform draggedCoin, Transform coin2)
    {
        bool isMerge = GetCoin(draggedCoin).Upgrade == GetCoin(coin2).Upgrade;
        if(isMerge)
        {
            int offset = GetCoin(draggedCoin).IndexPriviousDragging < coin2.GetSiblingIndex() ? -1 : 0;

            _dataManager.RemoveDataCoin(GetCoin(draggedCoin).IndexPriviousDragging);

            bool isSuccess = GetMergeChance(draggedCoin) >= UnityEngine.Random.Range(0f, 100f);
            if (isSuccess)
            {
                _dataManager.UpdateDataCoin(coin2.GetSiblingIndex() + offset, GetCoin(coin2).Upgrade);
                _scoreAccounting.AddMergedCoinToScore(GetCoin(coin2));

                MergeResult?.Invoke(((RectTransform)coin2.transform).position, "SUCCESS", Color.green);
            }
            else
            {
                _dataManager.RemoveDataCoin(coin2.GetSiblingIndex() + offset);
                MergeResult?.Invoke(((RectTransform)coin2.transform).position, "FAIL", Color.red);
            }
            GetCoin(draggedCoin).PerfomanceMergeResult(isSuccess);
            GetCoin(coin2).PerfomanceMergeResult(isSuccess);
        }

        return isMerge;
    }

    private float GetMergeChance(Transform draggedCoin)
    {
        float mergeChance;
        if (GetCoin(draggedCoin).Upgrade >= _upgradeChancesOnLevels.Length)
        {
            mergeChance = _upgradeChancesOnLevels[_upgradeChancesOnLevels.Length - 1];
        }
        else
        {
            mergeChance = _upgradeChancesOnLevels[(int)Math.Log(GetCoin(draggedCoin).Upgrade, 2)];
        }

        return mergeChance;
    }

    public void AddCoin(Coin coin)
    {
        _coins.Add(coin.transform, coin);
    }

    public Coin GetCoin(Transform coinTransform)
    {
        return _coins[coinTransform];
    }
    private void OnDestroy()
    {
        if(_inventory)
            _inventory.CoinCreated -= AddCoin;
    }
}
