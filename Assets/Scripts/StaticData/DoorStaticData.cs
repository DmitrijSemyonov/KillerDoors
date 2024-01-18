using System;
using UnityEngine;

namespace KillerDoors.StaticDataSpace
{
    [Serializable]
    public class DoorStaticData
    {
        [HideInInspector] public string id;
        [HideInInspector] public Vector3 position;
        [HideInInspector] public Vector3 rotation;
        public float baseOpenTime = 0.8f;
        public float baseCloseTime = 0.1f;
        public Vector3 doorUpgradeViewOffset;
        public DoorStaticData(string id, Vector3 position, Vector3 rotation, float baseOpenTime, float baseCloseTime, Vector3 doorUpgradeViewOffset)
        {
            this.id = id;
            this.position = position;
            this.baseOpenTime = baseOpenTime;
            this.baseCloseTime = baseCloseTime;
            this.rotation = rotation;
            this.doorUpgradeViewOffset = doorUpgradeViewOffset;
        }
    }
}