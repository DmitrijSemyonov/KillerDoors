using KillerDoors.Common;
using KillerDoors.UI.InventorySpace;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KillerDoors.Services.ScoreSpace
{
    public interface IScoreService : IService
    {
        event Action<Vector3, int> ComboChangeAt;

        void AddMergedCoinToScore(Coin coin);
        void Describes();
        int EarnedCoins();
        void EndDoorGame();
        void Init(LosingZone losingZone, List<Door> doors, Inventory inventory);
        void AddPointsPerPerson(Person person);
        void SetMultiplyReward(int multiplier);
        void Subscribes();
    }
}