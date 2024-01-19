using KillerDoors.Common;
using KillerDoors.Services;
using UnityEngine;

namespace KillerDoors.Services.Factories
{
    public interface IVFXFactory : IService
    {
        public DeathEffect CreateDeathEffect(Vector3 at);
    }
}