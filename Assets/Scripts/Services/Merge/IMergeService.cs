using KillerDoors.UI.InventorySpace;
using System;
using UnityEngine;

namespace KillerDoors.Services.Merge
{
    public interface IMergeService : IService
    {
        event Action<Vector3, string, Color> MergeResult;

        void Init(Inventory inventory);
        bool MergeOperation(Transform draggedCoin, Transform coin2);
    }
}