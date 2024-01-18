using System;
using UnityEngine;

namespace KillerDoors.StaticDataSpace
{
    [Serializable]
    public class SpawnPointStaticData
    {
        [HideInInspector] public string id;
        [HideInInspector] public Vector3 position;
        public int floorNumber;
        public SpawnPointStaticData(string id, Vector3 position, int floorNumber)
        {
            this.id = id;
            this.position = position;
            this.floorNumber = floorNumber;
        }
    }
}