using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

namespace AstroMath
{
    public class InfoPanel : MonoBehaviour
    {
        public MathProblem mathProblem;
        public int problemID;

        [SerializeField] bool isSelected;
        [HideInInspector] public Vector3 directionVector;

        public TMP_Text startPositionText;
        public TMP_Text directionVectorText;

        public Vector3 rayHitPoint;

        [SerializeField] Image cargoImage;
        [SerializeField] Sprite[] cargoSprites;
        [SerializeField] TMP_Text problemTypeText;

        #region Color
        [Header("Color")]
        [SerializeField] bool changePanelColor;
        [SerializeField] Image panelImage;
        [SerializeField] Image confirmButtonImage;
        [SerializeField] Color[] panelColors;
        #endregion

        private void Start()
        {
            directionVector = new Vector3(0, 0, 0);

            startPositionText.text = "0\n0\n0";
            directionVectorText.text = "0\n0\n0";
        }

        private void Update()
        {
            if(isSelected == true)
            {
                UpdateDirectionVectorText();
            }
        }

        public void ConfirmAnswer()
        {
            bool isCorrect = false;

            switch(mathProblem.type)
            {
                case MathProblem.Type.Direction:
                    // Check if the direction vector is correct
                    if(directionVector == mathProblem.directionSolution)
                    {
                        Debug.Log("Correct Direction!");
                        isCorrect = true;
                    }
                    else
                    {
                        Debug.Log("Incorrect Direction!");
                    }
                    break;
                case MathProblem.Type.Collision:
                    // Check if the collision is correct
                    Debug.LogWarning("Not implemented yet!");
                    break;
                case MathProblem.Type.Scale:
                    // Check if the scale is correct
                    Debug.LogWarning("Not implemented yet!");
                    break;
            }

            if(isCorrect == true)
            {
                if(HoloObjectManager.instance != null)
                {
                    HoloObjectManager.instance.DespawnHoloObjects(problemID);
                }
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
            var correctText = "";
            switch ((int)mathProblem.type)
            {
                case 0: //Direction problem
                    correctText = "Retning";
                    break;
                case 1: //Collision problem
                    correctText = "Kollision";
                    break;
                case 2: //Scalar t problem
                    correctText = "Skalering";
                    break;
            }

            problemTypeText.text = correctText;
        }

        void UpdatePanelColor()
        {
            panelImage.color = panelColors[(int)mathProblem.cargo];
            confirmButtonImage.color = panelColors[(int)mathProblem.cargo];
        }

        public void UpdateDirectionVectorText()
        {
            // magic numbers are limits of holorgram 'cookie' and spawning sphere
            //float x = PositionGenerator.Map(rayHitPoint.x, -4.5f, 4.5f, -100, 100);
            //float y = PositionGenerator.Map(rayHitPoint.y, -0.5f, 0.5f, -100, 100);
            //float z = PositionGenerator.Map(rayHitPoint.z, -4.5f, 4.5f, -100, 100);
            //Vector3 direction = new Vector3(x, y, z) - mathProblem.spaceshipPosition;

            directionVectorText.text = $"{directionVector.x}\n" +
                                       $"{directionVector.y}\n" +
                                       $"{directionVector.z}";
        }

        public void SetIsSelected(bool newBool)
        {
            isSelected = newBool;
        }
    }
}