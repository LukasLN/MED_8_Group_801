using Oculus.Interaction;
using System.Collections;
using UnityEngine;

namespace AstroMath
{
    public class Spaceship : MonoBehaviour
    {
        [SerializeField] float timeBeforeDestroy;

        [HideInInspector] public Vector3 directionVector;

        [SerializeField] bool isSelected;
        public bool hasTarget;
        public GameObject targetGO;
        public bool lineVisible;

        [SerializeField] InteractableSpaceship interactableSpaceship;

        #region Info Panel
        [Header("Info Panel")]
        [SerializeField] GameObject infoPanelGO; //GO = GameObject
        [HideInInspector] public InfoPanel infoPanel;
        [HideInInspector] InfoPanelMovement infoPanelMovement;
        #endregion

        #region Line
        [Header("Line")]
        [SerializeField] GameObject lineGO; //GO = GameObject
        LineRenderer lineRenderer;
        [SerializeField] Transform lineStartPoint;
        [SerializeField] Transform lineEndPoint;
        [SerializeField] GameObject sphere;
        [SerializeField] float maxDistance;
        [SerializeField] LayerMask layerMaskToIgnore;
        RaycastHit hit;
        #endregion

        #region Temporary Correct Wrong 
        [Header("Temporary Correct Wrong")]
        [SerializeField] GameObject correctGO;
        [SerializeField] GameObject wrongGO;
        #endregion

        private void Awake()
        {
            //lineRenderer = lineGO.GetComponent<LineRenderer>();
            //lineRenderer.SetPosition(0, lineStartPoint.localPosition);
            infoPanel = infoPanelGO.GetComponent<InfoPanel>();

            OVRCameraRig cameraRig = FindObjectOfType<OVRCameraRig>();
            infoPanelMovement = infoPanelGO.GetComponent<InfoPanelMovement>();
            infoPanelMovement.player = cameraRig.centerEyeAnchor;


            //if (mathProblem.m_type == MathProblem.Type.Scale)
            {
                lineVisible = true;
                var sphereInstance=  Instantiate(sphere,lineEndPoint);
                sphereInstance.transform.SetParent(transform);
            }
        }

        private void Update()
        {
            if (isSelected == true || lineVisible==true)
            {
                //DrawLine();
                UpdateDirectionVector();
                //UpdateGraphics();
            }

            if (hasTarget == true)
            {
                transform.LookAt(targetGO.transform);
                //Debug.Log("Hit Transform Position: " + hit.transform.position);
            }
        }

        public void LookAtTargetOnUnselect()
        {
            if (hasTarget == true)
            {
                transform.LookAt(targetGO.transform);
            }
        }

        public void UpdateInteractions()
        {
            //if(mathProblem.m_type == MathProblem.Type.Scale)
            //{
            //    GetComponent<Grabbable>().enabled = false;
            //    GetComponent<HandGrabInteractable>().enabled = false;
            //}
        }

        public void UpdateGraphics(int type, int cargo)
        {
            interactableSpaceship.SetActiveModel(cargo);

            infoPanel.UpdateGraphics(type, cargo);
        }

        public void UpdatePuzzleInformation()
        {
            infoPanel.UpdatePuzzleInformation();
        }

        public void DrawLine()
        {
            #region Shooting a ray to get distance to edge
            float raylength = maxDistance;

            Ray ray = new Ray(lineStartPoint.position, lineStartPoint.forward);

            if (Physics.Raycast(ray, out hit, maxDistance, ~layerMaskToIgnore))
            {
                //Debug.Log("Name of hit GameObject: " + hit.transform.gameObject.name);

                raylength = Vector3.Distance(lineStartPoint.position, hit.point);
                //
                if (hit.collider.gameObject.tag == "Parking" ||
                    hit.collider.gameObject.tag == "Asteroid")
                {
                    hasTarget = true;
                    targetGO = hit.collider.gameObject;
                    infoPanel.confirmButton.interactable = true;

                    targetGO.GetComponent<HoloParkingSpot>().SetHighlightActivation(true);
                }
            }
            else
            {
                hasTarget = false;
                if(targetGO != null)
                {
                    targetGO.GetComponent<HoloParkingSpot>().SetHighlightActivation(false);
                }
                targetGO = null;
                infoPanel.confirmButton.interactable = false;
            }
            #endregion

            Vector3 rayEndPosition = new Vector3(lineStartPoint.localPosition.x,
                                              lineStartPoint.localPosition.y,
                                              lineStartPoint.localPosition.z + raylength);
            lineRenderer.SetPosition(1, rayEndPosition);
        }

