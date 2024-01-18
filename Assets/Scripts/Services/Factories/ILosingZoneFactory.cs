using KillerDoors.Common;
using UnityEngine;

namespace KillerDoors.Services.Factories
{
    public interface ILosingZoneFactory : IService
    {
        LosingZone CreateLosingZone(Vector3 at);
    }
}