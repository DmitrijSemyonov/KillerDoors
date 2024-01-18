using KillerDoors.Data.Management;
using System;
using UnityEngine;

namespace KillerDoors.Common
{
    public class LosingZone : MonoBehaviour
    {
        public event Action<Vector3> LosePerson;
        private ObservablePlayerProgressWithoutInventoryCoins _progress;

        public void Construct(ObservablePlayerProgressWithoutInventoryCoins progress)
        {
            _progress = progress;
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Person person))
            {
                if (_progress.protection.Value > 0)
                    _progress.protection.Value--;
                else
                    LosePerson?.Invoke(other.transform.position);

                person.DestroyPerson();
            }
        }
        private void OnDestroy() => 
            LosePerson = null;
    }
}