using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{
    private PlayerDataLoader _dataLoader;
    void Start()
    {
        _dataLoader = PlayerDataLoader.Instance;
    }
    public void AddDataCoin(Coin coin)
    {
        _dataLoader.playerData.upgradesCoins.Insert(coin.transform.GetSiblingIndex(), coin.Upgrade);
    }

    public void UpdateDataCoin(int index, int Upgrade)
    {
        _dataLoader.playerData.upgradesCoins[index] = Upgrade;
    }

    public void RemoveDataCoin(int index)
    {
        _dataLoader.playerData.upgradesCoins.RemoveAt(index);
    }
    public void RemoveDataCoinAfterDragging(int index)
    {
        _dataLoader.playerData.upgradesCoins.RemoveAt(index);
    }
}
