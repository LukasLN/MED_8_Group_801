using System.Collections;
using Oculus.Interaction;
using UnityEngine;

namespace AstroMath
{
    public class InteractableSpaceship : MonoBehaviour
    {
        #region Enums
        public enum PointerShape
        {
            Ray = 0,
            Cylinder = 1,
            Cone = 2
        }
        #endregion

        #region Big papa
        [Header("Big Papa")]
        [SerializeField] Spaceship spaceship;
        #endregion


        #region Problem Information
        [Header("Problem Information")]
        [SerializeField] Vector3 problemPosition;
        #endregion

        #region Direction Vector
        [Header("Direction Vector")]
        [SerializeField] Vector3 correctDirectionVector;
        [SerializeField] Vector3 currentDirectionVector;
        #endregion

        #region Info Panel
        [Header("Info Panel")]
        [SerializeField] GameObject infoPanelGO; //GO = GameObject
        InfoPanel infoPanel;
        #endregion

        #region Interaction
        [Header("Interaction")]
        [SerializeField] bool isSelected;
        #endregion

        #region Target
        [Header("Target")]
        [SerializeField] bool hasTarget;
        [SerializeField] GameObject targetGO;
        [SerializeField] string[] tagsToLookFor;
        #endregion

        #region 3D Models
        [Header("3D Models")]
        [SerializeField] GameObject[] modelsGO; //GO = GameObject
        #endregion

        #region Movement
        [Header("Movement")]
        [SerializeField] bool isMoving;
        [SerializeField] float timeToMove;
        [SerializeField] float moveSpeed;
        #endregion

        #region Ray Parameters
        [Header("Ray Parameters")]
        [SerializeField] PointerShape pointerShape;
        [SerializeField] LayerMask layerMaskToIgnore;
        [SerializeField] float rayMaxDistance;
        [SerializeField] float cylinderRadius;
        [SerializeField] float coneAngle;
        [SerializeField] int coneResolution;
        Ray ray;
        RaycastHit hit;
        #endregion

        #region Line Graphics
        [Header("Line Graphics")]
        LineRenderer lineRenderer;
        [SerializeField] Transform startPoint;
        [SerializeField] Transform endPoint;
        #endregion

        #region Assessment
        [Header("Assessment")]
        [SerializeField] GameObject correctGO;
        [SerializeField] GameObject wrongGO;
        [SerializeField] float timeBeforeDestroy;
        #endregion

        private void Awake()
        {
            //var sphereInstance =  Instantiate(sphere,lineEndPoint);
            //sphereInstance.transform.SetParent(transform);
        }

        private void Start()
        {
            if(infoPanelGO != null)
            {
                infoPanel = infoPanelGO.GetComponent<InfoPanel>();
            }

            lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.SetPosition(0, startPoint.position);

            UpdateCurrentDirectionVector();
            infoPanel.SetCurrentDirectionVector(currentDirectionVector);
        }

        void Update()
        {
            if (isSelected)
            {
                //targetGO = null;
                //hasTarget = false;

                #region Pointing
                switch (pointerShape)
                {
                    case PointerShape.Ray:
                        #region Ray

                        #endregion
                        break;
                    case PointerShape.Cylinder:
                        #region Cylinder

                        #endregion
                        break;
                    case PointerShape.Cone:
                        #region Cone
                        
                        float horizontalStepAngle = coneAngle / coneResolution - 1;
                        float verticalStepAngle   = coneAngle / coneResolution - 1;

                        for(int i = 0; i < coneResolution; i++)
                        {
                            Quaternion horizontalRotation = Quaternion.AngleAxis(-coneAngle / 2 + i * horizontalStepAngle, transform.up);

                            for(int j = 0; j < coneResolution; j++)
                            {
                                Quaternion verticalRotation = Quaternion.AngleAxis(-90 + j * verticalStepAngle, transform.right);

                                Vector3 rayDirection = verticalRotation * horizontalRotation * transform.forward;

                                ShootRay(startPoint.position, rayDirection); //shoots rays from the spaceship in a cone shape
                            }
                        }
                        #endregion
                        break;
                }

                ShootRay(startPoint.position, startPoint.forward); //shoots ray from spaceship in its forward direction
                #endregion

                UpdateCurrentDirectionVector();
                infoPanel.SetCurrentDirectionVector(currentDirectionVector);
            }

            //################################################################################################
            //    > IDK WHY, BUT FOR SOME REASON, THIS PLACEMENT OF THIS IF-STATEMENT CREATES (in Laus opinion)
            //      THE FEELING OF ROTATING THE SPACESHIP :,)
            //################################################################################################
            if (hasTarget == true) 
            {
                transform.LookAt(targetGO.transform);
            }
            //################################################################################################

            if (isMoving == true)
            {
                MoveForward();

                if (Vector3.Distance(transform.position, targetGO.transform.position) < 0.1f) //if we are not at the target position
                {
                    isMoving = false;

                    ShowResult();
                }
            }

            infoPanel.SetIsSelected(isSelected);
        }

