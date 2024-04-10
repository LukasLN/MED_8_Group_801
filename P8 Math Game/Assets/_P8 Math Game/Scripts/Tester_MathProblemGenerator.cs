using UnityEngine;

namespace AstroMath
{
    public class Tester_MathProblemGenerator : MonoBehaviour
    {
        [SerializeField] GameObject holoSpaceshipPF; //PF = Prefab
        [SerializeField] float minDistance, maxDistance, discHeight;

        Collider collider;
        Bounds bounds;

        private void Start()
        {
            collider = GetComponent<MeshCollider>();
            bounds = collider.bounds;

            maxDistance = bounds.extents.x;
            discHeight = bounds.size.y;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                Vector3 randomPosition = transform.position + PositionGenerator.ContinuousDiscPosition(minDistance, maxDistance, discHeight);

                Instantiate(holoSpaceshipPF, randomPosition, Quaternion.identity);
            }
        }
    }
}
