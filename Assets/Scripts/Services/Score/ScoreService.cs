using KillerDoors.Common;
using KillerDoors.Services.ProgressSpace;
using KillerDoors.UI.InventorySpace;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KillerDoors.Services.ScoreSpace
{
    public class ScoreService : IScoreService
    {
        public event Action<Vector3, int> ComboChangeAt;

        private IProgressService _progress;

        private List<Door> _doors;
        private LosingZone _losingZone;
        private Inventory _inventory;
        private int _nextCoinOn = 2;
        private int _combo;
        public ScoreService(IProgressService progress)
        {
            _progress = progress;
        }
        public void Init(LosingZone losingZone, List<Door> doors, Inventory inventory)
        {
            _doors = doors;
            _losingZone = losingZone;
            _inventory = inventory;
        }

        public void Subscribes()
        {
            _doors.ForEach(door => door.PersonKilled += AddPointsPerPerson);
            _losingZone.LosePerson += OnPersonLosed;
        }

        public void AddPointsPerPerson(Person person)
        {
            _combo++;
            ComboChangeAt?.Invoke(person.transform.position, _combo);
            _progress.ObservableProgress.points.Value += _combo;

            if (_nextCoinOn <= _progress.ObservableProgress.points.Value)
            {
                _inventory.CreateBaseCoin();
                _nextCoinOn *= 2;
                _progress.ObservableProgress.scoreCoins.Value++;
            }
        }
        public void EndDoorGame()
        {
            _combo = 0;
            _progress.ObservableProgress.points.Value = 0;
            _nextCoinOn = 2;
        }
        public void AddMergedCoinToScore(Coin coin)
        {
            _progress.ObservableProgress.scoreCoins.Value += coin.Upgrade;
        }
        public void SetMultiplyReward(int multiplier)
        {
            int newCoins = EarnedCoins() * (multiplier - 1);
            _progress.ObservableProgress.scoreCoins.Value += newCoins;
            _inventory.CreateBaseCoins(newCoins);
        }

        public int EarnedCoins() =>
            _progress.ObservableProgress.points.Value > 0
            ? (int)Math.Log(_progress.ObservableProgress.points.Value, 2)
            : 0;

        public void Describes()
        {
            for (int i = 0; i < _doors.Count; i++)
            {
                if (_doors[i] != null)
                    _doors[i].PersonKilled -= AddPointsPerPerson;
            }
            if (_losingZone != null)
                _losingZone.LosePerson -= OnPersonLosed;
        }
        private void OnPersonLosed(Vector3 _) =>
            _combo = 0;
    }
}