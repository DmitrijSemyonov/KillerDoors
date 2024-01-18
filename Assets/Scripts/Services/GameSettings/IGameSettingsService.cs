using KillerDoors.Data;
using System;

namespace KillerDoors.Services.GameSettings
{
    public interface IGameSettingsService : IService
    {
        public SettingsData SettingsData { get; set; }

        event Action DataChanged;

        void Changed();
    }
}