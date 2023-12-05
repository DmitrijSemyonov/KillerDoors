using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoCache
{
    private DoorController _doorController;
    private Shop _shop;
    void Start()
    {
        _doorController = GetComponent<DoorController>();
        _shop = GetComponent<Shop>();
    }

    protected override void OnTick()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _doorController.CloseAllDoors();
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            _shop.ExploseAll();
        }
    }
}
