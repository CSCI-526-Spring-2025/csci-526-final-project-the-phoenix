
using UnityEngine;

public class Bounce : MonoBehaviour
{
    public float bounceSpeed = 2f;               
    public float bounceAmountFactor = 0.5f;    
    private Vector3 startLocalPos;
    public bool isVertical = true;

    private float portalX = 0.0729f;
    private float portalY = 0.0650f;

    void Start()
    {
        startLocalPos = transform.localPosition;
    }

    void Update()
    {
        float movement = Mathf.Sin(Time.time * bounceSpeed);

        float maxOffset = isVertical
            ? portalY * bounceAmountFactor
            : portalX * bounceAmountFactor;

        float offsetAmount = movement * maxOffset;

        Vector3 offset = isVertical
            ? new Vector3(0f, offsetAmount, 0f)
            : new Vector3(offsetAmount, 0f, 0f);

        transform.localPosition = startLocalPos + offset;
    }
}