        void ShootRay(Vector3 startPosition, Vector3 direction)
        {
            lineRenderer.SetPosition(0, startPoint.position);

            float raylength = rayMaxDistance;
            Color rayColor = Color.green;

            if (Physics.Raycast(startPosition, direction, out hit, rayMaxDistance, ~layerMaskToIgnore))
            {
                //Debug.Log("Hit object: " + hit.collider.gameObject.name);

                if (CheckForTarget())
                {
                    if(targetGO != null)
                    {
                        targetGO.GetComponent<TargetGameObject>().SetHighlightActivation(false);
                    }
                    hasTarget = true;
                    targetGO = hit.collider.gameObject;
                    targetGO.GetComponent<TargetGameObject>().SetHighlightActivation(true);
                    infoPanel.confirmButton.interactable = true;
                }

                raylength = Vector3.Distance(startPoint.position, hit.point);
                rayColor = Color.red;
            }
            else
            {
                Debug.Log("We are not hitting anything...");
                if(targetGO != null)
                {
                    Debug.Log("Already have a target, so disabling its activation...");
                    targetGO.GetComponent<TargetGameObject>().SetHighlightActivation(false);
                }
                hasTarget = false;
                targetGO = null;
                infoPanel.confirmButton.interactable = false;
            }
            //Debug.DrawRay(startPoint.position, rayDirection * rayMaxDistance, rayColor);

            endPoint.localPosition = new Vector3(startPoint.localPosition.x,
                                                 startPoint.localPosition.y,
                                                 startPoint.localPosition.z + raylength);

            lineRenderer.SetPosition(1, endPoint.position);
        }

        bool CheckForTarget()
        {
            for (int i = 0; i < tagsToLookFor.Length; i++)
            {
                if (hit.collider.gameObject.tag == tagsToLookFor[i])
                {
                    return true;
                }
            }

            return false;
        }

        void MoveForward()
        {
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
            //Vector3.MoveTowards(transform.position, targetPosition, 2); //moves spaceship
        }


        public void SetTarget(GameObject targetGO) //Activated when confirm button is pressed
        {
            this.targetGO = targetGO;

            var distanceToTravel = Vector3.Distance(transform.position, this.targetGO.transform.position);
            moveSpeed = distanceToTravel / timeToMove;

            //transform.position = Vector3.MoveTowards(interactableSpaceshipGO.transform.position, target.transform.position, step); //moves spaceship

            isMoving = true;
        }

        public void LockInAnswer()
        {
            SetTarget(targetGO);
            isMoving = true;
            hasTarget = false;
            isSelected = false;
            GetComponent<InteractableUnityEventWrapper>().enabled = false;
            infoPanelGO.SetActive(false);
            lineRenderer.enabled = false;
        }

        public void ShowResult()
        {
            #region Determine result
            bool isCorrect = false;

            switch (spaceship.mathProblem.GetType())
            {
                case MathProblem.Type.Direction:
                    // Check if the direction vector is correct
                    if (currentDirectionVector == correctDirectionVector)
                    {
                        isCorrect = true;
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

        public void SetCorrectDirectionVector(Vector3 direction)
        {
            correctDirectionVector = direction;
        }

        void UpdateCurrentDirectionVector()
        {
            var newDirectionVector = new Vector3(0, 0, 0);

            if (hasTarget == true) //if we are hitting a target (i.e. PARKING or ASTEROID)
            {
                newDirectionVector = targetGO.GetComponent<TargetGameObject>().problemPosition - problemPosition;
            }
            else
            {
                var pointerEndPoint = new Vector3(0, 0, 0);

                var mappedX = 0f;
                var mappedY = 0f;
                var mappedZ = 0f;

                if (hit.transform == null) //if we are not hitting anything at all
                {
                    pointerEndPoint = transform.forward * rayMaxDistance;

                    //HARD-CODED NUMBERS!!!
                    mappedX = PositionGenerator.Map(pointerEndPoint.x, -4.5f, 4.5f, -100f, 100f);
                    mappedY = PositionGenerator.Map(pointerEndPoint.y, -1f, 1f, -100f, 100f);
                    mappedZ = PositionGenerator.Map(pointerEndPoint.z, -4.5f, 4.5f, -100f, 100f);
                }
                else //if we are hitting something that is NOT A TARGET
                {
                    pointerEndPoint = hit.point;

                    //HARD-CODED NUMBERS!!!
                    mappedX = PositionGenerator.Map(pointerEndPoint.x, -4.5f, 4.5f, -100f, 100f);
                    mappedY = PositionGenerator.Map(pointerEndPoint.y, 0.15f, 8f, -100f, 100f); //Not perfect :/
                    mappedZ = PositionGenerator.Map(pointerEndPoint.z, -4.5f, 4.5f, -100f, 100f);
                }

                newDirectionVector = new Vector3(mappedX, mappedY, mappedZ) - problemPosition;
            }

            newDirectionVector.x = (int)newDirectionVector.x;
            newDirectionVector.y = (int)newDirectionVector.y;
            newDirectionVector.z = (int)newDirectionVector.z;

            currentDirectionVector = newDirectionVector;
        }

        public void SetProblemPosition(Vector3 position)
        {
            problemPosition = position;
        }

        public void SetActiveModel(int cargo)
        {
            for (int i = 0; i < modelsGO.Length; i++)
            {
                modelsGO[i].SetActive(cargo == i);
            }
        }

        public void SetIsSelected(bool newBool)
        {
            isSelected = newBool;
        }

        void UpdateEndPoint()
        {
            lineRenderer.SetPosition(1, endPoint.position);
        }
    }
}