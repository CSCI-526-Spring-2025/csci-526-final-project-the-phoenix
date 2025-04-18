using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
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
            LevelManager.Instance.TrackCloneUsage();
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
