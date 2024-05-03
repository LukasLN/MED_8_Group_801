using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using TMPro;

namespace AstroMath
{
    public class ChoosePointerShape : MonoBehaviour
    {
        [SerializeField] InteractableSpaceship Spaceship;
        [SerializeField] int Step;
        [SerializeField] int[] ShapeArray = new int[3];
        [SerializeField] List<int> tempList = new List<int>();
        [SerializeField] TMP_Text pointerShapeText;
        [SerializeField] float timeBeforeHideText;

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
            pointerShapeText.gameObject.SetActive(true);
            pointerShapeText.text = Spaceship.pointerShape.ToString();
            StartCoroutine(WaitBeforeHideText());

        }

        IEnumerator WaitBeforeHideText()
        {
            yield return new WaitForSeconds(timeBeforeHideText);
            pointerShapeText.gameObject.SetActive(false);
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
