using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    public event Action<Collider> OnEnter;

    private void OnTriggerEnter(Collider other)
    {
        OnEnter?.Invoke(other);
    }
}
