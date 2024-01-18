using KillerDoors.Data;

namespace KillerDoors.Services.SaveLoad
{
    public interface ISaveLoadService : IService
    {
        PlayerProgress LoadProgress();
        void DelayedSaveProgress();
        void CompareDataFromTheDeviceAndFromTheCloud(PlayerProgress dataFromTheCloud);
        SettingsData LoadSettings();
    }
}