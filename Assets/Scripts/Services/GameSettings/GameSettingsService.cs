using System;
using KillerDoors.Data;

namespace KillerDoors.Services.GameSettings
{
    public class GameSettingsService : IGameSettingsService
    {
        public SettingsData SettingsData { get; set; }
        public event Action DataChanged;
        public void Changed()
        {
            DataChanged?.Invoke();
        }
    }
}