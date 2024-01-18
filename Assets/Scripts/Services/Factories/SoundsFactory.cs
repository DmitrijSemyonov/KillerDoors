using KillerDoors.Services.AssetManagement;
using UnityEngine;

namespace KillerDoors.Services.Factories
{
    public class SoundsFactory : ISoundsFactory
    {
        private const string HitPath = "Audio/Hit";
        private const string RingPath = "Audio/Ring";
        private const string AmbientPath = "Audio/Ambient";

        private readonly IAssetProvider _assetProvider;

        public SoundsFactory(IAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
        }

        public AudioSource CreateHit(Vector3 at)
        {
            AudioSource hitSound = _assetProvider.Instantiate<AudioSource>(HitPath, at, Quaternion.identity, null);
            return hitSound;
        }
        public AudioSource CreateRing(Vector3 at)
        {
            AudioSource hitSound = _assetProvider.Instantiate<AudioSource>(RingPath, at, Quaternion.identity, null);
            return hitSound;
        }
        public AudioSource CreateAmbient(Vector3 at)
        {
            AudioSource hitSound = _assetProvider.Instantiate<AudioSource>(AmbientPath, at, Quaternion.identity, null);
            return hitSound;
        }
    }
}