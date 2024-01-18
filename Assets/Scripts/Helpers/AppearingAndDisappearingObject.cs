using Helpers.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Helpers
{
    public class AppearingAndDisappearingObject : MonoBehaviour
    {
        [SerializeField] private float _overscaleOnAppear = 1.2f;
        [SerializeField] private float _durationSec = 0.7f;
        [SerializeField] private bool _scale0AtAwake = true;
        [SerializeField] private bool _childrensControl;
        [SerializeField] private bool _ignoreReappearing;
        [SerializeField] private bool _ignoreParentControl;
        public bool ChildrensControl { get { return _childrensControl; } }
        public bool IgnoreParentControl
        {
            get => _ignoreParentControl;
            set
            {
                _ignoreParentControl = value;
                if (value)
                    transform.parent.GetComponentInParent<AppearingAndDisappearingObject>().RemoveChild(this);
                else
                    GetComponentInParent<AppearingAndDisappearingObject>().AddChild(this);
            }
        }
        public bool IsDisappearing => _disappearing;

        public event Action Appeared;
        public event Action Disappeared;

        private bool _appearing;
        private bool _disappearing;

        private float _durationWithSmoothDampCoef;

        private Vector3 _smoothDampVelocity;
        private const float _smoothDampCoefficient = 5.9f;

        private Vector3 _appearingVector;
        private Vector3 _baseVector;

        private int _animationAppearingStage = 0;

        private List<AppearingAndDisappearingObject> _childrens;
        private ScrollRect[] _scrollRects;


        private void AddChild(AppearingAndDisappearingObject appearingAndDisappearingObject)
        {
            if (_childrens == null) return;
            if (_childrens.Contains(appearingAndDisappearingObject)) return;

            _childrens.Add(appearingAndDisappearingObject);
        }

        public void RemoveChild(AppearingAndDisappearingObject appearingAndDisappearingObject)
        {
            if (_childrens == null) return;

            _childrens.Remove(appearingAndDisappearingObject);
        }

        public virtual void StartAppearing()
        {
            enabled = true;
            _appearing = true;

            if (!_ignoreReappearing)
                transform.localScale = CachedMath.Vector3Zero;

            if (_disappearing) { _disappearing = false; }
        }
        public virtual void StartDisappearing()
        {
            CheckAndSetActiveScrollRects(false);

            enabled = true;
            _disappearing = true;

            if (_childrensControl)
                _childrens.ForEach(x => x.StartDisappearing());

            if (_appearing)
            {
                ResetAnimationAppearingStage();
                _appearing = false;
            }
        }
        protected virtual void Awake()
        {
            _durationWithSmoothDampCoef = _durationSec / _smoothDampCoefficient;

            _baseVector = transform.localScale;
            if (_baseVector.x == 0 || _baseVector.y == 0)
                _baseVector = new Vector3(1f, 1f, 1f);

            _appearingVector = _baseVector * _overscaleOnAppear;

            _scrollRects = GetComponentsInChildren<ScrollRect>();

            if (_scale0AtAwake)
            {
                CheckAndSetActiveScrollRects(false);
                transform.localScale = Vector3.zero;
            }
            if (_childrensControl)
            {
                _childrens = new List<AppearingAndDisappearingObject>(
                    GetComponentsInChildren<AppearingAndDisappearingObject>().Where(x => !x.IgnoreParentControl && x != this));
            }
            if (!_appearing && !_disappearing)
                enabled = false;
        }
        private void OnValidate()
        {
            _durationWithSmoothDampCoef = _durationSec / _smoothDampCoefficient;
        }
        private void Update()
        {
            if (_appearing)
                Appearing();

            if (_disappearing)
                Disappearing();
        }
        private void Appearing()
        {
            if (_animationAppearingStage == 0)
            {
                transform.localScale = Vector3.SmoothDamp(transform.localScale, _appearingVector, ref _smoothDampVelocity,
                    _durationWithSmoothDampCoef * 0.5f, float.MaxValue, Time.unscaledDeltaTime);

                if (transform.localScale.Compare(_appearingVector))
                {
                    _animationAppearingStage = 1;
                    _smoothDampVelocity = CachedMath.Vector3Zero;
                }
            }
            else
            {
                transform.localScale = Vector3.SmoothDamp(transform.localScale, _baseVector, ref _smoothDampVelocity,
                    _durationWithSmoothDampCoef * 0.5f, float.MaxValue, Time.unscaledDeltaTime);


                if (transform.localScale.Compare(_baseVector))
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

            if (transform.localScale.Compare(CachedMath.Vector3Zero))
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
                _childrens.ForEach(x => x.StartAppearing());
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
        private void ResetAnimationAppearingStage() =>
            _animationAppearingStage = 0;
    }
}