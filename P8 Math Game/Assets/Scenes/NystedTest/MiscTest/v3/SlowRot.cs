using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AstroMath
{ 
public class SlowRot : MonoBehaviour
{
    public bool isOuter = false;

    // Update is called once per frame
    void Update()
    {
        if (isOuter == true)
        {
            // Rotate the object around its local y axis at 1 degree per second
            transform.Rotate(Vector3.up * -15 * Time.deltaTime);
        }
        else
        {
            // Rotate the object around its local y axis at 1 degree per second in other direction
            transform.Rotate(Vector3.up * 15 * Time.deltaTime);
        }
           
    }
}
}
