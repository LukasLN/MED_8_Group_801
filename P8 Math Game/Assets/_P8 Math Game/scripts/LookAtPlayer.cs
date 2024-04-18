using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    public GameObject player;

    void Update()
    {
        //################################################
        //
        // > I think we should change this so that the
        //   panel only looks at the player, when the
        //   player has selected the spaceship.
        //
        // - Lau
        //
        //################################################

        // Look at the player's position
        transform.LookAt(player.transform);

        // Rotate the object 180 degrees around the y-axis
        transform.Rotate(0, 180, 0);
    }
}