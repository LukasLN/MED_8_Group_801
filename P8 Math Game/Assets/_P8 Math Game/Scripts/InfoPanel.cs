using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

namespace AstroMath
{
    public class InfoPanel : MonoBehaviour
    {
        public TMP_Text spaceshipPositionText;
        public TMP_Text dockPositionText;

        [HideInInspector] public MathProblem mathProblem;

        [SerializeField] InputFieldOnlyNumbers x_inputField, y_inputField, z_inputField;
        [SerializeField] GameObject correctImageGO, wrongImageGO;
        GameObject assessmentImageToShowGO;
        [SerializeField] float secondsBeforeHideAssessment;

        bool result;

        public void SetPositions()
        {
            spaceshipPositionText.text = $"({mathProblem.spaceshipPosition.x})\n" +
                                         $"({mathProblem.spaceshipPosition.y})\n" +
                                         $"({mathProblem.spaceshipPosition.z})";

            dockPositionText.text = $"({mathProblem.parkingPosition.x})\n" +
                                    $"({mathProblem.parkingPosition.y})\n" +
                                    $"({mathProblem.parkingPosition.z})";
        }

        public void CheckIfIsCorrect()
        {
            result = IsCorrect();
            
            if(result == true)
            {
                assessmentImageToShowGO = correctImageGO;
            }
            else
            {
                assessmentImageToShowGO = wrongImageGO;
            }

            assessmentImageToShowGO.SetActive(true);
            StartCoroutine(WaitBeforeHideAssessment());
        }

        IEnumerator WaitBeforeHideAssessment()
        {
            yield return new WaitForSeconds(secondsBeforeHideAssessment);
            assessmentImageToShowGO.SetActive(false);
            if(result == true)
            {
                int index = MathProblemHolder.instance.mathProblems.IndexOf(mathProblem);
                MathProblemHolder.instance.mathProblems.RemoveAt(index);
                Destroy(MathProblemGeneratorPanel.instance.spaceships[index]);
                MathProblemGeneratorPanel.instance.spaceships.RemoveAt(index);
                Destroy(MathProblemGeneratorPanel.instance.parkings[index]);
                MathProblemGeneratorPanel.instance.parkings.RemoveAt(index);
            }
        }

        bool IsCorrect()
        {
            bool result = false;

            switch(mathProblem.type)
            {
                case MathProblem.Type.Direction:
                    Vector3 answerGiven = new Vector3(x_inputField.number, y_inputField.number, z_inputField.number);
                    result = answerGiven == mathProblem.directionAnswer;
                    break;
                case MathProblem.Type.Collision:

                    break;
                case MathProblem.Type.Scale:

                    break;
            }

            return result;
        }
    }
}