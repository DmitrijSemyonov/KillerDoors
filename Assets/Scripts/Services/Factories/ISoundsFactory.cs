using UnityEngine;

namespace KillerDoors.Services.Factories
{
    public interface ISoundsFactory : IService
    {
        AudioSource CreateAmbient(Vector3 at);
        AudioSource CreateHit(Vector3 at);
        AudioSource CreateRing(Vector3 at);
    }
}