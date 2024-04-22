using Oculus.Interaction;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

namespace AstroMath
{
    public class HoloSpaceship : MonoBehaviour
    {
        public MathProblem mathProblem;
        public int problemID;

        [HideInInspector] public Vector3 directionVector;

        [SerializeField] bool isSelected;
        [SerializeField] bool hasTarget;
        public GameObject targetGO;

        [SerializeField] GameObject[] modelsGO; //GO = GameObject

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
        [SerializeField] float maxDistance;
        [SerializeField] LayerMask layerMaskToIgnore;
        RaycastHit hit;
        #endregion

        private void Awake()
        {
            lineRenderer = lineGO.GetComponent<LineRenderer>();
            lineRenderer.SetPosition(0, lineStartPoint.localPosition);
            infoPanel = infoPanelGO.GetComponent<InfoPanel>();

            OVRCameraRig cameraRig = FindObjectOfType<OVRCameraRig>();
            infoPanelMovement = infoPanelGO.GetComponent<InfoPanelMovement>();
            infoPanelMovement.player = cameraRig.centerEyeAnchor;
        }

        private void Update()
        {
            if(isSelected == true)
            {
                DrawLine();
                UpdateDirectionVector();
                //UpdateGraphics();
            }

            if(hasTarget == true)
            {
                transform.LookAt(targetGO.transform);
                //Debug.Log("Hit Transform Position: " + hit.transform.position);
                //infoPanel.
            }
        }

        public void UpdateGraphics()
        {
            for (int i = 0; i < modelsGO.Length; i++)
            {
                //Debug.Log("i: " + i + ", Math Problem Type: " + (int)mathProblem.cargo);
                modelsGO[i].SetActive((int)mathProblem.cargo == i);
            }

            infoPanel.UpdateGraphics();
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
                }
                else
                {
                    hasTarget = false;
                    targetGO = null;
                }
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
                newDirectionVector = targetGO.GetComponent<HoloParkingSpot>().mathProblem.targetPosition -
                                                                              mathProblem.spaceshipPosition;
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

                newDirectionVector = new Vector3(mappedX, mappedY, mappedZ) - mathProblem.spaceshipPosition;
            }

            newDirectionVector.x = (int)newDirectionVector.x;
            newDirectionVector.y = (int)newDirectionVector.y;
            newDirectionVector.z = (int)newDirectionVector.z;

            infoPanel.directionVector = newDirectionVector;
            directionVector = newDirectionVector;
        }

        public void SetMathProblem(MathProblem newMathProblem)
        {
            mathProblem = newMathProblem;
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