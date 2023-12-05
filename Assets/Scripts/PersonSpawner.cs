using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonSpawner : MonoCache
{
    private int _floorCount = 1;
    [SerializeField] private List<float> _baseNextFloorOpenOnSecond = new List<float>() { 5f, 10f, 30f };
    private List<float> _nextFloorOpenOnSecond = new List<float>();
    [SerializeField] private Person[] _persons;
    [SerializeField] private Transform[] _spawnPoints;

    private List<float> _timesSpawn = new List<float>();
    private List<float> _timersSpawn = new List<float>();

    [SerializeField] private float _minTimeBetweenSwapn = 1.5f;
    [SerializeField] private float _maxTimeBetweenSwapn = 4f;

    private GameModeManager _gameModeManager;

    protected void Start()
    {
        _gameModeManager = GetComponent<GameModeManager>();
        _gameModeManager.EndDoorGame += StopSpawnAndKill;
        enabled = false;
    }
    private void Init()
    {
        _timesSpawn.Clear();
        _timersSpawn.Clear();
        for (int i = 0; i < _floorCount; i++)
        {
            float rand = Random.Range(_minTimeBetweenSwapn, _maxTimeBetweenSwapn);
            if(_floorCount == 1)
            {
                rand /= 3f;
            }
            else if(_floorCount == 2)
            {
                rand /= 2f;
            }
            _timesSpawn.Add(0f);
            _timersSpawn.Add(rand);
        }
    }
    protected override void OnTick()
    {
        CheckTimersForSpawn();

        CheckTimeForOpenFloor();
    }
    private void CheckTimersForSpawn()
    {
        for (int i = 0; i < _timersSpawn.Count; i++)
        {
            bool isReadyToSpawn = _timesSpawn[i] > _timersSpawn[i];
            if (isReadyToSpawn)
            {
                _timesSpawn[i] = 0;

                int rand = Random.Range(0, 10);
                int indexPerson = 0;
                if (rand == 9)
                {
                    indexPerson = 2;
                }
                else if (rand >= 6 && i != 3) //for indexPerson = 2 exclude 4 floor
                {
                    indexPerson = 1;
                }
                Instantiate(_persons[indexPerson], _spawnPoints[i].position, Quaternion.identity);
                _timersSpawn[i] = Random.Range(_minTimeBetweenSwapn, _maxTimeBetweenSwapn);
                if (_floorCount == 1)
                {
                    _timersSpawn[i] /= 3f;
                }
                else if (_floorCount == 2)
                {
                    _timersSpawn[i] /= 2f;
                }
            }
            _timesSpawn[i] += Time.deltaTime;
        }
    }
    private void CheckTimeForOpenFloor()
    {
        if (_nextFloorOpenOnSecond.Count == 0) return;

        int indexForRemove = -1;
        for(int i =0; i < _nextFloorOpenOnSecond.Count; i++)
        {
            _nextFloorOpenOnSecond[i] -= Time.deltaTime;
            if(_nextFloorOpenOnSecond[i] <= 0f)
            {
                _floorCount++;
                Init();
                indexForRemove = i;
            }
        }
        if(indexForRemove != -1)
        {
            _nextFloorOpenOnSecond.RemoveAt(indexForRemove);
        }
    }
    public void StartSpawn()
    {
        _floorCount = 1;
        _nextFloorOpenOnSecond.Clear();
        _nextFloorOpenOnSecond.AddRange(_baseNextFloorOpenOnSecond);
        Init();
        enabled = true;
    }
    public void StopSpawnAndKill()
    {
        Person[] persons = FindObjectsOfType<Person>();
        for (int i = 0; i < persons.Length; i++)
        {
            persons[i].Kill();
        }
        enabled = false;
    }
    protected override void OnDestroy()
    {
        if(_gameModeManager)
        _gameModeManager.EndDoorGame -= StopSpawnAndKill;
    }
}
