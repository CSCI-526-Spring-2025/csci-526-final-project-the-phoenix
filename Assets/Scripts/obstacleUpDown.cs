using UnityEngine;

public class BobbingObstacle : MonoBehaviour
{
    public float bobbingAmount = 0.8f;    // How much it moves up and down
    public float bobbingSpeed = 2.5f;        // How fast it bobs
    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        float newY = startPosition.y + Mathf.Sin(Time.time * bobbingSpeed) * bobbingAmount;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }
}
