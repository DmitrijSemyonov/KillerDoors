using KillerDoors.Common;
using KillerDoors.Services.InputSpace;
using KillerDoors.Services.MonoBehaviourFunctions;
using System.Collections.Generic;
using UnityEngine;

namespace KillerDoors.Services.DoorControl
{
    public class DoorController : UpdateListener, IDoorController
    {
        private List<Door> _activeDoors = new List<Door>();

        private List<float> _lastCloseTime = new List<float>();

        private IInputService _inputService;

        public DoorController(IMonoBehaviourService monoBehaviourService, IInputService inputService) : base(monoBehaviourService)
        {
            _inputService = inputService;
        }
        public void Init(List<Door> doors)
        {
            Describes();

            _activeDoors = doors;
            for (int i = 0; i < _activeDoors.Count; i++)
            {
                _activeDoors[i].PersonKilled += ResetLastCloseTime;
                _lastCloseTime.Add(-999f); //For first action
            }
        }
        public void Describes()
        {
            foreach (Door door in _activeDoors)
            {
                if (door)
                    door.PersonKilled -= ResetLastCloseTime;
            }
        }
        protected override void OnTick()
        {
            if (_inputService.IsCloseDoorPressed)
                TryCloseAllDoors();
        }
        public void TryCloseAllDoors()
        {
            for (int i = 0; i < _activeDoors.Count; i++)
            {
                bool isReadyToClose = _lastCloseTime[i] + _activeDoors[i].OpenTime < Time.time;
                if (!isReadyToClose) continue;

                _lastCloseTime[i] = Time.time;
                _activeDoors[i].Close();
            }
        }
        public void ResetLastCloseTime(Person person)
        {
            for (int i = 0; i < _activeDoors.Count; i++)
            {
                _lastCloseTime[i] = - 999f;
                _activeDoors[i].InstantResetState();
            }
        }
    }
}