using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AstroMath
{
    public class MoveShipToTargetOnConfirmation : MonoBehaviour
    {
        [SerializeField] GameObject Spaceship;
        GameObject Target;
        bool ismoving = false;
        int speed = 2;

        // Start is called before the first frame update
        void Start()
        {
        Target = Spaceship.transform.GetComponent<HoloSpaceship>().targetGO; //gets target eg. landingplatform    
        }

        // Update is called once per frame
        void Update()
        {
            if(ismoving == true)
            {
                MoveToTargetPos();
            }
        }

        public void StartMovement() //Activated when confirm button is pressed
        {
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
