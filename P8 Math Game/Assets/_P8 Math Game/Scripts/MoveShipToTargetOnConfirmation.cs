using UnityEngine;

namespace AstroMath
{
    public class MoveShipToTargetOnConfirmation : MonoBehaviour
    {
        [SerializeField] GameObject Spaceship;
        GameObject Target;
        bool ismoving = false;
        int speed = 2;

        void Update()
        {
            if(ismoving == true)
            {
                MoveToTargetPos();
            }
        }

        public void StartMovement() //Activated when confirm button is pressed
        {
            Target = Spaceship.transform.GetComponent<HoloSpaceship>().targetGO; //gets target eg. landingplatform    
            ismoving = true;
        }

        void MoveToTargetPos()
        {
            var step =  speed * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(Spaceship.transform.position, Target.transform.position, step); //moves spaceship
            if(Vector3.Distance(Spaceship.transform.position, Target.transform.position) < 0.5f) //destroys ship if it reaches destination.
            {
                Destroy(Spaceship);
            }
        }
    }
}