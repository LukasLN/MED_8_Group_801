using UnityEngine;

namespace AstroMath
{
    public class Tester_PositionGenerator : MonoBehaviour
    {
        [SerializeField] GameObject spaceshipPF; //PF = Prefab
        Vector3 testPosition;


        void Update()
        {
            if(Input.GetKeyDown(KeyCode.I))
            {
                testPosition = PositionGenerator.DiscreteRingPosition(2, 5);
                //Debug.Log("New INCLUSIVE Position Generated...");
                //Debug.Log($"X: {testPosition.x}, Y: {testPosition.y}, Z: {testPosition.z}");
                SpawnSpaceship();
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                testPosition = PositionGenerator.DiscreteRingPosition(2, 5, false);
                //Debug.Log("New EXCLUSIVE Position Generated...");
                //Debug.Log($"X: {testPosition.x}, Y: {testPosition.y}, Z: {testPosition.z}");
                SpawnSpaceship();
            }
            else if (Input.GetKeyDown(KeyCode.F))
            {
                testPosition = PositionGenerator.ContinuousRingPosition(2, 5);
                //Debug.Log("New FLOAT Position Generated...");
                //Debug.Log($"X: {testPosition.x}, Y: {testPosition.y}, Z: {testPosition.z}");
                SpawnSpaceship();
            }
        }

        void SpawnSpaceship()
        {
            Instantiate(spaceshipPF, testPosition, Quaternion.identity);
        }
    }
}