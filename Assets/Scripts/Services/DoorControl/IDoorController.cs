using KillerDoors.Common;
using System.Collections.Generic;

namespace KillerDoors.Services.DoorControl
{
    public interface IDoorController : IService
    {
        void TryCloseAllDoors();
        void Init(List<Door> doors);
        void ResetLastCloseTime(Person person);
        void Describes();
    }
}