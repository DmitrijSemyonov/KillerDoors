using KillerDoors.Common;
using KillerDoors.Services.AssetManagement;
using UnityEngine;

namespace KillerDoors.Services.Factories
{
    public class LosingZoneFactory : ILosingZoneFactory
    {
        private const string LosingZonePath = "Triggers/LoseTrigger";
        private IAssetProvider _assetProvider;

        public LosingZoneFactory(IAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
        }
        public LosingZone CreateLosingZone(Vector3 at)
        {
            LosingZone losingZone = _assetProvider.Instantiate<LosingZone>(LosingZonePath, at, Quaternion.identity.eulerAngles);
            return losingZone;
        }
    }
}