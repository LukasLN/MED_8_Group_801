using UnityEngine;

namespace AstroMath
{
    public class Tester_PositionGenerator : MonoBehaviour
    {
        [SerializeField] GameObject spaceshipPF, asteroidPF; //PF = Prefab
        Vector3 spaceshipPosition, asteroidPosition;
        [SerializeField] float spaceshipMin, spaceshipMax, asteroidMin, asteroidMax, iterations;

        void Update()
        {
            if(Input.GetKeyDown(KeyCode.D))
            {
                for(int i = 0; i < iterations; i++)
                {
                    asteroidPosition = PositionGenerator.DiscreteRingPosition((int)asteroidMin, (int)asteroidMax);
                    spaceshipPosition = PositionGenerator.DiscreteRingPosition((int)spaceshipMin, (int)spaceshipMax);

                    //Debug.Log("New DISCRETE RING Position Generated...");
                    //Debug.Log($"X: {spaceshipPosition.x}, Y: {spaceshipPosition.y}, Z: {spaceshipPosition.z}");

                    SpawnAsteroid();
                    SpawnSpaceship();
                }
            }
            else if (Input.GetKeyDown(KeyCode.C))
            {
                for (int i = 0; i < iterations; i++)
                {
                    asteroidPosition = PositionGenerator.ContinuousRingPosition((int)asteroidMin, (int)asteroidMax);
                    spaceshipPosition = PositionGenerator.ContinuousRingPosition((int)spaceshipMin, (int)spaceshipMax);

                    //Debug.Log("New DISCRETE RING Position Generated...");
                    //Debug.Log($"X: {spaceshipPosition.x}, Y: {spaceshipPosition.y}, Z: {spaceshipPosition.z}");

                    SpawnAsteroid();
                    SpawnSpaceship();
                }
            }
        }

        void SpawnSpaceship()
        {
            Instantiate(spaceshipPF, spaceshipPosition, Quaternion.LookRotation(asteroidPosition - spaceshipPosition));
        }

        void SpawnAsteroid()
        {
            Instantiate(asteroidPF, asteroidPosition, Quaternion.Euler(Random.Range(0f,360f), Random.Range(0f, 360f), Random.Range(0f, 360f)));
        }
    }
}