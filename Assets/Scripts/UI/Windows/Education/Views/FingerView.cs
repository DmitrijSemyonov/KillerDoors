using Helpers.Math;
using KillerDoors.Services.StaticDataSpace;
using KillerDoors.StaticDataSpace;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KillerDoors.UI.Windows.Education.Views
{
    public class FingerView : MonoBehaviour
    {
        private GameStaticData.FingerView _animationData;
        private Image _fingerImage;

        private Vector3 _baseScale;

        private List<Action> _animationParts = new List<Action>();
        private int _currentNumberPart;

        private RectTransform _dragFrom;
        private RectTransform _dragTo;

        private RectTransform _rectTransform;
        public void Construct(IStaticDataService staticDataService)
        {
            _animationData = staticDataService.GetGameData().fingerViewAnimationData;

            _rectTransform = (RectTransform)transform;

            _fingerImage = GetComponent<Image>();
            _fingerImage.enabled = false;

            _baseScale = transform.localScale;
            if (_baseScale.x == 0 || _baseScale.y == 0)
                _baseScale = new Vector3(1f, 1f, 1f);
        }
        public void ResetView()
        {
            StopAllCoroutines();
            _fingerImage.enabled = false;
            _animationParts.Clear();
        }
        public void OfferingClick(RectTransform target)
        {
            _fingerImage.enabled = true;

            ((RectTransform)_fingerImage.transform).position = target.position;

            _animationParts.Add(DownFinger);
            _animationParts.Add(UpFinger);

            DownFinger();
        }
        public void OfferingDrag(RectTransform from, RectTransform to)
        {
            _fingerImage.enabled = true;

            _rectTransform.position = from.position;

            _dragFrom = from;
            _dragTo = to;

            _animationParts.Add(DownFinger);
            _animationParts.Add(StartDrag);
            _animationParts.Add(UpFinger);
            _animationParts.Add(ResetPositionOnDrag);

            DownFinger();
        }
        private void DownFinger() => 
            StartCoroutine(DownFingerCoroutine());
        private IEnumerator DownFingerCoroutine()
        {
            Vector3 aimScale = _baseScale * _animationData.onDownScaleCoefficient;
            while (!transform.localScale.Compare(aimScale))
            {
                transform.localScale = Vector3.MoveTowards(transform.localScale, aimScale, Time.unscaledDeltaTime * _animationData.scaleSpeed);
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForSeconds(_animationData.delayBetweenAnimationParts);
            NextPart();
        }
        private void UpFinger() => 
            StartCoroutine(UpFingerCoroutine());
        private IEnumerator UpFingerCoroutine()
        {
            Vector3 aimScale = _baseScale;
            while (!transform.localScale.Compare(aimScale))
            {
                transform.localScale = Vector3.MoveTowards(transform.localScale, aimScale, Time.unscaledDeltaTime * _animationData.scaleSpeed);
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForSeconds(_animationData.delayBetweenAnimationParts);
            NextPart();
        }
        private void StartDrag() =>
            StartCoroutine(DragCoroutine());
        private IEnumerator DragCoroutine()
        {
            while (!_rectTransform.position.Compare(_dragTo.position))
            {
                _rectTransform.position = Vector3.MoveTowards(_rectTransform.position, _dragTo.position, Time.unscaledDeltaTime * _animationData.dragSpeed);
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForSeconds(_animationData.delayBetweenAnimationParts);
            NextPart();
        }
        private void ResetPositionOnDrag()
        {
            _rectTransform.position = _dragFrom.position;
            NextPart();
        }
        private void NextPart()
        {
            if (_animationParts.Count > 0)
            {
                _currentNumberPart++;
                if (_currentNumberPart >= _animationParts.Count)
                {
                    _currentNumberPart = 0;
                }
                _animationParts[_currentNumberPart]?.Invoke();
            }
        }
    }
}