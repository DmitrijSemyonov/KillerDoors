using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour { 
    public float CachedTimeScale { get; private set; } = 1f;

    private const float _timeScaleOnKill = 0.3f;
    private float _cachedOnFocusTimeScale = 1f;
    [SerializeField] private bool _isSlowdownTimeOnPersonKill;
    [SerializeField] private float _timeOfSlowdownTime = 0.3f;

    private Coroutine _slowdownTimeCoroutine;
    private List<Door> _doors;

    private GameModeManager _gameModeManager;
    public event Action FocusPause;
    public event Action FocusResume;
    void Start()
    {
        if (_isSlowdownTimeOnPersonKill)
        {
            _doors = new List<Door>(FindObjectsOfType<Door>());
            for (int i = 0; i < _doors.Count; i++)
            {
                _doors[i].PersonKilled += SlowDownTime;
            }
        }
        _gameModeManager = FindObjectOfType<GameModeManager>();
        _gameModeManager.EndDoorGame += ResetTimeScale;
    }
    public void DecreaseTimeScale()
    {
        if (CachedTimeScale != 1) return;

        CachedTimeScale -= 0.3f;
        Time.timeScale = CachedTimeScale;
    }
    public void ResetTimeScale()
    {
        CachedTimeScale = 1f;

        if (Time.timeScale != 0) 
            Time.timeScale = CachedTimeScale;
    }
    public void Pause()
    {
        CachedTimeScale = Time.timeScale;
        Time.timeScale = 0;
    }
    public void Resume()
    {
        Debug.Assert(CachedTimeScale != 0, "CachedTimeScale = 0 on Resume.");
        Time.timeScale = CachedTimeScale;
    }
    private void SlowDownTime(Person _)
    {
        if (_slowdownTimeCoroutine != null) StopCoroutine(_slowdownTimeCoroutine);

        _slowdownTimeCoroutine = StartCoroutine(SlowDownTimeCoroutine());
    }
    private IEnumerator SlowDownTimeCoroutine()
    {
        Time.timeScale = _timeScaleOnKill;
        yield return new WaitForSecondsRealtime(_timeOfSlowdownTime);
        
        if(Time.timeScale != 0)
            Time.timeScale = CachedTimeScale;
    }
    private void OnDestroy()
    {
        if (_doors != null)
        {
            for (int i = 0; i < _doors.Count; i++)
            {
                if (!_doors[i]) continue;
                _doors[i].PersonKilled -= SlowDownTime;
            }
        }
        if(_gameModeManager)
            _gameModeManager.EndDoorGame -= ResetTimeScale;
    }
    //window.onblur .onfocus WebGL events
    public void ApplicationFocusPause()
    {
        _cachedOnFocusTimeScale = Time.timeScale;
        Time.timeScale = 0f;
        FocusPause?.Invoke();
    }
    public void ApplicationFocusResume()
    {
        Time.timeScale = _cachedOnFocusTimeScale;
        FocusResume?.Invoke();
        Debug.Assert(CachedTimeScale != 0, "CachedTimeScale = 0 on AppFocusResume.");
    }
}