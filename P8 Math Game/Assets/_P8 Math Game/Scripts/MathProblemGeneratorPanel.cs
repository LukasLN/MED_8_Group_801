using UnityEngine;
using TMPro;

namespace AstroMath
{
    public class MathProblemGeneratorPanel : MonoBehaviour
    {
        public static MathProblemGeneratorPanel instance;

        #region Amount
        [Header("Amount")]
        [SerializeField] int numberOfProblemsToCreate;
        [SerializeField] int minNumberOfProblems = 5;
        [SerializeField] int maxNumberOfProblems = 20;
        [SerializeField] int increaseAmount = 5;
        [SerializeField] int decreaseAmount = 5;
        [SerializeField] TMP_Text numberOfProblemsToCreateText;
        #endregion

        #region Type
        public enum ProblemType
        {
            Direction = 0,
            Collision = 1,
            Scale = 2,
            Random = 3
        }
        [Header("Type")]
        [SerializeField] ProblemType mathProblemType;
        [SerializeField] TMP_Text mathProblemTypeText;
        #endregion

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            numberOfProblemsToCreate = minNumberOfProblems;
        }

        public void IncreaseMathProblemType()
        {
            mathProblemType++;

            if((int)mathProblemType > 3)
                mathProblemType = 0;

            MathProblemManager.instance.mathProblemType = (MathProblemManager.MathProblemType)(int)mathProblemType;

            mathProblemTypeText.text = mathProblemType.ToString();
        }

        public void DecreaseMathProblemType()
        {
            mathProblemType--;

            if((int)mathProblemType < 0)
                mathProblemType = (ProblemType)3;

            MathProblemManager.instance.mathProblemType = (MathProblemManager.MathProblemType)(int)mathProblemType;

            mathProblemTypeText.text = mathProblemType.ToString();
        }

        public void IncreaseNumberOfProblemToCreate()
        {
            if(numberOfProblemsToCreate >= maxNumberOfProblems)
                return;

            numberOfProblemsToCreate += increaseAmount;

            if(numberOfProblemsToCreate > maxNumberOfProblems)
                numberOfProblemsToCreate = maxNumberOfProblems;

            numberOfProblemsToCreateText.text = numberOfProblemsToCreate.ToString();
        }

        public void DecreaseNumberOfProblemToCreate()
        {
            if(numberOfProblemsToCreate <= minNumberOfProblems)
                return;

            numberOfProblemsToCreate -= decreaseAmount;

            if(numberOfProblemsToCreate < minNumberOfProblems)
                numberOfProblemsToCreate = minNumberOfProblems;

            numberOfProblemsToCreateText.text = numberOfProblemsToCreate.ToString();
        }

        public void CreateMathProblems()
        {
            MathProblemManager.instance.CreateNumberOfProblems(numberOfProblemsToCreate);
        }
    }
}