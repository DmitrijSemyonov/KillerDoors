using KillerDoors.Services.MonoBehaviourFunctions;
using UnityEngine;

namespace KillerDoors.Services.TimeSpace
{
    public class TimeService : ITimeService
    {
        public float CachedTimeScale { get; private set; } = 1f;

        private IMonoBehaviourService _monoBehaviourService;

        private float _cachedOnFocusTimeScale = 1f;

        public TimeService(IMonoBehaviourService monoBehaviourService)
        {
            _monoBehaviourService = monoBehaviourService;
            _monoBehaviourService.ApplicationFocus += OnApplicationFocus;
        }

        public void SetTimeScale(float newValue)
        {
            if (CachedTimeScale != 1) return;

            CachedTimeScale = newValue;
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
            Time.timeScale = CachedTimeScale;
        }
        private void OnApplicationFocus(bool focus)
        {
            if (focus)
                Time.timeScale = _cachedOnFocusTimeScale;
            else
            {
                _cachedOnFocusTimeScale = Time.timeScale;
                Time.timeScale = 0f;
            }
        }
    }
}