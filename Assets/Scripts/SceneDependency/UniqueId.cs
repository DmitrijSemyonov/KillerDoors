using System;
using UnityEngine;

namespace KillerDoors.SceneDependency
{
    public class UniqueId : MonoBehaviour
    {
        public string id;

        public void GenerateId() =>
            id = $"{gameObject.scene.name}_{Guid.NewGuid()}";
    }
}