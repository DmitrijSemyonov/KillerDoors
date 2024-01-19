using UnityEngine;

namespace KillerDoors.Common
{
    public class DeathEffect : MonoBehaviour
    {
        public ParticleSystem[] explosions;
        private void Start()
        {
            foreach (ParticleSystem explosion in explosions)
                explosion.Play();

            Destroy(gameObject, 0.6f);
        }
    }
}