using UnityEngine;
using System.Collections.Generic;

namespace KillerDoors.StaticDataSpace
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "Static Data/Level")]
    public class LevelStaticData : ScriptableObject
    {
        public string levelKey;
        public List<DoorStaticData> doorsDatas;
        public Vector3 looseTriggerPosition;
        public List<SpawnPointStaticData> spawnPointsDatas;
    }
}