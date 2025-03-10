using UnityEngine;

public class ArrowBounce : MonoBehaviour
{
    public float bounceSpeed = 2f;  
    public float bounceAmount = 0.2f; 
    private Vector3 startPos;
    public bool isVertical = true; 
    void Start()
    {
        startPos = transform.position; 
    }

    void Update()
    {
        float movement = Mathf.Sin(Time.time * bounceSpeed) * bounceAmount;
        if (isVertical)
            transform.position = startPos + new Vector3(0, movement, 0); 
        else
            transform.position = startPos + new Vector3(movement, 0, 0);
    }
}