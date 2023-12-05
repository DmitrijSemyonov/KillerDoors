using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LosingZone : MonoBehaviour
{
    public event Action<Vector3> LosePerson;
    private int _protection = -1;
    public int Protection { get { return _protection; } 
        set
        {
            PlayerDataLoader.Instance.playerData.protection = value;
            PlayerDataLoader.Instance.SaveData();
            _protection = value;
            ProtectionChanged?.Invoke(value);
        } }
    public event Action<int> ProtectionChanged;

    private void Awake()
    {
        PlayerDataLoader.Instance.OnUpdate += InitProtection;
    }
    private void Start()
    {
        InitProtection();
    }
    private void InitProtection()
    {
        _protection = PlayerDataLoader.Instance.playerData.protection;
        ProtectionChanged?.Invoke(_protection);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Person>())
        {
            if (Protection > 0)
            {
                Protection--;
            }
            else
            {
                LosePerson?.Invoke(other.transform.position);
            }
            Destroy(other.gameObject);
        }
    }

}
