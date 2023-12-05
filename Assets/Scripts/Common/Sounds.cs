using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sounds : MonoBehaviour
{
    private Door[] _doors;
    private MergeController _mergeController;
    private AudioSource _hit;
    private AudioSource _ring;
    void Start()
    {
        _doors = FindObjectsOfType<Door>();
        _mergeController = FindObjectOfType<MergeController>();
        _hit = GameObject.Find("Hit").GetComponent<AudioSource>();
        _ring = GameObject.Find("RingCoin").GetComponent<AudioSource>();

        for(int i =0; i < _doors.Length; i++)
        {
            _doors[i].PersonKilled += PlayHit;
        }
        _mergeController.MergeResult += PlayRing;
    }

    private void PlayRing(Vector3 _, string result, Color __)
    {
        if (result.Equals("SUCCESS"))
        {
            _ring.Play();
        }
    }
    private void PlayHit(Person _)
    {
        _hit.Play();
    }
    private void OnDestroy()
    {
        if(_doors != null && _doors.Length > 0)
        {
            for (int i = 0; i < _doors.Length; i++)
            {
                if(_doors[i]) 
                    _doors[i].PersonKilled -= PlayHit;
            }
        }
        _mergeController.MergeResult -= PlayRing;
    }
}
