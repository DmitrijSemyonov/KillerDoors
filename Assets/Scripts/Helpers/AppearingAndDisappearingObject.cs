using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//Used CachedMath Vector3 SmoothDamp
public class AppearingAndDisappearingObject : MonoCache
{
    private bool _appearing;
    private bool _disappearing;

    [SerializeField] private float _durationSec = 0.7f;
    private float _durationWithSmoothDampCoef;
    private Vector3 _smoothDampVelocity;
    [SerializeField] private bool _scale0AtAwake = true;
    [SerializeField] private bool _childrensControl;

    public bool IsDisappearing => _disappearing;
    public event Action Appeared;
    public event Action Disappeared;
    private Vector3 _appearingVector;
    private Vector3 _baseVector;
    private int _animationAppearingStage =0;
    private List<AppearingAndDisappearingObject> _childrens;
    private ScrollRect[] _scrollRects;
    public bool ChildrensControl { get { return _childrensControl; } }
    protected virtual void Awake()
    {
        _durationWithSmoothDampCoef = _durationSec / 5.9f;

        _baseVector = transform.localScale;
        if (_baseVector.x == 0 || _baseVector.y == 0)
        {
            _baseVector = new Vector3(1f, 1f, 1f);
        }

        _appearingVector = _baseVector * 1.2f;
        
        _scrollRects = GetComponentsInChildren<ScrollRect>();

        if (_scale0AtAwake)
        {
            CheckAndSetActiveScrollRects(false);
            transform.localScale = Vector3.zero;
        }
        if (_childrensControl)
        {
            _childrens = new List<AppearingAndDisappearingObject>(GetComponentsInChildren<AppearingAndDisappearingObject>());
            _childrens.Remove(this);
        }
        if (!_appearing && !_disappearing)
        {
            enabled = false;
        }
    }
    private void OnValidate()
    {
        _durationWithSmoothDampCoef = _durationSec / 5.9f;
    }
    protected override void OnTick()
    {
        if (_appearing)
        {
            Appearing();
        }

        if (_disappearing)
        {
            Disappearing();
        }
    }

    private void Appearing()
    {
        if (_animationAppearingStage == 0)
        {
            transform.localScale = Vector3.SmoothDamp(transform.localScale, _appearingVector, ref _smoothDampVelocity,
                _durationWithSmoothDampCoef * 0.5f, float.MaxValue, Time.unscaledDeltaTime);

            if (CachedMath.Compare(transform.localScale, _appearingVector))
            {
                _animationAppearingStage = 1;
                _smoothDampVelocity = CachedMath.Vector3Zero;
            }
        }
        else
        {
            transform.localScale = Vector3.SmoothDamp(transform.localScale, _baseVector, ref _smoothDampVelocity,
                _durationWithSmoothDampCoef * 0.5f, float.MaxValue, Time.unscaledDeltaTime);


            if (CachedMath.Compare(transform.localScale, _baseVector))
            {
                _smoothDampVelocity = CachedMath.Vector3Zero;
                _appearing = false;
                transform.localScale = _baseVector;
                enabled = false;
                Appeared?.Invoke();
                ResetAnimationAppearingStage();
                CheckAndSetActiveScrollRects(true);
                CheckAndStartAppearingChildrens();
            }
        }
    }
    private void Disappearing()
    {
        transform.localScale = Vector3.SmoothDamp(transform.localScale, CachedMath.Vector3Zero, ref _smoothDampVelocity,
            _durationWithSmoothDampCoef, float.MaxValue, Time.unscaledDeltaTime);

        if (CachedMath.Compare(transform.localScale, CachedMath.Vector3Zero))
        {
            _smoothDampVelocity = CachedMath.Vector3Zero;
            transform.localScale = CachedMath.Vector3Zero;
            _disappearing = false;
            enabled = false;
            Disappeared?.Invoke();
        }
    }
    private void CheckAndStartAppearingChildrens()
    {
        if (_childrensControl)
        {
            for (int i = 0; i < _childrens.Count; i++)
            {
                _childrens[i].StartAppearing();
            }
        }
    }

    private void CheckAndSetActiveScrollRects(bool isActive)
    {
        if (_scrollRects != null && _scrollRects.Length > 0)
        {
            for (int i = 0; i < _scrollRects.Length; i++)
            {
                _scrollRects[i].gameObject.SetActive(isActive);
            }
        }
    }

    public virtual void StartAppearing()
    {
        enabled = true;
        _appearing = true;
        transform.localScale = CachedMath.Vector3Zero;
        if (_disappearing) { _disappearing = false;   }
    }
    public virtual void StartDisappearing()
    {
        CheckAndSetActiveScrollRects(false);
        enabled = true;
        _disappearing = true;

        if (_childrensControl)
        {
            for (int i = 0; i < _childrens.Count; i++)
            {
                _childrens[i].StartDisappearing();
            }
        }

        if (_appearing) 
        {
            ResetAnimationAppearingStage();
            _appearing = false; 
        }
    }
    private void ResetAnimationAppearingStage()
    {
        _animationAppearingStage = 0;
    }
}
