using System.Collections.Generic;
using UnityEngine;

namespace AstroMath
{
    public struct HoloObjectPair
    {
        public HoloSpaceship spaceship;
        public HoloParkingSpot parkingSpot;
    }

    public class HoloObjectManager : MonoBehaviour
    {
        public static HoloObjectManager instance;

        [SerializeField] GameObject holoSpawnArea;
        Bounds bounds;
        float minHoloDistance, maxHoloDistance;

        [SerializeField] GameObject holoSpaceshipPF; //PF = Prefab
        [SerializeField] Transform holoSpaceshipParentTF; //TF = Transform
        [SerializeField] List<GameObject> holoSpaceships;

        [SerializeField] GameObject holoParkingSpotPF;
        [SerializeField] Transform holoParkingSpotParentTF;
        [SerializeField] List<GameObject> holoParkingSpots;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            bounds = holoSpawnArea.GetComponent<Collider>().bounds;
            minHoloDistance = -bounds.extents.x;
            maxHoloDistance = bounds.extents.x;
        }

        public void SpawnHoloObjects(MathProblem mathProblem)
        {
            #region Spaceship
            //Spawn spaceship at mapped position
            Vector3 originalPosition = mathProblem.spaceshipPosition;

            Vector2 spaceBounds = new Vector2(-MathProblemManager.instance.maxDistance, MathProblemManager.instance.maxDistance);

            float x = PositionGenerator.Map(originalPosition.x, spaceBounds.x, spaceBounds.y, minHoloDistance, maxHoloDistance);
            float y = PositionGenerator.Map(originalPosition.y, spaceBounds.x, spaceBounds.y,
                                            holoSpawnArea.transform.position.y - (bounds.size.y / 2),
                                            holoSpawnArea.transform.position.y + (bounds.size.y / 2));
            float z = PositionGenerator.Map(originalPosition.z, spaceBounds.x, spaceBounds.y, minHoloDistance, maxHoloDistance);

            Vector3 mappedPosition = new Vector3(x, y, z);

            GameObject newSpaceship = Instantiate(holoSpaceshipPF, mappedPosition, Quaternion.identity, holoSpaceshipParentTF);
            newSpaceship.name = "Holo Spaceship " + mappedPosition;

            newSpaceship.transform.GetChild(0).gameObject.GetComponent<HoloSpaceship>().SetMathProblem(mathProblem);
            newSpaceship.transform.GetChild(0).gameObject.GetComponent<HoloSpaceship>().UpdateGraphics();

            holoSpaceships.Add(newSpaceship);
            #endregion

            #region Parking Spot
            originalPosition = mathProblem.targetPosition;

            x = PositionGenerator.Map(originalPosition.x, spaceBounds.x, spaceBounds.y, minHoloDistance, maxHoloDistance);
            y = PositionGenerator.Map(originalPosition.y, spaceBounds.x, spaceBounds.y,
                                      holoSpawnArea.transform.position.y - (bounds.size.y / 2),
                                      holoSpawnArea.transform.position.y + (bounds.size.y / 2));
            z = PositionGenerator.Map(originalPosition.z, spaceBounds.x, spaceBounds.y, minHoloDistance, maxHoloDistance);

            mappedPosition = new Vector3(x, y, z);

            GameObject newParkingSpot = Instantiate(holoParkingSpotPF, mappedPosition, Quaternion.identity, holoParkingSpotParentTF);
            newParkingSpot.name = "Holo Parking Spot " + mappedPosition;
            newParkingSpot.GetComponent<HoloParkingSpot>().SetMathProblem(mathProblem);

            holoParkingSpots.Add(newParkingSpot);
            #endregion
        }

        public void DespawnHoloObjects(int index)
        {
            var holoSpaceship   = holoSpaceships[index];
            var holoParkingSpot = holoParkingSpots[index];

            Destroy(holoSpaceship);
            Destroy(holoParkingSpot);

            holoSpaceships.RemoveAt(index);
            holoParkingSpots.RemoveAt(index);
        }
    }
}