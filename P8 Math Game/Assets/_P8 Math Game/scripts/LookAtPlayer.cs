using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    public GameObject player;

    void Update()
    {
        // Look at the player's position
        transform.LookAt(player.transform);

        // Rotate the object 180 degrees around the y-axis
        transform.Rotate(0, 180, 0);
    }
}