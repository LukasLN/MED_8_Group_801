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
    }
}