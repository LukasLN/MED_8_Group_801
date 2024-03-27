using UnityEngine;

public class CircularPathGenerator : MonoBehaviour
{
    public int numWaypoints = 3; // Number of waypoints
    public float radius = 5f; // Radius of the circular path
    public Transform centerPoint; // Center point around which the circular path is created
    public GameObject waypointPrefab; // Prefab for the waypoint visual marker

    void Start()
    {
        CreateCircularPath();
    }

    void CreateCircularPath()
    {
        for (int i = 0; i < numWaypoints; i++)
        {
            // Calculate angle for each waypoint
            float angle = i * (360f / numWaypoints);

            // Calculate position of the waypoint
            float x = centerPoint.position.x + radius * Mathf.Cos(Mathf.Deg2Rad * angle);
            float y = centerPoint.position.y;
            float z = centerPoint.position.z + radius * Mathf.Sin(Mathf.Deg2Rad * angle);

            // Create waypoint GameObject
            GameObject waypoint = Instantiate(waypointPrefab, new Vector3(x, y, z), Quaternion.identity);
            waypoint.name = "Waypoint " + i;
        }
    }
}