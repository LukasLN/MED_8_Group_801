using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace AstroMath
{
    public class InfoPanel : MonoBehaviour
    {
        public MathProblem mathProblem;

        [SerializeField] Vector3 wristWatchOffset;
        [SerializeField] Vector3 wristWatchRotation;

        public int unlockingType;

        #region Booleans
        [Header("Booleans")]
        [SerializeField] bool isSelected;
        #endregion

        #region Math Problem
        [Header("Math Problem")]
        [SerializeField] Vector3 startPosition;
        [SerializeField] Vector3 correctDirectionVector;
        [SerializeField] Vector3 directionVector;
        [SerializeField] float correctTScalar;
        [SerializeField] float tScalar;
        #endregion

        #region Texts
        [Header("Texts")]
        public TMP_Text startPositionText;
        public TMP_Text directionVectorText;
        public TMP_Text tScalarText;
        #endregion

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
            SetTScalar(1);

            if(FindAnyObjectByType<MathProblemManager>() != null && MathProblemManager.instance.isForSUIDJK == true)
            {
                WristWatch wristWatch = FindObjectOfType<WristWatch>();

                #region Parenting and Positioning
                transform.parent = wristWatch.gameObject.transform;
                transform.localPosition = wristWatchOffset;
                transform.localRotation = Quaternion.Euler(wristWatchRotation);
                #endregion
            }
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
            spaceshipGO.GetComponent<InteractableSpaceship>().LockInAnswer();
        }

        public void UpdateGraphics(int type, int cargo)
        {
            #region Problem and Cargo
            UpdateProblemTypeText(type);
            UpdateCargoImage(cargo);
            #endregion

            #region Formula Locks
            HideStartPosition();
            HideDirectionVector();
            HideTScalar();
            #endregion

            #region Pin Code / Keypad
            keypad.Clear();
            #endregion

            #region Mini Game

            #endregion

            #region Colors
            if (changePanelColor == true)
            {
                UpdatePanelColor();
                UpdateConfirmButtonColor();
            }
            #endregion
        }



        public void UpdatePuzzleInformation(MathProblem mathProblem)
        {
            #region Pin Code / Keypad
            //> Correct pin code
            keypad.SetCorectCode(mathProblem.GetPinCode());

            //> What locks that the correct pin code with unlock
            unlockingType = (int)mathProblem.GetType(); //0 = Direction, 1 = Collision, 2 = Scalar

            //> Target Name and Image
            var targetName = $"# {mathProblem.GetTargetName()}"; //assuming we are dealing with a direction problem and therefore a parking spot number
            var targetImageIndex = 0;
            if ((int)mathProblem.GetType() == 1) //if we are dealing with a collision problem
            {
                targetName = mathProblem.GetTargetName(); //then the target name is that of an asteroid, which should not have a hashtag '#' in front of it
                targetImageIndex = 1;
            }
            keypad.SetTargetNameText(targetName);
            keypad.SetTargetImageSprite(targetImageIndex);
            #endregion
        }

        void UpdateProblemTypeText(int type)
        {
            var correctText = "";
            switch (type)
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

        void UpdateCargoImage(int cargo)
        {
            cargoImage.sprite = cargoSprites[cargo];
        }

        void FollowEndPoint()
        {
            //if(mathProblem.m_type == MathProblem.Type.Scale) {
            //    transform.parent = EndPointTF;
            //}
        }
        

        void UpdatePanelColor()
        {
            for (int i = 0; i < panelImages.Length; i++)
            {
                //panelImages[i].color = panelColors[(int)mathProblem.m_cargo];
            }
        }

        void UpdateConfirmButtonColor()
        {
            for (int i = 0; i < confirmButtonImages.Length; i++)
            {
                //confirmButtonImages[i].color = panelColors[(int)mathProblem.m_cargo];
            }
        }

        public void SetStartPosition(Vector3 position)
        {
            startPosition = position;
            startPositionText.text = $"{startPosition.x}\n" +
                                    $"{startPosition.y}\n" +
                                    $"{startPosition.z}";
        }

        public void SetCorrectDirectionVector(Vector3 direction)
        {
            correctDirectionVector = direction;
        }

        public void SetCurrentDirectionVector(Vector3 direction)
        {
            directionVector = direction;
            UpdateDirectionVectorText();
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

        public void SetCorrectTScalar(int tScalar)
        {
            correctTScalar = tScalar;
        }

        public void SetTScalar(int tScalar)
        {
            this.tScalar = tScalar;
            UpdateTScalarText();
        }

        public void UpdateTScalarText()
        {
            tScalarText.text = tScalar.ToString();
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

        public void ShowTScalar()
        {
            SetActivation(tScalarLock, false);
        }

        public void HideStartPosition()
        {
            SetActivation(startPositionLock, true);
        }

        public void HideDirectionVector()
        {
            SetActivation(directionVectorLock, true);
        }

        public void HideTScalar()
        {
            SetActivation(tScalarLock, true);
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