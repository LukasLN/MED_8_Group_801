using Unity.Mathematics;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

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

        #region IDK
        [Header("IDK")]
        [SerializeField] GameObject[] modelsGO; //GO = GameObject
        [SerializeField] bool isSelected;
        [SerializeField] float timeToMove;
        [SerializeField] bool isMoving;
        [SerializeField] float moveSpeed;
        public Vector3 targetPosition;
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

        private void Start()
        {
            lineRenderer = GetComponent<LineRenderer>();

            lineRenderer.SetPosition(0, startPoint.position);
        }

        void Update()
        {
            if (isMoving == true)
            {
                MoveForward();

                if (Vector3.Distance(transform.position, targetPosition) < 0.1f) //if we are not at the target position
                {
                    isMoving = false;

                    GetComponent<Spaceship>().ShowResult();
                }
            }

            if (isSelected)
            {
                float raylength = rayMaxDistance;
                Color rayColor = Color.green;

                switch (pointerShape)
                {
                    case PointerShape.Ray:
                        Debug.Log("Ray...");
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

                                if (Physics.Raycast(startPoint.position, rayDirection, out hit, rayMaxDistance))
                                {
                                    Debug.Log("Hit object: " + hit.collider.gameObject.name);
                                    raylength = Vector3.Distance(startPoint.position, hit.point);
                                    rayColor = Color.red;
                                }
                                //Debug.DrawRay(startPoint.position, rayDirection * rayMaxDistance, rayColor);
                            }
                        }
                        #endregion
                        break;
                }

                ray = new Ray(startPoint.position, startPoint.forward);

                if (Physics.Raycast(ray, out hit, rayMaxDistance, ~layerMaskToIgnore))
                {
                    raylength = Vector3.Distance(startPoint.position, hit.point);
                    rayColor = Color.red;
                }
                //Debug.DrawRay(ray.origin, ray.direction * rayMaxDistance, rayColor);

                endPoint.localPosition = new Vector3(startPoint.localPosition.x,
                                                     startPoint.localPosition.y,
                                                     startPoint.localPosition.z + raylength);

                lineRenderer.SetPosition(0, startPoint.position);
                lineRenderer.SetPosition(1, endPoint.position);
            }
        }

        void MoveForward()
        {
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
            //Vector3.MoveTowards(transform.position, targetPosition, 2); //moves spaceship
        }


        public void SetTarget(GameObject targetGO) //Activated when confirm button is pressed
        {
            targetPosition = targetGO.transform.position;

            var distanceToTravel = Vector3.Distance(transform.position, targetPosition);
            moveSpeed = distanceToTravel / timeToMove;

            //transform.position = Vector3.MoveTowards(interactableSpaceshipGO.transform.position, target.transform.position, step); //moves spaceship

            isMoving = true;
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