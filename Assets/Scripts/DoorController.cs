using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] private Door[] _firstFloorDoors;
    [SerializeField] private Door[] _secondFloorDoors;
    [SerializeField] private Door[] _thirdFloorDoors;
    [SerializeField] private Door[] _fourthFloorDoors;
    private List<Door> _activeDoors = new List<Door>();

    private List<float> _lastCloseTime = new List<float>();

    void Start()
    {
        _activeDoors.AddRange(_firstFloorDoors);
        _activeDoors.AddRange(_secondFloorDoors);
        _activeDoors.AddRange(_thirdFloorDoors);
        _activeDoors.AddRange(_fourthFloorDoors);

        for(int i=0; i < _activeDoors.Count; i++)
        {
            _activeDoors[i].PersonKilled += ResetLastCloseTime;
            _lastCloseTime.Add(-100f); //For first action
        }
    }
    public void CloseAllDoors()
    {
        for(int i=0; i< _activeDoors.Count; i++)
        {
            bool isReadyToClose = _lastCloseTime[i] + _activeDoors[i].OpenTime < Time.time;
            if (!isReadyToClose) continue;

            _lastCloseTime[i] = Time.time;
            _activeDoors[i].Close();
        }
    }
    public void ResetLastCloseTime(Person person)
    {
        for (int i = 0; i < _activeDoors.Count; i++)
        {
            _lastCloseTime[i] = -100f;
            _activeDoors[i].InstantResetDoor();
        }
    }

}
