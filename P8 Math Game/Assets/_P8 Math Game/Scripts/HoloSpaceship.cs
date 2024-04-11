using Unity.VisualScripting;
using UnityEngine;

namespace AstroMath
{
    public class HoloSpaceship : MonoBehaviour
    {
        [HideInInspector] public MathProblem mathProblem;

        Vector3 startPosition, directionVector;

        [SerializeField] bool isSelected;

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
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, maxDistance))
            {
                Vector3 hitPosition = hit.point;
                Debug.Log("Hit Position: " + hitPosition);
                infoPanel.rayHitPoint = hitPosition;//
                distance = Vector3.Distance(lineStartPoint.position, hitPosition);
                //Debug.DrawRay(startPoint.position, startPoint.forward * distance, Color.red);

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
