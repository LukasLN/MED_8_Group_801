using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace AstroMath
{
    public class MathProblemGeneratorPanel : MonoBehaviour
    {
        public static MathProblemGeneratorPanel instance;

        [SerializeField] int numberOfProblemsToCreate;
        [SerializeField] TMP_Text numberOfProblemsToCreateText;

        public List<GameObject> spaceships;
        [SerializeField] GameObject spaceshipPF; //PF = Prefab
        [SerializeField] Transform spaceshipParentTF; //TF = Transform

        [SerializeField] int minSP, maxSP, minPP, maxPP;
        [SerializeField, Range(-100, 1)] float worldToHologramScale;

        private void Awake()
        {
            instance = this;
        }

        void Start()
        {
            numberOfProblemsToCreateText.text = numberOfProblemsToCreate.ToString(); ;
        }

        public void CreateMathProblems()
        {
            ClearMathProblems();

            for (int i = 0; i < numberOfProblemsToCreate; i++)
            {
                var newMathProblem = MathProblemGenerator.GenerateMathProblem(minSP, maxSP, minPP, maxPP, false, 0);
                MathProblemHolder.instance.mathProblems.Add(newMathProblem);

                Vector3 mathProblemSpaceshipPosition = MathProblemHolder.instance.mathProblems[i].spaceshipPosition;
                Vector3 positionToSpawnSpaceshipIn = new Vector3(mathProblemSpaceshipPosition.x, mathProblemSpaceshipPosition.y, mathProblemSpaceshipPosition.z);

                var scalar = worldToHologramScale;
                if (scalar < 0) scalar *= -1;
                positionToSpawnSpaceshipIn /= scalar;

                var newSpaceship = Instantiate(spaceshipPF, positionToSpawnSpaceshipIn, Quaternion.identity, spaceshipParentTF);

                newSpaceship.GetComponent<HologramSpaceship>().mathProblem = newMathProblem;
                newSpaceship.GetComponent<HologramSpaceship>().SetPositions();

                spaceships.Add(newSpaceship);
            }
        }

        public void ClearMathProblems()
        {
            for (int i = 0; i < MathProblemHolder.instance.mathProblems.Count; i++)
            {
                Destroy(spaceships[i]);
            }

            MathProblemHolder.instance.mathProblems.Clear();
            spaceships.Clear();
        }

        public void IncreaseNumberOfProblemToCreate(int increaseAmount)
        {
            numberOfProblemsToCreate += increaseAmount;
            numberOfProblemsToCreateText.text = numberOfProblemsToCreate.ToString();
        }

        public void DecreaseNumberOfProblemToCreate(int decreaseAmount)
        {
            numberOfProblemsToCreate -= decreaseAmount;
            numberOfProblemsToCreateText.text = numberOfProblemsToCreate.ToString();//
        }
    }
}