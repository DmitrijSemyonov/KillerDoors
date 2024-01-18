using Helpers;
using Helpers.Math;
using KillerDoors.StaticDataSpace;
using System;
using UnityEngine;

namespace KillerDoors.Common
{
    public class Door : MonoBehaviour
    {
        public event Action<Person> PersonKilled;
        public event Action<float> OpenProgress;

        [SerializeField] private Transform _meshTransform;
        [SerializeField] private BoxCollider _boxCollider;
        [SerializeField] private Trigger _trigger;
        [SerializeField] private Vector3 _openState = new Vector3(0f, -280f, 0f);
        [SerializeField] private Vector3 _closeState = new Vector3(0f, -160f, 0f);
        [field: SerializeField] public float OpenTime { get; private set; }
        [SerializeField] private float _closeTime;
        private float _openTimeWithCoef;
        private float _closeTimeWithCoef;
        private const float SmoothDampDismissCoefficient = 5.9f;

        private bool _isOpening;
        private bool _isClosing;
        private bool _resettedOnClosing;

        private Vector3 _smoothDampVelocity = CachedMath.Vector3Zero;
        private float PersentageOfReadiness =>
            1f - (_meshTransform.localEulerAngles.y - _openState.y) / (_closeState.y - _openState.y);
        public void Init(DoorStaticData data)
        {
            _trigger.OnEnter += OnEnter;

            OpenTime = data.baseOpenTime;
            _closeTime = data.baseCloseTime;
            _closeTimeWithCoef = _closeTime / SmoothDampDismissCoefficient;
            _openTimeWithCoef = OpenTime / SmoothDampDismissCoefficient;
        }
        public void UpgradeTimeReset(float upgradeStepOpenTime)
        {
            OpenTime -= upgradeStepOpenTime;
            _openTimeWithCoef = OpenTime / SmoothDampDismissCoefficient;
        }
        public void Close()
        {
            _resettedOnClosing = false;
            _isClosing = true;
            _isOpening = false;
            enabled = true;
            _boxCollider.enabled = true;
        }        
        // With animation continue
        public void InstantResetState()
        {
            OpenProgress?.Invoke(1f);
            _resettedOnClosing = true;
        }
        private void Update()
        {
            if (_isClosing)
                Closing();
            else if (_isOpening)
                Opening();
        }
        private void Opening()
        {
            _meshTransform.localEulerAngles = MathExtensions.SmoothDampAngle(_meshTransform.localEulerAngles, _openState, ref _smoothDampVelocity,
                _openTimeWithCoef, float.MaxValue, Time.deltaTime);

            if (!_resettedOnClosing)
                OpenProgress?.Invoke(PersentageOfReadiness);

            if (_meshTransform.localEulerAngles.CompareAngles(_openState))
                ResetState();
        }

        private void Closing()
        {
            _meshTransform.localEulerAngles = MathExtensions.SmoothDampAngle(_meshTransform.localEulerAngles, _closeState, ref _smoothDampVelocity,
                _closeTimeWithCoef, float.MaxValue, Time.deltaTime);

            if (_meshTransform.localEulerAngles.CompareAngles(_closeState))
                OnDoorOpened();
        }
        private void OnDoorOpened()
        {
            _isClosing = false;
            _isOpening = true;
            _smoothDampVelocity = CachedMath.Vector3Zero;
            _boxCollider.enabled = false;
        }
        private void ResetState()
        {
            OpenProgress?.Invoke(1f);
            _meshTransform.localEulerAngles = _openState;
            _smoothDampVelocity = CachedMath.Vector3Zero;
            _isClosing = false;
            _isOpening = false;
            _boxCollider.enabled = true;
            enabled = false;
        }
        private void OnEnter(Collider other)
        {
            if (other.TryGetComponent(out Person person))
            {
                person.Kill();
                PersonKilled?.Invoke(person);
            }
        }
        private void OnDestroy()
        {
            PersonKilled = null;
            OpenProgress = null;
            if (_trigger)
                _trigger.OnEnter -= OnEnter;
        }
    }
}