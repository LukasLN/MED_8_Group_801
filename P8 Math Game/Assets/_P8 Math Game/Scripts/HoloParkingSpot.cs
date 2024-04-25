using UnityEngine;

namespace AstroMath
{
    public class HoloParkingSpot : MonoBehaviour
    {
        public MathProblem mathProblem;
        public bool highlightIsActive;
        public GameObject highlightGO; //GO = GameObject
        //public int problemID;

        public void SetMathProblem(MathProblem newMathProblem)
        {
            mathProblem = newMathProblem;
        }

        public void SetHighlightActivation(bool activation)
        {
            highlightIsActive = activation;
            highlightGO.SetActive(highlightIsActive);
        }

        //public void SetProblemID(int newProblemID)
        //{
        //    problemID = newProblemID;
        //}
    }
}