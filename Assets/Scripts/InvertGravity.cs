using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvertGravity : MonoBehaviour
{
    // Reference to the player and clone scripts
    public Player player;
    public Clone clone;
    
    // Start is called before the first frame update
    void Start()

    {
        if (clone != null)
        {
            clone.GetComponent<Rigidbody2D>().gravityScale = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Press space to invert gravity
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Physics2D.gravity *= -1;
            player.invertGravity();
            clone.invertGravity();
        }
    }
}
