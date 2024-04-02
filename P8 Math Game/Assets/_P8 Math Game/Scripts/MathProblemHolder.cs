using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace AstroMath
{
    public class MathProblemHolder : MonoBehaviour
    {
        public static MathProblemHolder instance;
        [SerializeField] public List<MathProblem> mathProblems;
        public List<GameObject> spaceships;
        [SerializeField] GameObject spaceshipPF; //PF = Prefab
        [SerializeField] Transform spaceshipParentTF; //TF = Transform

        [SerializeField] int numberOfProblemsToCreate;
        [SerializeField] TMP_Text numberOfProblemsToCreateText;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            numberOfProblemsToCreateText.text = numberOfProblemsToCreate.ToString();
        }

        public void CreateMathProblems()
        {
            ClearMathProblems();

            for (int i = 0; i < numberOfProblemsToCreate; i++)
            {
                mathProblems.Add(MathProblemGenerator.GenerateMathProblem());
                spaceships.Add(Instantiate(spaceshipPF, mathProblems[i].spaceshipPosition, Quaternion.identity, spaceshipParentTF));
            }
        }

        public void ClearMathProblems()
        {
            for (int i = 0; i < mathProblems.Count; i++)
            {
                Destroy(spaceships[i]);
            }

            mathProblems.Clear();
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