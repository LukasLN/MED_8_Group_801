using UnityEngine;

namespace AstroMath
{
    public class HoloParkingSpot : MonoBehaviour
    {
        public MathProblem mathProblem;
        public int problemID;

        public void SetMathProblem(MathProblem newMathProblem)
        {
            mathProblem = newMathProblem;
        }

        public void SetProblemID(int newProblemID)
        {
            problemID = newProblemID;
        }
    }
}