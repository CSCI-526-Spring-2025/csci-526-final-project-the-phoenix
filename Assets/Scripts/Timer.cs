using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public float lifetime = 20f;
    private float countdown;
    private Text timerText;

    void OnEnable()
    {
        countdown = lifetime;
        timerText = GetComponentInChildren<Text>();
    }

    void Update()
    {
        countdown -= Time.deltaTime;

        if (timerText != null)
        {
            timerText.text = Mathf.CeilToInt(countdown).ToString();
        }

        if (countdown <= 0f)
        {
            Destroy(gameObject);
        }
    }
}