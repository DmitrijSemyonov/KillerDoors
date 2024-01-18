using System;
using System.Collections.Generic;
namespace KillerDoors.Data
{
    [Serializable]
    public class LevelData
    {
        public string levelName;
        public List<int> upgradeCountsDoors;
    }
}