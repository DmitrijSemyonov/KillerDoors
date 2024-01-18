using KillerDoors.Common;
using System.Collections.Generic;

namespace KillerDoors.Services.GameSounds
{
    public interface ISounds : IService
    {
        void Init(List<Door> doors);
    }
}