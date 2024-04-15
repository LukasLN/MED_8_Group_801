using System.Collections.Generic;
using UnityEngine;

namespace AstroMath
{
    public class MathProblemManager : MonoBehaviour
    {
        public static MathProblemManager instance;

        public float minDistance, maxDistance;
        [SerializeField] int numberOfProblemsToCreate;

        [SerializeField] List<MathProblem> mathProblems;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            CreateNumberOfProblems(numberOfProblemsToCreate);
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.C))
            {
                CreateNumberOfProblems(numberOfProblemsToCreate);
            }

            if(Input.GetKeyDown(KeyCode.R))
            {
                RemoveProblem(0);
            }

        }

        public void CreateNumberOfProblems(int numberOfProblems)
        {
            RemoveAllProblems();

            FixedPositionsContainer.instance.CreateNewSampleParkingPositions(5);
            FixedPositionsContainer.instance.CreateNewSampleAsteroidPositions(5);

            for (int i = 0; i < numberOfProblems; i++)
            {
                CreateProblem();
            }
        }

        void CreateProblem()
        {
            MathProblem newMathProblem = MathProblemGenerator.GenerateMathProblem(minDistance, maxDistance);
            mathProblems.Add(newMathProblem);

            HoloObjectManager.instance.SpawnHoloObjects(newMathProblem);
        }

        void RemoveAllProblems()
        {
            var count = mathProblems.Count;

            for (int i = 0; i < count; i++)
            {
                RemoveProblem(0);
            }
        }

        void RemoveProblem(int index)
        {
            HoloObjectManager.instance.DespawnHoloObjects(index);
            mathProblems.RemoveAt(index);
        }
    }
}