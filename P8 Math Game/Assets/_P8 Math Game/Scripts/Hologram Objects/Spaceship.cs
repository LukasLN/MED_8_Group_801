using UnityEngine;

namespace AstroMath
{
    public class Spaceship : MonoBehaviour
    {
        #region Big Papa
        [Header("Big Papa")]
        public MathProblem mathProblem;
        #endregion

        #region Direction Vector
        [Header("Direction Vector")]
        [SerializeField] Vector3 correctDirectionVector;
        [SerializeField] Vector3 currentDirectionVector;
        #endregion

        #region Interactable Spaceship
        [Header("Interactable Spaceship")]
        [SerializeField] InteractableSpaceship interactableSpaceship;
        #endregion

        #region Info Panel
        [Header("Info Panel")]
        [SerializeField] GameObject infoPanelGO; //GO = GameObject
        [SerializeField] InfoPanel infoPanel;
        InfoPanelMovement infoPanelMovement;
        #endregion

        private void Awake()
        {
            if(infoPanelGO != null)
            {
                if(infoPanel == null)
                {
                    infoPanel = infoPanelGO.GetComponent<InfoPanel>();
                }
                if(infoPanelMovement == null)
                {
                    infoPanelMovement = infoPanelGO.GetComponent<InfoPanelMovement>();
                }
            }
        }

        public void SetCorrectDirectionVector(Vector3 direction)
        {
            correctDirectionVector = direction;
            interactableSpaceship.SetCorrectDirectionVector(direction);
            infoPanel.SetCorrectDirectionVector(direction);
        }

        public void SetCorrectTScalar(int tScalar)
        {
            //interactableSpaceship.SetCorrectTScalar(tScalar);
        }

        public void SetProblemPosition(Vector3 position)
        {
            interactableSpaceship.SetProblemPosition(position);
            infoPanel.SetStartPosition(position);
        }

        public void UpdateGraphics(int type, int cargo)
        {
            interactableSpaceship.SetActiveModel(cargo);
            infoPanel.UpdateGraphics(type, cargo);
        }

        public void UpdatePuzzleInformation(MathProblem mathProblem)
        {
            infoPanel.UpdatePuzzleInformation(mathProblem);
        }

        public void UpdateInteractions()
        {
            //if(mathProblem.m_type == MathProblem.Type.Scale)
            //{
            //    GetComponent<Grabbable>().enabled = false;
            //    GetComponent<HandGrabInteractable>().enabled = false;
            //}
        }
    }
}