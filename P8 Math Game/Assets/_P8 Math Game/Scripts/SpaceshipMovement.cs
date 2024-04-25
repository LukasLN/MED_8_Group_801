using TMPro;
using UnityEngine;
using static OVRPlugin;
using static UnityEngine.GraphicsBuffer;

namespace AstroMath
{
    public class SpaceshipMovement : MonoBehaviour
    {
        [SerializeField] GameObject interactableSpaceshipGO;
        
        [SerializeField] float timeToMove;
        float moveSpeed;

        bool isMoving;
        

        void Update()
        {
            if(isMoving == true)
            {
                MoveToTarget();
            }
        }

        void MoveToTarget()
        {
            
        }

        public void StartMovement(GameObject target) //Activated when confirm button is pressed
        {
            var targetPosition = target.transform.position;
            var distanceToTravel = Vector3.Distance(transform.position, targetPosition);
            moveSpeed = distanceToTravel / timeToMove;

            //transform.position = Vector3.MoveTowards(interactableSpaceshipGO.transform.position, target.transform.position, step); //moves spaceship
            if (Vector3.Distance(interactableSpaceshipGO.transform.position, target.transform.position) < 0.5f) //destroys ship if it reaches destination.
            {
                //Destroy(interactableSpaceshipGO);
            }
            
            isMoving = true;
        }        
    }
}