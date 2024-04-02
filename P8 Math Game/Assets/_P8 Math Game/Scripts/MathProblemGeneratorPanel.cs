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
                MathProblemHolder.instance.mathProblems.Add(MathProblemGenerator.GenerateMathProblem());
                spaceships.Add(Instantiate(spaceshipPF, MathProblemHolder.instance.mathProblems[i].spaceshipPosition, Quaternion.identity, spaceshipParentTF));
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