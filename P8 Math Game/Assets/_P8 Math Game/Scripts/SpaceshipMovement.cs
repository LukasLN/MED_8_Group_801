using UnityEngine;

namespace AstroMath
{
    public class SpaceshipMovement : MonoBehaviour
    {
        [SerializeField] GameObject interactableSpaceshipGO;
        
        [SerializeField] float timeToMove;
        float moveSpeed;
        Vector3 targetPosition;

        bool isMoving;
        

        void Update()
        {
            if(isMoving == true)
            {
                MoveForward();

                if (Vector3.Distance(transform.position, targetPosition) < 0.1f) //if we are not at the target position
                {
                    isMoving = false;
                    
                    interactableSpaceshipGO.GetComponent<Spaceship>().ShowResult();
                }
            }
        }

        void MoveForward()
        {
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
            //Vector3.MoveTowards(transform.position, targetPosition, 2); //moves spaceship
        }

        public void SetTarget(GameObject targetGO) //Activated when confirm button is pressed
        {
            targetPosition = targetGO.transform.position;
            
            var distanceToTravel = Vector3.Distance(transform.position, targetPosition);
            moveSpeed = distanceToTravel / timeToMove;

            //transform.position = Vector3.MoveTowards(interactableSpaceshipGO.transform.position, target.transform.position, step); //moves spaceship
            
            isMoving = true;
        }        
    }
}