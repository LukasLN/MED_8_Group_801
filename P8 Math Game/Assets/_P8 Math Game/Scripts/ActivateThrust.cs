using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AstroMath
{
    public class ActivateThrust : MonoBehaviour
    {
        [SerializeField] GameObject SpaceShipGraphics;
        bool IsShipMoving;
        // Start is called before the first frame update

        void Start()
        {

        }
        void Update() 
        {

        }
        void StartThrust()
        {
            SpaceShipGraphics.SetActive(true);
        }
      
    }
}
