using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public float lifetime = 20f;
    private float countdown;
    public Text timerText;

    void OnEnable()
    {
        countdown = lifetime;
        UpdateTimerText();
    }

    void Update()
    {
        countdown -= Time.deltaTime;

        if (countdown > 0f)
        {
            UpdateTimerText();
        }
        else
        {
            timerText.text = "0";
            gameObject.SetActive(false);
        }
    }

    void UpdateTimerText()
    {
        timerText.text = "Clone decay: " + Mathf.CeilToInt(countdown).ToString();
    }
}