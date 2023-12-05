using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FingerAnimation : MonoBehaviour
{
    private Image _fingerImage;

    [SerializeField] private float _dragSpeed = 120f;
    [SerializeField] private float _scaleSpeed = 2f;
    [SerializeField] private float _onDownScaleCoefficient = 0.8f;
    [SerializeField] private float _delayBetweenAnimationParts = 0.5f;
    private Vector3 _baseScale;

    private List<Action> _animationParts = new List<Action>();
    private int _currentNumberPart;

    private RectTransform _dragFrom;
    private RectTransform _dragTo;

    private RectTransform _rectTransform;

    void Start()
    {
        _rectTransform = (RectTransform)transform;
        _fingerImage = GetComponent<Image>();
        _fingerImage.enabled = false;
        _baseScale = transform.localScale;
        if (_baseScale.x == 0 || _baseScale.y == 0)
            _baseScale = new Vector3(1f, 1f, 1f);
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
    private void DownFinger()
    {
        StartCoroutine(DownFingerCoroutine());
    }
    private IEnumerator DownFingerCoroutine()
    {
        Vector3 aimScale = _baseScale * _onDownScaleCoefficient;
        while (!transform.localScale.Compare(aimScale)) 
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale, aimScale, Time.unscaledDeltaTime * _scaleSpeed);
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(_delayBetweenAnimationParts);
        NextPart();
    }
    private void UpFinger()
    {
        StartCoroutine(UpFingerCoroutine());
    }
    private IEnumerator UpFingerCoroutine()
    {
        Vector3 aimScale = _baseScale;
        while (!transform.localScale.Compare(aimScale)) 
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale, aimScale, Time.unscaledDeltaTime * _scaleSpeed);
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(_delayBetweenAnimationParts);
        NextPart();
    }
    private void StartDrag()
    {
        StartCoroutine(DragCoroutine());
    }
    private IEnumerator DragCoroutine()
    {
        while (!_rectTransform.position.Compare(_dragTo.position))
        {
            _rectTransform.position = Vector3.MoveTowards(_rectTransform.position, _dragTo.position, Time.unscaledDeltaTime * _dragSpeed);
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(_delayBetweenAnimationParts);
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
            if(_currentNumberPart >= _animationParts.Count)
            {
                _currentNumberPart = 0;
            }
            _animationParts[_currentNumberPart]?.Invoke();
        }
    }
    public void ResetAnimation()
    {
        StopAllCoroutines();
        _fingerImage.enabled = false;
        _animationParts.Clear();
    }
}
