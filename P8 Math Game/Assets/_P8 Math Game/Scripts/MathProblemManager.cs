using System.Collections.Generic;
using UnityEngine;
using static AstroMath.MathProblemManager;

namespace AstroMath
{
    public class MathProblemManager : MonoBehaviour
    {
        public static MathProblemManager instance;

        public enum MathProblemType
        {
            Direction = 0,
            Collision = 1,
            Scale = 2,
            Random = 3
        }
        public MathProblemType mathProblemType;
        [SerializeField] bool spawnAtStart;

        public float minDistance, maxDistance;
        [SerializeField] int numberOfProblemsToCreate;

        [SerializeField] List<MathProblem> mathProblems;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            if(spawnAtStart == true)
            {
                CreateNumberOfProblems(numberOfProblemsToCreate);
            }
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

            FixedPositionsContainer.instance.CreateNewSampleParkingPositions(numberOfProblems);
            FixedPositionsContainer.instance.CreateNewSampleAsteroidPositions(numberOfProblems);

            for (int i = 0; i < numberOfProblems; i++)
            {
                CreateProblem();
            }
        }

        void CreateProblem()
        {
            MathProblem newMathProblem;
            if (mathProblemType == MathProblemType.Random)
            {
                newMathProblem = MathProblemGenerator.GenerateMathProblem(minDistance, maxDistance);
            }
            else
            {
                newMathProblem = MathProblemGenerator.GenerateMathProblem(minDistance, maxDistance, false, (int)mathProblemType);
            }

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