using UnityEngine;

namespace AstroMath
{
    public class HoloSpaceship : MonoBehaviour
    {
        Vector3 startPosition, directionVector;

        #region Info Panel
        [Header("Info Panel")]
        [SerializeField] GameObject infoPanelGO; //GO = GameObject
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
        }

        private void Start()
        {
            startPosition = transform.position;
        }

        private void Update()
        {
            directionVector = transform.forward;
            Debug.Log($"Direction Vector: {directionVector}");
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
                distance = Vector3.Distance(lineStartPoint.position, hitPosition);
                //Debug.DrawRay(startPoint.position, startPoint.forward * distance, Color.red);

            }
            #endregion

            Vector3 endPosition = new Vector3(lineStartPoint.localPosition.x, lineStartPoint.localPosition.y, lineStartPoint.localPosition.z + distance);
            lineRenderer.SetPosition(1, endPosition);
        }
    }
}
