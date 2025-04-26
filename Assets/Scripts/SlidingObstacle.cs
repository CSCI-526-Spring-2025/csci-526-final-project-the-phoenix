
using UnityEngine;



public class SlidingObstacle : MonoBehaviour
{
    public float slideAmount = 0.8f;     
    public float slideSpeed = 2.8f;        
    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        float newX = startPosition.x + Mathf.Sin(Time.time * slideSpeed) * slideAmount;
        transform.position = new Vector3(newX, startPosition.y, startPosition.z);
    }
}

