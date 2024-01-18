using KillerDoors.Common;
using System.Collections.Generic;

namespace KillerDoors.Services.Factories
{
    public interface IDoorFactory : IService
    {
        List<Door> CreateDoors();
    }
}