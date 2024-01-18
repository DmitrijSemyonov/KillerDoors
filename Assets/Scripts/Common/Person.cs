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

        public ParticleSystem[] explosions;

        public event Action<Person> Destroyed;
        private void Start() =>
            explosions = GetComponentsInChildren<ParticleSystem>();
        public void Construct(IStaticDataService staticDataService)
        {
            int index = (int)_type - 1;
            _moveSpeed = staticDataService.GetGameData().moveSpeedsPersons[index];
        }
        private void Update() => 
            transform.Translate(CachedMath.VectorForward * Time.deltaTime * _moveSpeed);

        public void Kill()
        {
            for (int i = 0; i < explosions.Length; i++)
            {
                explosions[i].Play();
                explosions[i].gameObject.transform.parent = null;
                Destroy(explosions[i].gameObject, 1f);
            }
            DestroyPerson();
        }
        public void DestroyPerson()
        {
            Destroyed?.Invoke(this);
            Destroy(gameObject);
        }
        private void OnDestroy() =>
            Destroyed = null;
    }
}