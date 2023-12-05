using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeManager : MonoBehaviour
{
    private PersonSpawner _personSpawner;
    private UIManager _ui;
    private LosingZone _losingZone;
    [SerializeField] private float _doorGameDuration = 50f;
    private float _startTimeDoorGame;

    public event Action EndDoorGame;
    void Start()
    {
        _losingZone = FindObjectOfType<LosingZone>();
        _personSpawner = GetComponent<PersonSpawner>();
        _ui = GetComponent<UIManager>();
        _losingZone.LosePerson += PersonLosed;
    }

    public void StartCloseDoorMode()
    {
        _startTimeDoorGame = Time.time;
        _ui.SetCloseDoorState();
        _personSpawner.StartSpawn();
    }

    private void FinishCloseDoorMode()
    {
        _ui.SetCoinMergeState();
        EndDoorGame?.Invoke();
    }

    private void PersonLosed(Vector3 _)
    {
        bool timeGameElapsed = Time.time - _startTimeDoorGame > _doorGameDuration;
        if (timeGameElapsed)
        {
            FinishCloseDoorMode();
        }
    }
    private void OnDestroy()
    {
        if (_losingZone)
        {
            _losingZone.LosePerson -= PersonLosed;
        }
    }
}