        void UpdateDirectionVector()
        {
            var newDirectionVector = new Vector3(0, 0, 0);

            if(hasTarget == true) //if we are hitting a target (i.e. PARKING or ASTEROID)
            {
                //newDirectionVector = targetGO.GetComponent<HoloParkingSpot>().mathProblem.targetPosition -
                //                                                              mathProblem.spaceshipPosition;
            }
            else
            {
                var originalVector = new Vector3(0,0,0);

                var mappedX = 0f;
                var mappedY = 0f;
                var mappedZ = 0f;

                if (hit.transform == null) //if we are not hitting anything at all
                {
                    originalVector = transform.forward * maxDistance;

                    //HARD-CODED NUMBERS!!!
                    mappedX = PositionGenerator.Map(originalVector.x, -4.5f, 4.5f, -100f, 100f);
                    mappedY = PositionGenerator.Map(originalVector.y, -1f, 1f, -100f, 100f);
                    mappedZ = PositionGenerator.Map(originalVector.z, -4.5f, 4.5f, -100f, 100f);
                }
                else //if we are hitting something that is NOT A TARGET
                {
                    originalVector = hit.point;

                    //HARD-CODED NUMBERS!!!
                    mappedX = PositionGenerator.Map(originalVector.x, -4.5f, 4.5f, -100f, 100f);
                    mappedY = PositionGenerator.Map(originalVector.y, 0.15f, 8f, -100f, 100f); //Not perfect :/
                    mappedZ = PositionGenerator.Map(originalVector.z, -4.5f, 4.5f, -100f, 100f);
                }

                //newDirectionVector = new Vector3(mappedX, mappedY, mappedZ) - mathProblem.spaceshipPosition;
            }

            newDirectionVector.x = (int)newDirectionVector.x;
            newDirectionVector.y = (int)newDirectionVector.y;
            newDirectionVector.z = (int)newDirectionVector.z;

            infoPanel.directionVector = newDirectionVector;
            directionVector = newDirectionVector;
        }

        public void LockInAnswer()
        {
            //isMoving = true;
            GetComponent<InteractableUnityEventWrapper>().enabled = false;
            infoPanelGO.SetActive(false);
            lineGO.SetActive(false);
            //SetTarget(targetGO);
        }

        public void ShowResult()
        {
            #region Determine result
            bool isCorrect = false;

            //switch(mathProblem.m_type)
            //{
            //    case MathProblem.Type.Direction:
            //        // Check if the direction vector is correct
            //        if(directionVector == mathProblem.directionSolution)
            //        {
            //            isCorrect = true;
            //        }
            //        break;
            //    case MathProblem.Type.Collision:
            //        // Check if the collision is correct
            //        Debug.LogWarning("Not implemented yet!");
            //        break;
            //    case MathProblem.Type.Scale:
            //        // Check if the scale is correct
            //        Debug.LogWarning("Not implemented yet!");
            //        break;
            //}
            #endregion

            # region Show Result
            if (isCorrect == true)
            {
                //success sound
                correctGO.SetActive(true);
                StartCoroutine(WaitBeforeIncrementSolved()); //wait before incrementing score
            }
            else
            {
                //failure sound
                wrongGO.SetActive(true);
            }
            #endregion

            StartCoroutine(WaitBeforeDestroy()); //wait before destroying the spaceship
        }

        IEnumerator WaitBeforeDestroy()
        {
            yield return new WaitForSeconds(timeBeforeDestroy);
            MathProblemManager.instance.CreateMathProblem();
        }

        IEnumerator WaitBeforeIncrementSolved()
        {
            yield return new WaitForSeconds(timeBeforeDestroy);
            WristWatch.instance.IncrementSolved();   
        }

        public void SetMathProblem(MathProblem newMathProblem)
        {
            //mathProblem = newMathProblem;
            infoPanel.mathProblem = newMathProblem;
        }

        //public void SetProblemID(int newProblemID)
        //{
        //    problemID = newProblemID;
        //    infoPanel.problemID = newProblemID;
        //}

        public void SetIsSelected(bool newBool)
        {
            isSelected = newBool;
            infoPanel.SetIsSelected(newBool);
        }
    }
}