using System;
using System.Collections.Generic;

namespace KillerDoors.Data.Management
{
    public class ObservablePlayerProgressWithoutInventoryCoins
    {
        public Action DataChanged;

        public ObservableVariable<int> scoreCoins = new ObservableVariable<int>(0);
        public ObservableVariable<int> protection = new ObservableVariable<int>(0);
        public ObservableVariable<int> dinamit = new ObservableVariable<int>(0);
        public List<ObservableVariable<int>> upgradeCountsDoorsForCurrentLevel = new List<ObservableVariable<int>>();
        public ObservableVariable<bool> educationCompleted = new ObservableVariable<bool>(false);

        //no saving data
        public ObservableVariable<int> points = new ObservableVariable<int>(0);
        public ObservableVariable<int>[] upgradeDoorsPrices;
        public ObservableVariable<int> dinamitPrice = new ObservableVariable<int>(0);
        public ObservableVariable<int> protectionPrice = new ObservableVariable<int>(0);

        private PlayerProgress _serializableData;
        private LevelData _currentLevelData;

        public ObservablePlayerProgressWithoutInventoryCoins(PlayerProgress serializableData, Action DataChanged)
        {
            _serializableData = serializableData;

            this.DataChanged = DataChanged;
        }
        public void InitForLevel(string levelName)
        {
            scoreCoins.Value = _serializableData.scoreCoins;
            scoreCoins.Changed += UpdateScoreData;

            protection.Value = _serializableData.protection;
            protection.Changed += UpdateProtectionData;

            dinamit.Value = _serializableData.dinamit;
            dinamit.Changed += UpdateDinamitData;

            educationCompleted.Value = _serializableData.educationCompleted;
            educationCompleted.Changed += UpdateEducationData;

            _currentLevelData = _serializableData.levels.Find(data => data.levelName.Equals(levelName));

            upgradeCountsDoorsForCurrentLevel = _currentLevelData.upgradeCountsDoors.ConvertAll(upgrade => new ObservableVariable<int>(upgrade));
            upgradeCountsDoorsForCurrentLevel.ForEach(upgrade => upgrade.Changed += (x) => { UpdateUpgradeDoorsData(); });

            upgradeDoorsPrices = new ObservableVariable<int>[upgradeCountsDoorsForCurrentLevel.Count];
            for (int i = 0; i < upgradeDoorsPrices.Length; i++)
                upgradeDoorsPrices[i] = new ObservableVariable<int>(0);
        }
        public void SpendScoreCoins(int price) => 
            scoreCoins.Value -= price;
        public void Dispose()
        {
            scoreCoins.Dispose();
            protection.Dispose();
            dinamit.Dispose();
            educationCompleted.Dispose();
            points.Dispose();
            dinamitPrice.Dispose();
            protectionPrice.Dispose();
            upgradeCountsDoorsForCurrentLevel.ForEach(upgrade => upgrade.Dispose());
            if (upgradeDoorsPrices != null)
            {
                foreach (ObservableVariable<int> price in upgradeDoorsPrices)
                    price.Dispose();
            }

        }
        private void UpdateScoreData(int value)
        {
            _serializableData.scoreCoins = value;
            DataChanged?.Invoke();
        }
        private void UpdateProtectionData(int value)
        {
            _serializableData.protection = value;
            DataChanged?.Invoke();
        }
        private void UpdateDinamitData(int value)
        {
            _serializableData.dinamit = value;
            DataChanged?.Invoke();
        }
        private void UpdateUpgradeDoorsData()
        {
            _currentLevelData.upgradeCountsDoors = upgradeCountsDoorsForCurrentLevel.ConvertAll(x => x.Value);
            DataChanged?.Invoke();
        }
        private void UpdateEducationData(bool value)
        {
            _serializableData.educationCompleted = value;
            DataChanged?.Invoke();
        }
    }
}