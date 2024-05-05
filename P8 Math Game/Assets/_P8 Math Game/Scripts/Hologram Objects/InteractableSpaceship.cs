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

        #region Answer
        [Header("Answers")]
        [SerializeField] Vector3 correctDirectionVector;
        [SerializeField] Vector3 currentDirectionVector;
        [SerializeField] bool correctCollisionAnswer;
        [SerializeField] bool chosenCollisionAnswer;
        [SerializeField] GameObject correctTargetGO;
        [SerializeField] int correctTScalar;
        [SerializeField] int chosenTScalar;
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
        public GameObject targetGO;
        [SerializeField] string[] tagsToLookFor;
        #endregion

        #region 3D Models and Exhaust Plumes
        [Header("3D Models")]
        [SerializeField] GameObject[] modelsGO; //GO = GameObject
        [SerializeField] GameObject[] exhaustPlumesGO;
        GameObject relevantExhaustPlumeGO;
        #endregion

        #region Movement
        [Header("Movement")]
        [SerializeField] bool isMoving;
        [SerializeField] float timeToMove;
        [SerializeField] float moveSpeed;
        #endregion

        #region Ray Pointer Parameters
        [Header("Ray Pointer Parameters")]
        public PointerShape pointerShape;
        [SerializeField] LayerMask layerMaskToIgnore;
        [SerializeField] float rayMaxDistance;
        Ray ray;
        RaycastHit hit;
        #endregion

        #region Cylinder Pointer Parameters
        [Header("Cylinder Pointer Parameters")]
        [SerializeField] GameObject cylinderGO;
        #endregion

        #region Cone Pointer Parameters
        [Header("Cone Pointer Parameters")]
        [SerializeField] GameObject coneGO;
        #endregion

        #region Line Graphics
        [Header("Line Graphics")]
        [SerializeField] Transform startPoint;
        [SerializeField] Transform endPoint;
        LineRenderer lineRenderer;
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
            infoPanel = infoPanelGO.GetComponent<InfoPanel>();

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
                if (pointerShape == PointerShape.Ray)
                {
                    ShootRay(startPoint.position, startPoint.forward); //shoots ray from spaceship in its forward direction
                }
                else if (pointerShape == PointerShape.Cylinder)
                {
                    ShootCylinder(startPoint.position, startPoint.forward);
                    //ShootRay(startPoint.position, startPoint.forward);
                }
                else if (pointerShape == PointerShape.Cone)
                {
                    ShootCone(startPoint.position, startPoint.forward);
                    //ShootRay(startPoint.position, startPoint.forward);
                }

                UpdateCurrentDirectionVector();
                infoPanel.SetCurrentDirectionVector(currentDirectionVector);
            }
            else
            {
                if(hasTarget)
                {
                    transform.LookAt(targetGO.transform);
                }
            }

            //################################################################################################
            //    > IDK WHY, BUT FOR SOME REASON, THIS PLACEMENT OF THIS IF-STATEMENT CREATES (in Laus opinion)
            //      THE FEELING OF ROTATING THE SPACESHIP :,)
            //################################################################################################
            //if (hasTarget == true) 
            //{
            //    transform.LookAt(targetGO.transform);
            //}
            //################################################################################################

            if (isMoving == true)
            {
                MoveTowardsTarget();

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

                raylength = Vector3.Distance(startPoint.position, hit.point);
                rayColor = Color.red;

                if (pointerShape == PointerShape.Ray)
                {
                    if (CheckForTarget())
                    {
                        AddNewTarget(hit.collider.gameObject);
                    }
                }

            }
            else
            {
                if (pointerShape == PointerShape.Ray)
                {
                    RemoveOldTarget();
                }
            }
            //Debug.DrawRay(startPoint.position, rayDirection * rayMaxDistance, rayColor);

            endPoint.localPosition = new Vector3(startPoint.localPosition.x,
                                            startPoint.localPosition.y,
                                            startPoint.localPosition.z + raylength);

            lineRenderer.SetPosition(1, endPoint.position);
        }

        void ShootCylinder(Vector3 position, Vector3 direction)
        {
            //cylinderGO.transform.position = position; //MIGHT BE OBSOLETE :)
            cylinderGO.transform.rotation = Quaternion.LookRotation(direction);
        }

        void ShootCone(Vector3 position, Vector3 direction)
        {
            coneGO.transform.rotation = Quaternion.LookRotation(direction);
        }

        public void AddNewTarget(GameObject target)
        {
            hasTarget = true;

            if (targetGO != null)
            {
                targetGO.GetComponent<TargetGameObject>().SetHighlightActivation(false);
            }
            targetGO = target;
            targetGO.GetComponent<TargetGameObject>().SetHighlightActivation(true);

            SetConfirmButtonsInteractability(true);
        }

        public void RemoveOldTarget()
        {
            //Debug.Log("We are not hitting anything...");

            hasTarget = false;

            if (targetGO != null)
            {
                //Debug.Log("Already have a target, so disabling its activation...");
                targetGO.GetComponent<TargetGameObject>().SetHighlightActivation(false);
            }
            targetGO = null;

            SetConfirmButtonsInteractability(false);
        }

        void SetConfirmButtonsInteractability(bool isInteractable)
        {
            if(spaceship.mathProblem.GetType() == MathProblem.Type.Collision)
            {
                infoPanel.collisionShootButton.interactable = isInteractable;
                //infoPanel.collisionFlyButton.interactable = isInteractable;
            }
            else
            {
                infoPanel.flyButton.interactable = isInteractable;
            }
        }

        public void CompareDistanceDifference()
        {

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

        void MoveTowardsTarget()
        {
            transform.LookAt(targetGO.transform);
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
            //Vector3.MoveTowards(transform.position, targetPosition, 2); //moves spaceship
        }


        public void SetTarget(GameObject targetGO) //Activated when confirm button is pressed
        {
            if(this.targetGO != null && this.targetGO.GetComponent<TargetGameObject>().isAsteroid)
            {
                this.targetGO.GetComponent<TargetGameObject>().SetHighlightActivation(false);
            }
            this.targetGO = targetGO;
            this.targetGO.GetComponent<TargetGameObject>().SetHighlightActivation(true);

            SetConfirmButtonsInteractability(true);
        }

        public void UnsetTarget(GameObject targetGO)
        {
            targetGO.GetComponent<TargetGameObject>().SetHighlightActivation(false);
            this.targetGO = correctTargetGO;
        }

        void CalculateMoveSpeed()
        {
            var distanceToTravel = Vector3.Distance(transform.position, targetGO.transform.position);
            moveSpeed = distanceToTravel / timeToMove;
        }

        public void FlyToTarget()
        {
            CalculateMoveSpeed();

            relevantExhaustPlumeGO.SetActive(true);
            infoPanelGO.SetActive(false);

            GetComponent<InteractableUnityEventWrapper>().enabled = false;
            //lineRenderer.enabled = true;

            isMoving = true;
            hasTarget = false;
            isSelected = false;
        }

        public void ShootTarget()
        {
            chosenCollisionAnswer = true;

            //Shoot
            //wait some time

            FlyToTarget();
        }

        public void CollisionFlyToTarget()
        {
            FlyToTarget();

            chosenCollisionAnswer = false;
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
                    if(chosenCollisionAnswer == correctCollisionAnswer && targetGO == correctTargetGO)
                    {
                        isCorrect = true;
                    }
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

        public void SetCorrectCollisionAnswer(bool collisionAnswer)
        {
            correctCollisionAnswer = collisionAnswer;
        }

        public void SetTargetGO(GameObject targetGO)
        {
            this.targetGO = targetGO;
        }

        public void SetCorrectTargetGO(GameObject correctTargetGO)
        {
            this.correctTargetGO = correctTargetGO;
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

        public void SetCorrectTScalar(int tScalar)
        {
            Debug.Log("Got to Interactable Spaceship!");
            correctTScalar = tScalar;
            //infoPanel.SetCorrectTScalar(tScalar);
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
                if(cargo == i)
                {
                    relevantExhaustPlumeGO = exhaustPlumesGO[i];
                }
            }
        }

        public void SetIsSelected(bool newBool)
        {

            isSelected = newBool;
            //lineRenderer.enabled = newBool;

            if (pointerShape == PointerShape.Ray) //Ray
            {
                lineRenderer.enabled = newBool;
            }
            else if (pointerShape == PointerShape.Cylinder) //Cylinder
            {
                cylinderGO.SetActive(newBool);
            }
            else if (pointerShape == PointerShape.Cone) //Cone
            {
                coneGO.SetActive(newBool);
            }
        }

        public bool GetIsSelected()
        {
            return isSelected;
        }

        void UpdateEndPoint(Vector3 position)
        {
            endPoint.position = position;
            lineRenderer.SetPosition(1, endPoint.position);
        }

        public void SetRandomRotation()
        {
            transform.rotation = Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
        }

        public void LookUp()
        {
            transform.rotation = Quaternion.Euler(-90, 0, 0);
        }

        public void LookAt(Transform target)
        {
            transform.LookAt(target);
        }
    }
}