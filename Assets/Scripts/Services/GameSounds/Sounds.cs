using KillerDoors.Common;
using KillerDoors.Services.Factories;
using KillerDoors.Services.Merge;
using System.Collections.Generic;
using UnityEngine;

namespace KillerDoors.Services.GameSounds
{
    public class Sounds : ISounds
    {
        private List<Door> _doors;

        private readonly IMergeService _mergeService;
        private readonly ISoundsFactory _soundsFactory;

        private AudioSource _hit;
        private AudioSource _ring;

        public Sounds(IMergeService mergeService, ISoundsFactory soundsFactory)
        {
            _mergeService = mergeService;
            _soundsFactory = soundsFactory;

            _mergeService.MergeResult += PlayRing;
        }

        public void Init(List<Door> doors)
        {
            _doors = doors;
            _hit = _soundsFactory.CreateHit(Vector3.zero);
            _ring = _soundsFactory.CreateRing(Vector3.zero);
            _soundsFactory.CreateAmbient(Vector3.zero);

            Subscribes();
        }

        private void PlayRing(Vector3 _, string result, Color __)
        {
            if (result.Equals("SUCCESS"))
                _ring.Play();
        }
        private void PlayHit(Person _) =>
            _hit.Play();

        private void Subscribes()
        {
            _doors.ForEach(door => door.PersonKilled += PlayHit);
        }
    }
}