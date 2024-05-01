using UnityEngine;

namespace AstroMath
{
    public class LineController : MonoBehaviour
    {
        public bool isVisible;

        #region Ray Parameters
        [Header("Ray Parameters")]
        Ray ray;
        RaycastHit hit;
        [SerializeField] float rayMaxDistance;
        [SerializeField] LayerMask layerMaskToIgnore;
        #endregion

        #region Line Graphics
        [Header("Line Graphics")]
        [SerializeField] LineRenderer lineRenderer;
        [SerializeField] Transform startPoint;
        [SerializeField] Transform endPoint;
        #endregion

        private void Start()
        {
            lineRenderer.SetPosition(0, startPoint.position);
            lineRenderer.SetPosition(1, endPoint.position);
        }

        private void Update()
        {
            if (isVisible)
            {
                ray = new Ray(startPoint.position, transform.forward);

                //Color rayColor = Color.green;
                if (Physics.Raycast(ray, out hit, rayMaxDistance, ~layerMaskToIgnore))
                {
                    //rayColor = Color.red;
                    endPoint.position = hit.point;
                }
                else
                {
                    endPoint.position = transform.forward * rayMaxDistance;
                }

                lineRenderer.SetPosition(1, endPoint.position);
                //Debug.DrawRay(ray.origin, ray.direction * rayMaxDistance, rayColor);
            }
        }

        public void SetIsVisible(bool newBool)
        {
            isVisible = newBool;
        }

        void UpdateEndPoint()
        {
            lineRenderer.SetPosition(1, endPoint.position);
        }


        
    }
}