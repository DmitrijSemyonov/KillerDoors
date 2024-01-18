using KillerDoors.Common;
using KillerDoors.Services.ProgressSpace;
using KillerDoors.Services.StaticDataSpace;
using KillerDoors.StaticDataSpace;
using System;
using System.Collections.Generic;

namespace KillerDoors.Services.UpgradeDoor
{
    public class DoorUpgrade : IDoorUpgradeService
    {
        private GameStaticData _gameStaticData;
        public List<Door> Doors { get; private set; }

        private IProgressService _progress;

        public DoorUpgrade(IStaticDataService staticDataService, IProgressService progress)
        {
            _gameStaticData = staticDataService.GetGameData();
            _progress = progress;
        }
        public void Init(List<Door> doors)
        {
            Doors = doors;
            InitUpgradesDoors();

            _progress.ObservableProgress.upgradeCountsDoorsForCurrentLevel.ForEach(upgrade => upgrade.Changed += (x) => { UpdateUpgradeDoorsPrices(); });
            UpdateUpgradeDoorsPrices();
        }
        public void UpgradeDoor(int doorIndex)
        {
            if (_progress.ObservableProgress.scoreCoins.Value < _progress.ObservableProgress.upgradeDoorsPrices[doorIndex].Value) return;
            if (_progress.ObservableProgress.upgradeCountsDoorsForCurrentLevel[doorIndex].Value > _gameStaticData.upgradeDoor.maxDoorUpgrade) return;

            _progress.ObservableProgress.SpendScoreCoins(_progress.ObservableProgress.upgradeDoorsPrices[doorIndex].Value);

            _progress.ObservableProgress.upgradeCountsDoorsForCurrentLevel[doorIndex].Value++;

            Doors[doorIndex].UpgradeTimeReset(_gameStaticData.upgradeDoor.openTimeDoorUpgradeStep);
        }
        private void UpdateUpgradeDoorsPrices()
        {
            for (int i = 0; i < _progress.ObservableProgress.upgradeDoorsPrices.Length; i++)
                _progress.ObservableProgress.upgradeDoorsPrices[i].Value =
                    (int)Math.Pow(2, _progress.ObservableProgress.upgradeCountsDoorsForCurrentLevel[i].Value);
        }
        private void InitUpgradesDoors()
        {
            for (int j = 0; j < Doors.Count; j++)
            {
                for (int i = 0; i < _progress.ObservableProgress.upgradeCountsDoorsForCurrentLevel[j].Value; i++)
                {
                    Doors[j].UpgradeTimeReset(_gameStaticData.upgradeDoor.openTimeDoorUpgradeStep);
                }
            }
        }
    }
}