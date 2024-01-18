using KillerDoors.UI.InventorySpace;
using System;
using System.Collections.Generic;

namespace KillerDoors.Data.Management
{
    public class CoinsDataManager
    {
        private PlayerProgress _serializableData;
        private Action DataChanged;
        public CoinsDataManager(PlayerProgress serializableData, Action DataChanged)
        {
            _serializableData = serializableData;
            this.DataChanged = DataChanged;
        }
        public void AddDataCoin(Coin coin)
        {
            _serializableData.upgradesCoins.Insert(coin.transform.GetSiblingIndex(), coin.Upgrade);
            DataChanged?.Invoke();
        }
        public void AddDataCoins(List<Coin> coins)
        {
            foreach (Coin coin in coins)
                _serializableData.upgradesCoins.Insert(coin.transform.GetSiblingIndex(), coin.Upgrade);

            DataChanged?.Invoke();
        }

        public void UpdateDataCoin(int index, int Upgrade)
        {
            _serializableData.upgradesCoins[index] = Upgrade;
            DataChanged?.Invoke();
        }

        public void RemoveDataCoin(int index)
        {
            _serializableData.upgradesCoins.RemoveAt(index);
            DataChanged?.Invoke();
        }
        public void RemoveDataCoinAfterDragging(int index)
        {
            _serializableData.upgradesCoins.RemoveAt(index);
            DataChanged?.Invoke();
        }
        public List<int> GetUpgradesCoinsList() =>
             _serializableData.upgradesCoins;

    }
}