using System;
using UnityEditor.PackageManager;
using UnityEngine;

namespace AstroMath
{
    public class LineController : MonoBehaviour
    {
        LineRenderer lineRenderer;
        [SerializeField] Transform startPoint;
        [SerializeField] float maxDistance;

        private void Awake()
        {
            lineRenderer = GetComponent<LineRenderer>();
        }

        void Update()
        {
            lineRenderer.SetPosition(0, startPoint.localPosition);

            #region Shooting a ray to get distance to edge
            float distance = maxDistance;

            Ray ray = new Ray(startPoint.position, startPoint.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, maxDistance))
            {
                Vector3 hitPosition = hit.point;
                distance = Vector3.Distance(startPoint.position, hitPosition);
                Debug.DrawRay(startPoint.position, startPoint.forward * distance, Color.red);
                
            }
            #endregion

            Vector3 endPosition = new Vector3(startPoint.localPosition.x, startPoint.localPosition.y, startPoint.localPosition.z + distance);
            lineRenderer.SetPosition(1, endPosition);
        }
    }
}