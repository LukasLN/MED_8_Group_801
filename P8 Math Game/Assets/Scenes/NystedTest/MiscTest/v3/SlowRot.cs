using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowRot : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
         // Rotate the object around its local y axis at 1 degree per second
            transform.Rotate(Vector3.up * 15 * Time.deltaTime);
    }
}
