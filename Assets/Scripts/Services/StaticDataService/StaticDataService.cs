using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using KillerDoors.StaticDataSpace;
using KillerDoors.UI.StaticDataSpace;

namespace KillerDoors.Services.StaticDataSpace
{
    public class StaticDataService : IStaticDataService
    {
        private const string LevelsDataPath = "StaticData/Levels";
        private const string GameDataPath = "StaticData/GameStaticData";
        private const string WindowsDataPath = "StaticData/WindowsStaticData/WindowStaticData";

        public Dictionary<string, LevelStaticData> Levels { get; private set; }

        private Dictionary<WindowId, WindowConfig> _windowsConfigs;
        public void Load()
        {
            Levels = Resources
              .LoadAll<LevelStaticData>(LevelsDataPath)
              .ToDictionary(x => x.levelKey, x => x);

            _windowsConfigs = Resources.Load<WindowStaticData>(WindowsDataPath)
                .configs
                .ToDictionary(x => x.windowId, x => x);
        }
        public LevelStaticData ForLevel(string sceneKey) =>
         Levels.TryGetValue(sceneKey, out LevelStaticData staticData)
        ? staticData
        : null;

        public GameStaticData GetGameData() =>
            Resources.Load<GameStaticData>(GameDataPath);

        public WindowConfig ForWindow(WindowId windowId) =>
            _windowsConfigs.TryGetValue(windowId, out WindowConfig config)
            ? config
            : null;
    }
}