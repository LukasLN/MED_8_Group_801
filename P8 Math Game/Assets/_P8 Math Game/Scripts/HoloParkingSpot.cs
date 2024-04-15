using UnityEngine;

namespace AstroMath
{
    public class HoloParkingSpot : MonoBehaviour
    {
        public MathProblem mathProblem;
        public void SetMathProblem(MathProblem newMathProblem)
        {
            mathProblem = newMathProblem;
        }
    }
}