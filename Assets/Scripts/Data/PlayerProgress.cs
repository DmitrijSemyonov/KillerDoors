using KillerDoors.StaticDataSpace;
using System;
using System.Collections.Generic;

namespace KillerDoors.Data
{
    [Serializable]
    public class PlayerProgress
    {
        public List<int> upgradesCoins = new List<int>();
        public int scoreCoins;
        public int protection;
        public int dinamit;
        public bool educationCompleted;
        public List<LevelData> levels;
        public PlayerProgress(List<LevelStaticData> staticDatas)
        {
            levels = new List<LevelData>();
            foreach (LevelStaticData levelStaticData in staticDatas)
            {
                LevelData levelData = new LevelData()
                {
                    levelName = levelStaticData.levelKey,
                    upgradeCountsDoors = new List<int>(),
                };
                levelStaticData.doorsDatas.ForEach((_) => { levelData.upgradeCountsDoors.Add(1); });
                levels.Add(levelData);
            }
        }
    }
}
