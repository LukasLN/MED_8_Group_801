using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{

    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
<<<<<<< Updated upstream
=======
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
>>>>>>> Stashed changes
        transform.LookAt(player.transform);
    }
}
