using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
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

        public void SetCorrectCollisionAnswer(bool collisionAnswer)
        {
            interactableSpaceship.SetCorrectCollisionAnswer(collisionAnswer );
        }

        public void SetCorrectTargetGO(GameObject targetGO)
        {
            interactableSpaceship.SetCorrectTargetGO(targetGO);
        }

        public void SetTargetGO(GameObject targetGO)
        {
            interactableSpaceship.SetTargetGO(targetGO);
        }

        public void SetCorrectTScalar(int tScalar)
        {
            Debug.Log("Got to Spaceship!");
            interactableSpaceship.SetCorrectTScalar(tScalar);
        }

        public void SetProblemPosition(Vector3 position)
        {
            interactableSpaceship.SetProblemPosition(position);
            infoPanel.SetStartPosition(position);
        }

        public void SetPointerToRay()
        {
            interactableSpaceship.pointerShape = InteractableSpaceship.PointerShape.Ray;
        }

        public void SetLineRendererActivation(bool activation)
        {
            interactableSpaceship.gameObject.GetComponent<LineRenderer>().enabled = activation;
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
            if (mathProblem.GetType() == MathProblem.Type.Collision ||
               mathProblem.GetType() == MathProblem.Type.Scale)
            {
                interactableSpaceship.GetComponent<Grabbable>().enabled = false;
                interactableSpaceship.GetComponent<HandGrabInteractable>().enabled = false;
            }
        }

        public void SetRandomRotation()
        {
            interactableSpaceship.SetRandomRotation();
        }

        public void LookUp()
        {
            interactableSpaceship.LookUp();
        }

        public void LookAt(Transform target)
        {
            interactableSpaceship.LookAt(target);
        }
    }
}