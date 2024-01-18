using KillerDoors.Common;
using KillerDoors.Services.Factories;
using KillerDoors.Services.InputSpace;
using KillerDoors.Services.MonoBehaviourFunctions;
using KillerDoors.Services.ProgressSpace;
using KillerDoors.Services.ScoreSpace;
using KillerDoors.Services.StaticDataSpace;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KillerDoors.Services.PersonSpawn
{
    public class PersonSpawner : UpdateListener, IPersonSpawner
    {
        private List<float> _nextFloorOpenOnSecond = new List<float>();
        private int _floorCount;
        private Vector3[] _spawnPoints;
        private int[] _spawnChancesSinceLowSpeedPerson;

        private List<float> _timersSpawn = new List<float>();

        [SerializeField] private float _minTimeBetweenSwapn;
        [SerializeField] private float _maxTimeBetweenSwapn;

        private List<Person> _persons = new List<Person>();

        private readonly IStaticDataService _staticDataService;
        private readonly IPersonFactory _personFactory;
        private readonly IProgressService _progressService;
        private readonly IScoreService _scoreService;
        private readonly IInputService _input;

        public PersonSpawner(IMonoBehaviourService monoBehaviourService, IStaticDataService staticDataService, IPersonFactory personFactory,
            IProgressService progressService, IScoreService scoreService, IInputService input) : base(monoBehaviourService)
        {
            _staticDataService = staticDataService;
            _personFactory = personFactory;
            _progressService = progressService;
            _scoreService = scoreService;
            _input = input;
        }
        public void Init()
        {
            _spawnPoints = new Vector3[_staticDataService.ForLevel(SceneManager.GetActiveScene().name).spawnPointsDatas.Count];
            for (int i = 0; i < _spawnPoints.Length; i++)
                _spawnPoints[i] = _staticDataService.ForLevel(SceneManager.GetActiveScene().name).spawnPointsDatas[i].position;

            _spawnChancesSinceLowSpeedPerson = _staticDataService.GetGameData().spawnPerson.spawnChancesSinceLowSpeedPerson;

            _minTimeBetweenSwapn = _staticDataService.GetGameData().spawnPerson.minTimeBetweenSwapn;
            _maxTimeBetweenSwapn = _staticDataService.GetGameData().spawnPerson.maxTimeBetweenSwapn;
        }
        public void StartSpawn()
        {
            _floorCount = 1;
            _nextFloorOpenOnSecond.Clear();
            _nextFloorOpenOnSecond.AddRange(_staticDataService.GetGameData().spawnPerson.floorOpenOnSecond);
            OnFloorCountChanged();
            AddToUpdateList();
        }
        public void StopSpawnAndKill()
        {
            while (_persons.Count > 0)
                _persons[0].Kill();
            RemoveFromUpdateList();
        }
        public void TryExploseAll()
        {
            if (_progressService.ObservableProgress.dinamit.Value <= 0) return;

            if (_persons.Count > 0)
                _progressService.ObservableProgress.dinamit.Value--;

            _persons.ForEach(person => _scoreService.AddPointsPerPerson(person));
            while (_persons.Count > 0)
                _persons[0].Kill();
        }
        protected override void OnTick()
        {
            if (_input.IsDinamitPressed)
                TryExploseAll();

            CheckTimersForSpawn();

            CheckTimeForOpenFloor();
        }
        private void CheckTimersForSpawn()
        {
            for (int indexFloor = 0; indexFloor < _timersSpawn.Count; indexFloor++)
            {
                _timersSpawn[indexFloor] -= Time.deltaTime;

                bool isReadyToSpawn = _timersSpawn[indexFloor] < 0f;
                if (isReadyToSpawn)
                {
                    Person person = _personFactory.CreatePerson(GetRandomPersonType(indexFloor), _spawnPoints[indexFloor]);
                    _persons.Add(person);
                    person.Destroyed += PersonDestroyed;

                    _timersSpawn[indexFloor] = GetTimeBetweenSpawns(_minTimeBetweenSwapn);
                }
            }
        }

        private PersonType GetRandomPersonType(int indexFloor)
        {
            int rand = Random.Range(0, 100);
            PersonType personType = PersonType.MiddleSpeed;

            if (rand < _spawnChancesSinceLowSpeedPerson[0])
                personType = PersonType.LowSpeed;
            else if (rand >= 100 - _spawnChancesSinceLowSpeedPerson[2] && indexFloor != 3)
                personType = PersonType.HighSpeed;

            return personType;
        }

        private void CheckTimeForOpenFloor()
        {
            if (_nextFloorOpenOnSecond.Count == 0) return;

            for (int i = 0; i < _nextFloorOpenOnSecond.Count; i++)
            {
                _nextFloorOpenOnSecond[i] -= Time.deltaTime;
                if (_nextFloorOpenOnSecond[i] <= 0f)
                {
                    _floorCount++;
                    OnFloorCountChanged();
                    _nextFloorOpenOnSecond.RemoveAt(i);
                }
            }
        }

        private void OnFloorCountChanged()
        {
            _timersSpawn.Clear();
            for (int i = 0; i < _floorCount; i++)
                _timersSpawn.Add(GetTimeBetweenSpawns(_minTimeBetweenSwapn / 3f));
        }

        private float GetTimeBetweenSpawns(float minTimeBetweenSwapn)
        {
            float rand = Random.Range(minTimeBetweenSwapn, _maxTimeBetweenSwapn);
            if (_floorCount == 1)
                rand /= 3f;
            else if (_floorCount == 2)
                rand /= 2f;

            return rand;
        }
        private void PersonDestroyed(Person person) => 
            _persons.Remove(person);
    }
}