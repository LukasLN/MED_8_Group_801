using System.Collections.Generic;
using UnityEngine;

namespace AstroMath
{
    public class ChoosePointerShape : MonoBehaviour
    {
        [SerializeField] InteractableSpaceship Spaceship;
        [SerializeField] int Step;
        [SerializeField] int[] ShapeArray = new int[3];
        [SerializeField] List<int> tempList = new List<int>();

        void Start()
        {
            tempList.Add(0);
            tempList.Add(1);
            tempList.Add(2);

            for (int i = 0; i < ShapeArray.Length; i++)
            {
               int spotInList = Random.Range(0, tempList.Count);
               ShapeArray[i] = tempList[spotInList];
               tempList.RemoveAt(spotInList);
            }

            UpdatePointerShape();
        }

        public void UpdatePointerShape()
        {
            Spaceship.pointerShape = (InteractableSpaceship.PointerShape)ShapeArray[Step];
        }

        public void IncreaseStep(int increaseAmount)
        {
            Step += increaseAmount;
            if (Step >= ShapeArray.Length)
            {
                Application.Quit();
                return;
            }

            UpdatePointerShape();
        }
    }
}
