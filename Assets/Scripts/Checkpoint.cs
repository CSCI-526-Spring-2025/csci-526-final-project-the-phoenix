using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private Clone cloneScript;
    [SerializeField] private GameObject prevPlatform;
    [SerializeField] private GameObject nextPlatform;

    // Start is called before the first frame update
    void Start()
    {
        nextPlatform.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Clone"))
        {
            // Initialize the clone's position to the next platform + a small offset vertically
            cloneScript.changeInitialPosition(nextPlatform.transform.position + new Vector3(0, 0.5f, 0));

            nextPlatform.SetActive(true);
            prevPlatform.SetActive(false);
        }
    }
}
