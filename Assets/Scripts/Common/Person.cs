using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Person : MonoCache
{
    [SerializeField] private float _moveSpeed = 3f;
    public ParticleSystem[] explosions;
    void Start()
    {
        explosions = GetComponentsInChildren<ParticleSystem>();
    }

    protected override void OnTick()
    {
        transform.Translate(CachedMath.VectorForward * Time.deltaTime * _moveSpeed);
    }

    public void Kill()
    {
        for(int i =0; i < explosions.Length; i++)
        {
            explosions[i].Play();
            explosions[i].gameObject.transform.parent = null;
            Destroy(explosions[i].gameObject, 1f);
        }
        Destroy(gameObject);
    }
}
