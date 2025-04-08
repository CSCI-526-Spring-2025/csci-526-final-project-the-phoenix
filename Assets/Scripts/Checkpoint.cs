using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private Clone cloneScript;
    [SerializeField] private Player playerScript;
    [SerializeField] private GameObject prevPlatform_clone;
    [SerializeField] private GameObject nextPlatform_clone;
    [SerializeField] private GameObject prevPlatform_player;
    [SerializeField] private GameObject nextPlatform_player;

    // Start is called before the first frame update
    void Start()
    {
        nextPlatform_clone.SetActive(false);
        if (nextPlatform_player != null)
        {
            // nextPlatform_player.SetActive(false);
        }
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
            cloneScript.changeInitialPosition(nextPlatform_clone.transform.position + new Vector3(0, 0.5f, 0));

            nextPlatform_clone.SetActive(true);
            prevPlatform_clone.SetActive(false);
            if (nextPlatform_player != null)
            {
                nextPlatform_player.SetActive(true);
                prevPlatform_player.SetActive(false);
            }
        }

        if (collider.gameObject.CompareTag("Player"))
        {
            Debug.Log("Neter checkpint");
            // Initialize the clone's position to the next platform + a small offset vertically
            playerScript.changeInitialPosition(nextPlatform_player.transform.position + new Vector3(0, 0.5f, 0));

        }
    }
}
