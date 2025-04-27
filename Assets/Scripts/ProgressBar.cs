using UnityEngine;
using System.Collections;

public class ProgressBar : MonoBehaviour
{
    [Header("Timer Settings")]
    public float totalDuration = 20f;
    public float dangerTime = 10f;
    public float blinkInterval = 0.3f;
    public Color normalColor = Color.green;
    public Color dangerColor = Color.red;

    private float countdown;
    private bool isRunning = false;
    private Coroutine blinkCoroutine;

    private SpriteRenderer cloneRenderer;    
    private SpriteRenderer barRenderer;     
    private Vector3 barOriginalScale;
    private Color barOriginalColor;

    void Awake()
    {
        cloneRenderer = GetComponent<SpriteRenderer>();
        barRenderer = transform.Find("Square")?.GetComponent<SpriteRenderer>(); // Find the child bar

        if (barRenderer != null)
        {
            barOriginalScale = barRenderer.transform.localScale;
            barOriginalColor = normalColor;
            barRenderer.color = normalColor;
        }
    }

    void OnEnable()
    {
        ResetTimer();
    }

    void Update()
    {
        if (!isRunning) return;

        countdown -= Time.deltaTime;

        if (barRenderer != null)
        {
            float progress = Mathf.Clamp01(countdown / totalDuration);
            barRenderer.transform.localScale = new Vector3(barOriginalScale.x * progress, barOriginalScale.y, barOriginalScale.z);

            if (countdown <= dangerTime && blinkCoroutine == null)
            {
                barRenderer.color = dangerColor;
                blinkCoroutine = StartCoroutine(BlinkBoth());
            }
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

            gameObject.SetActive(false);  // Deactivate clone
        }
    }

    public void ResetTimer()
    {
        countdown = totalDuration;
        isRunning = true;

        if (barRenderer != null)
        {
            barRenderer.transform.localScale = barOriginalScale;
            barRenderer.color = normalColor;
            barRenderer.enabled = true;
        }
        if (cloneRenderer != null)
            cloneRenderer.enabled = true;

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