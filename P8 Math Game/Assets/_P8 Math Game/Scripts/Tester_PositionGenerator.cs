using UnityEngine;

namespace AstroMath
{
    public class Tester_PositionGenerator : MonoBehaviour
    {
        void Update()
        {
            if(Input.GetKeyDown(KeyCode.I))
            {
                Vector3 testPosition = PositionGenerator.DiscreteRingPosition(2, 5);
                Debug.Log("New INCLUSIVE Position Generated...");
                Debug.Log($"X: {testPosition.x}, Y: {testPosition.y}, Z: {testPosition.z}");
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                Vector3 testPosition = PositionGenerator.DiscreteRingPosition(2, 5, false);
                Debug.Log("New EXCLUSIVE Position Generated...");
                Debug.Log($"X: {testPosition.x}, Y: {testPosition.y}, Z: {testPosition.z}");
            }
            else if (Input.GetKeyDown(KeyCode.F))
            {
                Vector3 testPosition = PositionGenerator.ContinuousRingPosition(2, 5);
                Debug.Log("New FLOAT Position Generated...");
                Debug.Log($"X: {testPosition.x}, Y: {testPosition.y}, Z: {testPosition.z}");
            }
        }
    }
}