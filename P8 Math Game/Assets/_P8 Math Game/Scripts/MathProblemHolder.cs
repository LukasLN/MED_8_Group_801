using System.Collections.Generic;
using UnityEngine;

namespace AstroMath
{
    public class MathProblemHolder : MonoBehaviour
    {
        public static MathProblemHolder instance;
        [SerializeField] public List<MathProblem> mathProblems;

        private void Awake()
        {
            instance = this;
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Alpha1))
            {
                MathProblem newMathProblem = MathProblemGenerator.GenerateMathProblem(false, 0);
                mathProblems.Add(newMathProblem);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                MathProblem newMathProblem = MathProblemGenerator.GenerateMathProblem(false, 1);
                mathProblems.Add(newMathProblem);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                MathProblem newMathProblem = MathProblemGenerator.GenerateMathProblem(false, 2);
                mathProblems.Add(newMathProblem);
            }
        }
    }
}