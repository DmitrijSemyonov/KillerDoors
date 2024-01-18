using KillerDoors.StaticDataSpace;
using KillerDoors.UI.StaticDataSpace;
using System.Collections.Generic;

namespace KillerDoors.Services.StaticDataSpace
{
    public interface IStaticDataService : IService
    {
        Dictionary<string, LevelStaticData> Levels { get; }

        LevelStaticData ForLevel(string sceneKey);
        WindowConfig ForWindow(WindowId windowId);
        GameStaticData GetGameData();
        void Load();
    }
}