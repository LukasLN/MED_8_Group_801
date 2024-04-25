using System.ComponentModel.Design;
using UnityEngine;

namespace AstroMath
{
    public class HoloParkingSpot : MonoBehaviour
    {
        public MathProblem mathProblem;
        public bool highlightIsActive;
        //public GameObject highlightGO; //GO = GameObject
        //public int problemID;
        [SerializeField] GameObject Graphics;
        [SerializeField] Material DefaultMaterial;
        [SerializeField] Material HighlightMaterial;

        public void SetMathProblem(MathProblem newMathProblem)
        {
            mathProblem = newMathProblem;
        }

        public void SetHighlightActivation(bool activation)
        {
            highlightIsActive = activation;
            //highlightGO.SetActive(highlightIsActive);
            ChangeMaterial();
        }

        void ChangeMaterial()
        {
            if (highlightIsActive == true )
            {
                Graphics.transform.GetComponent<Renderer>().material = HighlightMaterial;
            }
            else
            {
                Graphics.transform.GetComponent<Renderer>().material = HighlightMaterial;
            }

        }

        //public void SetProblemID(int newProblemID)
        //{
        //    problemID = newProblemID;
        //}
    }
}