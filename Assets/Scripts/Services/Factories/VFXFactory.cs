using KillerDoors.Common;
using KillerDoors.Services.AssetManagement;
using UnityEngine;

namespace KillerDoors.Services.Factories
{
    public class VFXFactory : IVFXFactory
    {
        private const string DeathEffectPath = "VFX/DeathEffect";
        private readonly IAssetProvider _assetProvider;

        public VFXFactory(IAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
        }
        public DeathEffect CreateDeathEffect(Vector3 at)
        {
            DeathEffect effect = _assetProvider.Instantiate<DeathEffect>(DeathEffectPath, at, Quaternion.identity, null);
            return effect;
        }
    }
}