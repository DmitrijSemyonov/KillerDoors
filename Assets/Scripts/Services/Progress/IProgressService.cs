using KillerDoors.Data;
using KillerDoors.Data.Management;
using KillerDoors.Services.StaticDataSpace;
using System;

namespace KillerDoors.Services.ProgressSpace
{
    public interface IProgressService : IService
    {
        PlayerProgress ProgressData { get; set; }
        ObservablePlayerProgressWithoutInventoryCoins ObservableProgress { get; set; }
        CoinsDataManager CoinsDataManager { get; set; }

        event Action DataChanged;
        void Init(IStaticDataService staticDataService);
    }
}