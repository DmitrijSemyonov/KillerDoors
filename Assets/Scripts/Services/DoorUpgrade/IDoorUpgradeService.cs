using KillerDoors.Common;
using System.Collections.Generic;

namespace KillerDoors.Services.UpgradeDoor
{
    public interface IDoorUpgradeService : IService
    {
        List<Door> Doors { get; }

        void Init(List<Door> doors);
        void UpgradeDoor(int doorIndex);
    }
}