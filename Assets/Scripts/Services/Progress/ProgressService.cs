using KillerDoors.Data;
using KillerDoors.Data.Management;
using KillerDoors.Services.StaticDataSpace;
using System;
using System.Linq;

namespace KillerDoors.Services.ProgressSpace
{
    public class ProgressService : IProgressService
    {
        public PlayerProgress ProgressData { get; set; }
        public ObservablePlayerProgressWithoutInventoryCoins ObservableProgress { get; set; }

        public CoinsDataManager CoinsDataManager { get; set; }

        public event Action DataChanged;

        public void Init(IStaticDataService staticDataService)
        {
            if (ProgressData == null)
                ProgressData = new PlayerProgress(staticDataService.Levels.Values.ToList());

            ObservableProgress = new ObservablePlayerProgressWithoutInventoryCoins(ProgressData, Changed);
            CoinsDataManager = new CoinsDataManager(ProgressData, Changed);
        }
        private void Changed() =>
            DataChanged?.Invoke();
    }
}