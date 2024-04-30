using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

namespace AstroMath
{
    public class InfoPanel : MonoBehaviour
    {
        public MathProblem mathProblem;
        //public int problemID;

        [SerializeField] bool isSelected;
        [HideInInspector] public Vector3 directionVector;

        public TMP_Text startPositionText;
        public TMP_Text directionVectorText;
        public Transform EndPointTF;

        public Vector3 rayHitPoint;

        [SerializeField] Image cargoImage;
        [SerializeField] Sprite[] cargoSprites;
        [SerializeField] TMP_Text problemTypeText;

        #region Color
        [Header("Color")]
        [SerializeField] bool changePanelColor;
        [SerializeField] Image[] panelImages;
        [SerializeField] Image[] confirmButtonImages;
        [SerializeField] Color[] panelColors;
        #endregion

        #region Locks
        [Header("Locks")]
        [SerializeField] GameObject startPositionLock;
        [SerializeField] GameObject directionVectorLock;
        [SerializeField] GameObject tScalarLock;
        #endregion

        public Keypad keypad;
        public Button confirmButton;
        [SerializeField] GameObject spaceshipGO;

        AudioPlayer audioPlayer;

        private void Start()
        {
            audioPlayer = GetComponent<AudioPlayer>();

            directionVector = new Vector3(0, 0, 0);
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
            spaceshipGO.GetComponent<Spaceship>().LockInAnswer();
        }

        public void UpdateGraphics()
        {
            UpdateCargoImage();
            UpdateProblemTypeText();
            //Debug.Log("Start Position: " + mathProblem.spaceshipPosition);
            startPositionText.text = $"{mathProblem.spaceshipPosition.x}\n" +
                                    $"{mathProblem.spaceshipPosition.y}\n" +
                                    $"{mathProblem.spaceshipPosition.z}";

            if(changePanelColor == true)
            {
                UpdatePanelColor();
                UpdateConfirmButtonColor();
            }
        }

        public void UpdatePuzzleInformation()
        {
            keypad.correctCode = mathProblem.pinCode;
            //Debug.Log("The pin code of this math problem is: " + mathProblem.pinCode);
            keypad.targetIDText.text = "# " + mathProblem.targetID;
        }

        void UpdateCargoImage()
        {
            cargoImage.sprite = cargoSprites[(int)mathProblem.cargo];
        }

        void FollowEndPoint()
        {
            if(mathProblem.type == MathProblem.Type.Scale) {
                transform.parent = EndPointTF;
            }
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
            for (int i = 0; i < panelImages.Length; i++)
            {
                panelImages[i].color = panelColors[(int)mathProblem.cargo];
            }
        }

        void UpdateConfirmButtonColor()
        {
            for (int i = 0; i < confirmButtonImages.Length; i++)
            {
                confirmButtonImages[i].color = panelColors[(int)mathProblem.cargo];
            }
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

        public void ShowStartPosition()
        {
            SetActivation(startPositionLock, false);
        }

        public void ShowDirectionVector()
        {
            SetActivation(directionVectorLock, false);
        }

        void SetActivation(GameObject gameObject, bool newBool)
        {
            gameObject.SetActive(newBool);
        }

        public void PlayButtonPressSound()
        {
            audioPlayer.PlaySoundEffect("ButtonPress");
        }
    }
}