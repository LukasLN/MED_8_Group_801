using UnityEngine;
using TMPro;
using System.Collections;

namespace AstroMath
{
    public class Tester_MathProblemDisplayer : MonoBehaviour
    {
        public TMP_Text spaceshipPositionText;
        public TMP_Text targetPositionText;
        public TMP_Text directionVectorText;

        public InputFieldOnlyNumbers inputFieldX, inputFieldY, inputFieldZ;

        Vector3 spaceshipPosition;
        Vector3 targetPosition;
        Vector3 directionVector;
        bool doesCollide;
        int tScale;

        public GameObject rightGO; //GO = GameObject
        public GameObject wrongGO;
        public float timeBeforeHideRightWrongGO;

        MathProblem currentMathProblem;

        public void CreateNewMathProblem()
        {
            currentMathProblem = MathProblemGenerator.GenerateMathProblem(false, 0);
            MathProblemHolder.instance.mathProblems.Add(currentMathProblem);

            spaceshipPosition = currentMathProblem.spaceshipPosition;
            targetPosition    = currentMathProblem.parkingPosition;
            directionVector   = currentMathProblem.directionAnswer;
            doesCollide       = currentMathProblem.collisionAnswer;
            tScale            = currentMathProblem.scaleAnswer;

            spaceshipPositionText.text = $"{spaceshipPosition.x}\n" +
                                         $"{spaceshipPosition.y}\n" +
                                         $"{spaceshipPosition.z}";

            targetPositionText.text = $"{targetPosition.x}\n" +
                                      $"{targetPosition.y}\n" +
                                      $"{targetPosition.z}";

            /*directionVectorText.text = $"{directionVector.x}\n" +
                                       $"{directionVector.y}\n" +
                                       $"{directionVector.z}";*/
        }

        public void CheckIfCorrect()
        {
            bool gotItRight = false;

            switch (currentMathProblem.type)
            {
                case MathProblem.Type.Direction:
                    gotItRight = DirectionAnswerIsCorrect();
                    break;
                case MathProblem.Type.Collision:
                    gotItRight = CollisionAnswerIsCorrect();
                    break;
                case MathProblem.Type.Scale:
                    gotItRight = CollisionAnswerIsCorrect();
                    break;
                default:
                    break;
            }

            var rightWrongObject = wrongGO;
            if(gotItRight) { rightWrongObject = rightGO; }

            StartCoroutine(WaitBeforeHideRightWrong(rightWrongObject));
        }

        IEnumerator WaitBeforeHideRightWrong(GameObject rightWrongObject)
        {
            rightWrongObject.SetActive(true);
            yield return new WaitForSeconds(timeBeforeHideRightWrongGO);
            rightWrongObject.SetActive(false);
        }

        bool DirectionAnswerIsCorrect()
        {
            Vector3 answer = new Vector3(inputFieldX.number, inputFieldY.number, inputFieldZ.number);

            return answer == directionVector;
        }

        bool CollisionAnswerIsCorrect()
        {
            bool answer = false;

            return answer == doesCollide;
        }

        bool ScaleAnswerIsCorrect()
        {
            int answer = 0;

            return answer == tScale;
        }
    }
}