using System;
using UnityEngine;

namespace Helpers
{
    public class Trigger : MonoBehaviour
    {
        public event Action<Collider> OnEnter;

        private void OnTriggerEnter(Collider other) => 
            OnEnter?.Invoke(other);

        private void OnDestroy() => 
            OnEnter = null;
    }
}