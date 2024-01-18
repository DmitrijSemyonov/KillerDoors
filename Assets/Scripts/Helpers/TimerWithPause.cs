using UnityEngine;

namespace Helpers
{
    public class TimerWithPause
    {
        public bool IsEvent =>
           _elapsedTime > _countdownTime;

        private float _elapsedTime;
        private float _countdownTime;
        private bool _pause;
        public void Start(float countdownTime)
        {
            _countdownTime = countdownTime;
            _elapsedTime = 0;
        }

        public void Update()
        {
            if (!_pause && _elapsedTime < _countdownTime)
                _elapsedTime += Time.deltaTime;
        }
        public void Pause() =>
            _pause = true;
        public void TurnOffPause()
            => _pause = false;
        public void Zeroing() =>
            _elapsedTime = 0f;

    }
}