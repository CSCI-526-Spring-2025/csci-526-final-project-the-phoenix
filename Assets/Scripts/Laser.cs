using UnityEngine;

public class Laser : MonoBehaviour
{
    public GameObject laserObject;
    public float swingAngle = 10f;
    public float swingSpeed = 1f;

    public float showInterval = 7f;
    public float visibleDuration = 3f;

    private bool isVisible = true;

    void Start()
    {   
        InvokeRepeating("ToggleLaser", showInterval, showInterval);
    }

    void Update()
    {
        float angle = Mathf.Sin(Time.time * swingSpeed) * swingAngle;
        transform.localRotation = Quaternion.Euler(180f, 0f, angle);
    }

    void ToggleLaser()
    {
        StartCoroutine(ShowLaserTemporarily());
    }

    System.Collections.IEnumerator ShowLaserTemporarily()
    {
        laserObject.SetActive(true);
        isVisible = true;
        yield return new WaitForSeconds(visibleDuration);
        laserObject.SetActive(false);
        isVisible = false;
    }
}