using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AstroMath
{
    public class SpaceshipGraphicsHandler : MonoBehaviour
    {

        [SerializeField] Transform HoloCylinder;
        [SerializeField] Transform HoloInnerCircle;
        [SerializeField] Transform HoloOuterCircle;

        public float HoloCylinderHeight = 0;
        public float HoloCircleHeight = 0;

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            HoloCylinder.position = new Vector3(this.transform.position.x, this.transform.position.y + HoloCylinderHeight, this.transform.position.z);

            HoloInnerCircle.position = new Vector3(this.transform.position.x, 0 + HoloCircleHeight, this.transform.position.z);
            HoloOuterCircle.position = new Vector3(this.transform.position.x, 0 + HoloCircleHeight, this.transform.position.z);
        }
    }
}
