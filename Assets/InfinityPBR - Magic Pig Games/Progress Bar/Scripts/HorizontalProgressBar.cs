// using System;
// namespace MagicPigGames
// {
//     [Serializable]
//     public class HorizontalProgressBar : ProgressBar
//     {
//         /*
//          * Note: The default ProgressBar class is actually a horizontal progress bar. I'm including this as
//          * a separate class to make it more clear that this is the "Horizontal" one, since there will be other
//          * ones for Vertical etc.
//          *
//          * Perhaps in the future there will be additional updates to Progress Bar as well,
//          * though right now, it really is just a horizontal progress bar.
//          */
//     }
// }


using System;
using UnityEngine;

namespace MagicPigGames
{
    [Serializable]
    public class HorizontalProgressBar : ProgressBar
    {
        [Header("Timer Settings")]
        [Tooltip("If true, the bar will auto‐start its countdown on Enable")]
        public bool autoStart = true;
        [Tooltip("Total seconds for the countdown")]
        public float duration = 20f;

        // runtime
        private float _timeRemaining;
        private bool  _running;

        void OnEnable()
        {
            if (autoStart)
                StartTimer(duration);
        }

        void Update()
        {
            if (!_running) return;

            _timeRemaining -= Time.deltaTime;
            float t = Mathf.Clamp01(_timeRemaining / duration);
            SetProgress(t);

            if (_timeRemaining <= 0f)
                _running = false;
        }

        /// <summary>
        /// (Re)start the countdown for the given seconds.
        /// </summary>
        public void StartTimer(float seconds)
        {
            duration       = seconds;
            _timeRemaining = seconds;
            _running       = true;
            SetProgress(1f);   // fill at start
        }
    }
}
