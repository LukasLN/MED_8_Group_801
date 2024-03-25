using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace AstroMath
{
    public class FloatingText : MonoBehaviour
    {
        Transform mainCam;
        public Transform unit;
        public Transform worldSpaceCanvas;
        public TMP_Text coord;
        public Vector3 offset;

        // Start is called before the first frame update
        void Start()
        {
            mainCam = Camera.main.transform;
            unit = transform.parent;
            worldSpaceCanvas = GameObject.FindObjectOfType<Canvas>().transform;
            transform.SetParent(worldSpaceCanvas);

            coord.text = "("+unit.position.x+", "+unit.position.y+", " + unit.position.z+")";


        }

        // Update is called once per frame
        void Update()
        {
            transform.rotation = Quaternion.LookRotation(transform.position - mainCam.transform.position);
            transform.position = unit.position + offset;

            coord.text = "(" + unit.position.x + ", " + unit.position.y + ", " + unit.position.z + ")";

        }
    }
}
