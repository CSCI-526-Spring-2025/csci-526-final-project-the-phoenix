using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpDisable : MonoBehaviour
{
    // Reference to the player and clone scripts
    public Player player;
    public Clone clone;

    // Start is called before the first frame update
    void Start()
    {
        player.disableJump();
        clone.disableJump();
    }

    // Update is called once per frame
    void Update()
    {
        player.disableJump();
        clone.disableJump();
    }
}
