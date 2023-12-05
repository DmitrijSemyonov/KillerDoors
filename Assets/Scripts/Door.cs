using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoCache
{
    [SerializeField] private Transform _meshGO;
    private BoxCollider _boxCollider;
    [field: SerializeField] public float OpenTime { get; private set; } = 1f;
    private float _openTimeWithCoef;
    [SerializeField] private float _closeTime = 0.05f;
    private float _closeTimeWithCoef;

    public event Action<Person> PersonKilled;

    [SerializeField] private Vector3 _openState = new Vector3(0f, -280f, 0f) ;
    [SerializeField] private Vector3 _closeState = new Vector3(0f, -160f, 0f);
    private bool _isOpening;
    private bool _isClosing;
    private bool _resettedOnClosing;
    
    private Vector3 _smoothDampVelocity = CachedMath.Vector3Zero;
    public event Action<float> OpenProgress;
    protected void Start()
    {
        _boxCollider = GetComponentInChildren<BoxCollider>();
        GetComponentInChildren<Trigger>().OnEnter += OnEnter;
        _closeTimeWithCoef = _closeTime / 5.9f; // coefficient of dismiss SmoothDamp
        _openTimeWithCoef = OpenTime / 5.9f;
    }
    protected override void OnTick()
    {
        if (_isClosing)
        {
            Closing();
        }
        else if (_isOpening)
        {
            Opening();
        }
    }

    private void Opening()
    {
        _meshGO.localEulerAngles = CachedMath.SmoothDampAngle(_meshGO.localEulerAngles, _openState, ref _smoothDampVelocity, _openTimeWithCoef, float.MaxValue, Time.deltaTime);
        if (!_resettedOnClosing)
        {
            OpenProgress?.Invoke(1f - (_meshGO.localEulerAngles.y - _openState.y) / (_closeState.y - _openState.y));
        }
        if (_meshGO.localEulerAngles.CompareAngles(_openState))
        {
            ResetDoor();
        }
    }

    private void Closing()
    {
        _meshGO.localEulerAngles = CachedMath.SmoothDampAngle(_meshGO.localEulerAngles, _closeState, ref _smoothDampVelocity, _closeTimeWithCoef, float.MaxValue, Time.deltaTime);
        if (_meshGO.localEulerAngles.CompareAngles(_closeState))
        {
            _isClosing = false;
            _isOpening = true;
            _smoothDampVelocity = CachedMath.Vector3Zero;
            _boxCollider.enabled = false;
        }
    }

    public void Close()
    {
        _resettedOnClosing = false;
        enabled = true;
        _isClosing = true;
        _isOpening = false;
        _boxCollider.enabled = true;
    }
    private void ResetDoor()
    {
        _boxCollider.enabled = true;
        OpenProgress?.Invoke(1f);
        _meshGO.localEulerAngles = _openState;
        _smoothDampVelocity = CachedMath.Vector3Zero;
        _isClosing = false;
        _isOpening = false;
        enabled = false;
    }
    // With animation continue
    public void InstantResetDoor()
    {
        OpenProgress?.Invoke(1f);
        _resettedOnClosing = true;
    }
    public void UpgradeTimeReset(float upgradeStepOpenTime)
    {
        OpenTime -= upgradeStepOpenTime;
        _openTimeWithCoef = OpenTime / 5.9f;
    }
    private void OnEnter(Collider other)
    {
        if (other.TryGetComponent<Person>(out Person person))
        {
            person.Kill();
            PersonKilled?.Invoke(person);
            InstantResetDoor();
        }
    }
}
