using Oculus.Interaction;
using Unity.VisualScripting;
using UnityEngine;

namespace AstroMath
{
    public class HoloSpaceship : MonoBehaviour
    {
        [HideInInspector] public MathProblem mathProblem;

        Vector3 startPosition, directionVector;

        [SerializeField] bool isSelected;
        [SerializeField] bool hasSavedRotation;

        [SerializeField] GameObject[] modelsGO; //GO = GameObject

        #region Info Panel
        [Header("Info Panel")]
        [SerializeField] GameObject infoPanelGO; //GO = GameObject
        [HideInInspector] public InfoPanel infoPanel;
        #endregion

        #region Line
        [Header("Line")]
        [SerializeField] GameObject lineGO; //GO = GameObject
        LineRenderer lineRenderer;
        [SerializeField] Transform lineStartPoint;
        [SerializeField] float maxDistance;
        RaycastHit hit;
        #endregion

        private void Awake()
        {
            lineRenderer = lineGO.GetComponent<LineRenderer>();
            lineRenderer.SetPosition(0, lineStartPoint.localPosition);
            infoPanel = infoPanelGO.GetComponent<InfoPanel>();
        }

        private void Start()
        {
            startPosition = transform.position;
        }

        private void Update()
        {
            directionVector = transform.forward;
            
            if(isSelected == true)
            {
                DrawLine();
                //UpdateGraphics();
            }

            if(hasSavedRotation == true)
            {
                transform.LookAt(hit.transform.position);
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

        public void DrawLine()
        {
            #region Shooting a ray to get distance to edge
            float distance = maxDistance;

            Ray ray = new Ray(lineStartPoint.position, lineStartPoint.forward);

            if (Physics.Raycast(ray, out hit, maxDistance))
            {
                Vector3 hitPosition = hit.point;

                #region Debugging to figure out mapping issue

                Debug.Log("BEFORE Hit Position: " + hitPosition);
                Debug.Log("BEFORE Spaceship Position: " + transform.position);

                float x = PositionGenerator.Map(hitPosition.x, -4.5f, 4.5f, -100, 100);
                float y = PositionGenerator.Map(hitPosition.y, -0.5f + 1.5f, 0.5f + 1.5f, -100, 100);
                float z = PositionGenerator.Map(hitPosition.z, -4.5f, 4.5f, -100, 100);

                Debug.Log("AFTER Hit Position: " + new Vector3(x,y,z));
                Debug.Log("AFTER Spaceship Position: " + mathProblem.spaceshipPosition);

                #endregion

                infoPanel.rayHitPoint = hitPosition; //
                distance = Vector3.Distance(lineStartPoint.position, hitPosition);
                //Debug.DrawRay(startPoint.position, startPoint.forward * distance, Color.red);

                if (hit.collider.gameObject.tag == "Parking")
                {
                    Debug.Log("Collided with PARKING!");

                    hasSavedRotation = true;

                    //Debug.Log("Saved Rotation: " + savedRotation);
                    //GetComponent<OneGrabFreeTransformer>().transform.rotation = savedRotation;
                    //var grabRotation = GetComponent<OneGrabFreeTransformer>().transform.rotation;
                    //Debug.Log("OneGrabFreeTransformer Rotation: " + grabRotation);
                }
                else
                {
                    hasSavedRotation = false;
                }

            }
            #endregion

            Vector3 endPosition = new Vector3(lineStartPoint.localPosition.x, lineStartPoint.localPosition.y, lineStartPoint.localPosition.z + distance);
            lineRenderer.SetPosition(1, endPosition);
        }

        public void SetMathProblem(MathProblem newMathProblem)
        {
            mathProblem = newMathProblem;
            infoPanel.mathProblem = newMathProblem;
        }

        public void SetIsSelected(bool newBool)
        {
            isSelected = newBool;
            infoPanel.SetIsSelected(newBool);
        }
    }
}