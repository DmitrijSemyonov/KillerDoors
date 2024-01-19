using Helpers.Math;
using KillerDoors.Services.Factories;
using KillerDoors.Services.StaticDataSpace;
using System;
using UnityEngine;

namespace KillerDoors.Common
{
    public class Person : MonoBehaviour
    {
        [SerializeField] private PersonType _type;
        [Header("Edit in Game Static Data")]
        [SerializeField] private float _moveSpeed;

        public event Action<Person> Destroyed;
        public void Construct(IStaticDataService staticDataService)
        {
            int index = (int)_type - 1;
            _moveSpeed = staticDataService.GetGameData().moveSpeedsPersons[index];
        }
        private void Update() => 
            transform.Translate(CachedMath.VectorForward * Time.deltaTime * _moveSpeed);

        public void Kill() =>
            DestroyPerson();
        public void DestroyPerson()
        {
            Destroyed?.Invoke(this);
            Destroy(gameObject);
        }
        private void OnDestroy() =>
            Destroyed = null;
    }
}