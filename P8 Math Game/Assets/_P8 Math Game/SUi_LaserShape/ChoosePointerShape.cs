using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AstroMath
{
    public class ChoosePointerShape : MonoBehaviour
    {
       [SerializeField] GameObject Spaceship;
        int Step = -1;
        int[] ShapeArray = new int[3];
        List<int> tempList = new List<int>();

        // Start is called before the first frame update
        void Start()
        {
            tempList.Add(1);
            tempList.Add(2);
            tempList.Add(3);

            for (int i = 0; i < ShapeArray.Length; i++)
            {
               int spotInList = Random.Range(0, tempList.Count);
               ShapeArray[i] = tempList[spotInList];
               tempList.Remove(spotInList);
            }

            SelectPointerShape();


        }

        public void SelectPointerShape()
        {
            Step++;
            Spaceship.GetComponent<InteractableSpaceship>().pointerShape = (InteractableSpaceship.PointerShape)ShapeArray[Step];
        }
    }
}
