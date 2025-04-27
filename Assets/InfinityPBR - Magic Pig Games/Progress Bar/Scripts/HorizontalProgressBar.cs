using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace MagicPigGames
{
    [Serializable]
    public class HorizontalProgressBar : ProgressBar
    {
        [Header("Timer Settings")]
        public bool autoStart = true;
        public float duration = 20f;

        [Header("Color and Blinking Settings")]
        public Color normalColor = Color.green;
        public Color dangerColor = Color.red;
        public float dangerTime = 10f;
        public float blinkInterval = 0.5f;

        private float _timeRemaining;
        private bool _running;
        private Coroutine blinkCoroutine;
        private bool blinkingStarted = false;

        public GameObject fillImageObject;
        private RawImage fillImage;

        void Start()
        {
            fillImage = fillImageObject.GetComponent<RawImage>();
        }

        void OnEnable()
        {
            if (autoStart)
                StartTimer(duration);

            if (fillImage != null)
                fillImage.color = normalColor;

            blinkingStarted = false;
        }

        void Update()
        {
            if (!_running) return;

            _timeRemaining -= Time.deltaTime;
            float t = Mathf.Clamp01(_timeRemaining / duration);
            SetProgress(t);

            if (_timeRemaining <= dangerTime && !blinkingStarted)
            {
                if (fillImage != null)
                    fillImage.color = dangerColor;

                blinkCoroutine = StartCoroutine(Blink());
                blinkingStarted = true;
            }

            if (_timeRemaining <= 0f && blinkingStarted)
            {
                if (blinkCoroutine != null)
                    StopCoroutine(blinkCoroutine);

                if (fillImage != null)
                    fillImage.enabled = true;

                blinkingStarted = false;
                _running = false;
                gameObject.SetActive(false);

                // Add your clone decay logic if needed
                LevelManager.Instance.TrackCloneUsage();
            }
        
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

        IEnumerator Blink()
        {
            while (true)
            {
                if (fillImage != null)
                    fillImage.enabled = !fillImage.enabled;

                yield return new WaitForSeconds(blinkInterval);
            }
        }
    }
}
