using KillerDoors.Services.Factories;
using KillerDoors.Services.ScoreSpace;
using KillerDoors.Services.StaticDataSpace;
using UnityEngine;

namespace KillerDoors.Services.VFX
{
    public class VFXService : IVFXService
    {
        private readonly IVFXFactory _VFXFactory;
        private readonly IScoreService _scoreService;

        private Vector3 _deathEffectOffset;
        public VFXService(IVFXFactory vFXFactory, IStaticDataService staticDataService, IScoreService scoreService)
        {
            _VFXFactory = vFXFactory;
            _deathEffectOffset = staticDataService.GetGameData().deathEffectOffset;
            _scoreService = scoreService;

            _scoreService.ComboChangeAt += PlayDeathEffect;
        }
        private void PlayDeathEffect(Vector3 position, int _) => 
            _VFXFactory.CreateDeathEffect(position + _deathEffectOffset);
    }
}