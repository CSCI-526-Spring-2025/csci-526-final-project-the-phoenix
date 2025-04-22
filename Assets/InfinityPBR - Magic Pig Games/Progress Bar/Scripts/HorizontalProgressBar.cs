using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
namespace MagicPigGames
{
    [Serializable]
    public class HorizontalProgressBar : ProgressBar
    {
        /*
         * Note: The default ProgressBar class is actually a horizontal progress bar. I'm including this as
         * a separate class to make it more clear that this is the "Horizontal" one, since there will be other
         * ones for Vertical etc.
         *
         * Perhaps in the future there will be additional updates to Progress Bar as well,
         * though right now, it really is just a horizontal progress bar.
         */
         public float lifetime = 20f;
    private float countdown;
    public Text timerText;

    private SpriteRenderer spriteRenderer;
    private Coroutine blinkCoroutine;
    public float blinkInterval = 0.6f;

    void OnEnable()
    {
        countdown = lifetime;
        UpdateTimerText();

        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
            spriteRenderer.enabled = true;

        if (blinkCoroutine != null)
            StopCoroutine(blinkCoroutine);
        blinkCoroutine = null;
    }

    void Update()
    {
        countdown -= Time.deltaTime;

        if (countdown <= 10f && blinkCoroutine == null && spriteRenderer != null)
        {
            blinkCoroutine = StartCoroutine(BlinkClone());
        }

        if (countdown > 0f)
        {
            UpdateTimerText();
        }
        else
        {
            timerText.text = "0";
       
            if (blinkCoroutine != null)
                StopCoroutine(blinkCoroutine);

            if (spriteRenderer != null)
                spriteRenderer.enabled = true;

            gameObject.SetActive(false);
        }
    }

    public void ResetTimer()
    {
        countdown = lifetime;
        UpdateTimerText();

        if (spriteRenderer != null)
            spriteRenderer.enabled = true;

        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
            blinkCoroutine = null;
        }
    }

    void UpdateTimerText()
    {
        timerText.text = "Clone decay: " + Mathf.CeilToInt(countdown).ToString();
    }

    IEnumerator BlinkClone()
    {
        while (true)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(blinkInterval);
        }
    }
    }

}
