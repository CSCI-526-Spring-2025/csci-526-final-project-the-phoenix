using UnityEngine;
using System.Collections;

public class SpriteProgressBar : MonoBehaviour
{
    public float totalDuration = 20f;
    private float countdown;
    private Vector3 originalScale;
    private bool isRunning = false;

    private SpriteRenderer barRenderer;
    private SpriteRenderer cloneRenderer;
    private Color originalColor;
    public Color warningColor = Color.red;
    public float blinkInterval = 0.3f;
    private Coroutine blinkCoroutine;

    void OnEnable()
    {
        countdown = totalDuration;
        originalScale = transform.localScale;
        isRunning = true;

        barRenderer = GetComponent<SpriteRenderer>();
        cloneRenderer = transform.parent.GetComponent<SpriteRenderer>();

        if (barRenderer != null)
        {
            originalColor = barRenderer.color;
            barRenderer.color = originalColor;
        }

        if (blinkCoroutine != null)
            StopCoroutine(blinkCoroutine);
        blinkCoroutine = null;
    }

    void Update()
    {
        if (!isRunning) return;

        countdown -= Time.deltaTime;
        float progress = Mathf.Clamp01(countdown / totalDuration);
        transform.localScale = new Vector3(originalScale.x * progress, originalScale.y, originalScale.z);

        if (countdown <= 10f && blinkCoroutine == null && barRenderer != null)
        {
            barRenderer.color = warningColor;

            blinkCoroutine = StartCoroutine(BlinkBoth());
        }

        if (countdown <= 0f)
        {
            isRunning = false;

            if (blinkCoroutine != null)
                StopCoroutine(blinkCoroutine);

            if (barRenderer != null)
                barRenderer.enabled = true;
            if (cloneRenderer != null)
                cloneRenderer.enabled = true;


            transform.parent.gameObject.SetActive(false);
        }
    }

    public void ResetTimer()
    {
        transform.parent.gameObject.SetActive(true);

        countdown = totalDuration;
        transform.localScale = originalScale;
        isRunning = true;

        if (barRenderer != null)
        {
            barRenderer.color = originalColor;
            barRenderer.enabled = true;
        }
        if (cloneRenderer != null)
        {
            cloneRenderer.color = Color.white;
            cloneRenderer.enabled = true;
        }

        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
            blinkCoroutine = null;
        }
    }

    IEnumerator BlinkBoth()
    {
        while (true)
        {
            if (barRenderer != null)
                barRenderer.enabled = !barRenderer.enabled;
            if (cloneRenderer != null)
                cloneRenderer.enabled = !cloneRenderer.enabled;
            yield return new WaitForSeconds(blinkInterval);
        }
    }
}