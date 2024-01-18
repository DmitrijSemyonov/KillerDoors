using System;

namespace KillerDoors.Data
{
    [Serializable]
    public class DoorUpgradeData
    {
        public string marker;
        public int upgrade;

        public DoorUpgradeData(string marker, int upgrade)
        {
            this.upgrade = upgrade;
            this.marker = marker;
        }
    }
}