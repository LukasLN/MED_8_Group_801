using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

namespace AstroMath
{
    public class InfoPanel : MonoBehaviour
    {
        [HideInInspector] public MathProblem mathProblem;

        [SerializeField] bool isSelected;

        public TMP_Text spaceshipPositionText;
        public TMP_Text directionVectorText;

        public Vector3 rayHitPoint;

        [SerializeField] Image cargoImage;
        [SerializeField] Sprite[] cargoSprites;
        [SerializeField] TMP_Text problemTypeText;

        #region Color
        [Header("Color")]
        [SerializeField] bool changePanelColor;
        [SerializeField] Image panelImage;
        [SerializeField] Color[] panelColors;
        #endregion

        [SerializeField] InputFieldOnlyNumbers x_inputField, y_inputField, z_inputField;
        [SerializeField] GameObject correctImageGO, wrongImageGO;
        GameObject assessmentImageToShowGO;
        [SerializeField] float secondsBeforeHideAssessment;

        bool result;

        private void Update()
        {
            if(isSelected == true)
            {
                UpdateDirectionVectorText();
            }
        }

        public void UpdateGraphics()
        {
            UpdateCargoImage();
            UpdateProblemTypeText();

            if(changePanelColor == true)
            {
                UpdatePanelColor();
            }
        }

        void UpdateCargoImage()
        {
            cargoImage.sprite = cargoSprites[(int)mathProblem.cargo];
        }

        void UpdateProblemTypeText()
        {
            var correctText = "A";
            switch ((int)mathProblem.type)
            {
                case 0: //Direction problem
                    correctText = "A";
                    break;
                case 1: //Collision problem
                    correctText = "B";
                    break;
                case 2: //Scalar t problem
                    correctText = "C";
                    break;
            }

            problemTypeText.text = correctText;
        }

        void UpdatePanelColor()
        {
            panelImage.color = panelColors[(int)mathProblem.cargo];
        }

        public void UpdateDirectionVectorText()
        {
            
              // magic numbers are limits of holorgram 'cookie' and spawning sphere
            float x = PositionGenerator.Map(rayHitPoint.x, -4.5f, 4.5f, -100, 100);
            float y = PositionGenerator.Map(rayHitPoint.y, -0.5f, 0.5f, -100, 100);
            float z = PositionGenerator.Map(rayHitPoint.z, -4.5f, 4.5f, -100, 100);
            Vector3 direction = new Vector3(x, y, z) - mathProblem.spaceshipPosition;

            directionVectorText.text = $"({direction.x:F1})\n" +
                                       $"({direction.y:F1})\n" +
                                       $"({direction.z:F1})";
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

        public void SetIsSelected(bool newBool)
        {
            isSelected = newBool;
        }
    }
}