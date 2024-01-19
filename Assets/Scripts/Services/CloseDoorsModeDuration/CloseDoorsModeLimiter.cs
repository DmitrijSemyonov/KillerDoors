using KillerDoors.Common;
using KillerDoors.Services.PersonSpawn;
using KillerDoors.Services.StaticDataSpace;
using KillerDoors.UI.StaticDataSpace;
using KillerDoors.UI.Windows;
using UnityEngine;

namespace KillerDoors.Services.CloseDoorModeDuration
{
    public class CloseDoorsModeLimiter : ICloseDoorsModeLimiter
    {
        private readonly IStaticDataService _staticDataService;
        private readonly IPersonSpawner _personSpawner;
        private readonly IWindowsService _windowsService;

        private LosingZone _losingZone;

        private float _doorGameDuration;
        private float _startTimeDoorGame;
        
        private bool TimeGameElapsed =>
            Time.time - _startTimeDoorGame > _doorGameDuration;

        public CloseDoorsModeLimiter(IStaticDataService staticDataService, IPersonSpawner personSpawner, IWindowsService windowsService)
        {
            _staticDataService = staticDataService;
            _personSpawner = personSpawner;
            _windowsService = windowsService;
        }
        public void Init(LosingZone losingZone)
        {
            _doorGameDuration = _staticDataService.GetGameData().doorGameDuration;

            _losingZone = losingZone;

            Subscribes();
        }
        public void StartGame()
        {
            _startTimeDoorGame = Time.time;
            _personSpawner.StartSpawn();
        }
        private void Subscribes()
        {
            _losingZone.LosePerson += PersonLosed;
        }
        private void PersonLosed(Vector3 _)
        {
            if (TimeGameElapsed)
            {
                _personSpawner.StopSpawnAndKill();
                _windowsService.Open(WindowId.Result);
            }
        }
    }
}