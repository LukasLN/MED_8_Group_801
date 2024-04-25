using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace AstroMath
{
    public class MathProblemManager : MonoBehaviour
    {
        public static MathProblemManager instance;

        #region Enums
        public enum MathProblemType
        {
            Direction = 0,
            Collision = 1,
            Scale = 2,
            Random = 3
        }
        #endregion

        #region Spawning Variables
        [Header("Spawning Variables")]
        public MathProblemType mathProblemType;
        [SerializeField] bool spawnAtStart;
        public float problemSpaceMaxCoordinate;
        [SerializeField] GameObject hologramSpawnAreaGO;
        Bounds bounds;
        float minHoloDistance, maxHoloDistance;
        [SerializeField] int numberOfProblemsToCreate;
        #endregion

        #region Spaceships
        [Header("Spaceships")]
        [SerializeField] GameObject spaceshipPF; //PF = Prefab
        [SerializeField] Transform spaceshipsParentTF; //TF = Transform
        #endregion

        #region Parking Spots
        [Header("Parking Spots")]
        [SerializeField] GameObject parkingSpotPF;
        [SerializeField] Transform parkingSpotsParentTF;
        [SerializeField] int numberOfDummyParkingSpots;
        #endregion

        [SerializeField] List<MathProblem> mathProblems;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            #region Setting Default Spawn Area Values
            bounds = hologramSpawnAreaGO.GetComponent<Collider>().bounds;
            minHoloDistance = -bounds.extents.x;
            maxHoloDistance = bounds.extents.x;
            #endregion

            if (spawnAtStart == true)
            {
                //CreateNumberOfProblems(numberOfProblemsToCreate);
            }
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.C))
            {
                CreateMathProblem();
            }

            if(Input.GetKeyDown(KeyCode.R))
            {
                //RemoveProblem(0);
            }
        }

        public void CreateMathProblem()
        {
            DestroyMathProblem(0);

            FixedPositionsContainer.instance.CreateNewSampleParkingPositions(FixedPositionsContainer.instance.numberOfSamplesToCreate);
            FixedPositionsContainer.instance.CreateNewSampleAsteroidPositions(FixedPositionsContainer.instance.numberOfSamplesToCreate);

            MathProblem newMathProblem;
            if (mathProblemType == MathProblemType.Random)
            {
                newMathProblem = MathProblemGenerator.GenerateMathProblem(0, problemSpaceMaxCoordinate);
            }
            else
            {
                newMathProblem = MathProblemGenerator.GenerateMathProblem(0, problemSpaceMaxCoordinate, false, (int)mathProblemType);
            }

            mathProblems.Add(newMathProblem);

            SpawnSpaceship(newMathProblem);
            SpawnParkingSpot(newMathProblem);

            for (int i = 0; i < numberOfDummyParkingSpots; i++)
            {
                var sample = FixedPositionsContainer.instance.TakeSampleObject();
                SpawnParkingSpot(sample.position);
            }
        }

        void DestroyMathProblem(int index)
        {
            if(mathProblems.Count == 0) { return; }

            mathProblems.RemoveAt(index);

            foreach (Transform child in spaceshipsParentTF)
            {
                Destroy(child.gameObject);
            }

            foreach (Transform child in parkingSpotsParentTF)
            {
                Destroy(child.gameObject);
            }
        }

        void SpawnSpaceship(MathProblem mathProblem)
        {
            Vector3 spawnPosition = MapProblemSpaceToCookieSpace(mathProblem.spaceshipPosition);

            GameObject newSpaceship = Instantiate(spaceshipPF, spawnPosition, Quaternion.identity, spaceshipsParentTF);
            newSpaceship.name = "Spaceship " + mathProblem.spaceshipPosition;

            newSpaceship.transform.GetChild(1).gameObject.GetComponent<HoloSpaceship>().SetMathProblem(mathProblem);
            newSpaceship.transform.GetChild(1).gameObject.GetComponent<HoloSpaceship>().UpdateGraphics();
            newSpaceship.transform.GetChild(1).gameObject.GetComponent<HoloSpaceship>().UpdatePuzzleInformation();
        }

        void SpawnParkingSpot(MathProblem mathProblem)
        {
            Vector3 spawnPosition = MapProblemSpaceToCookieSpace(mathProblem.targetPosition);

            GameObject newParkingSpot = Instantiate(parkingSpotPF, spawnPosition, Quaternion.identity, parkingSpotsParentTF);
            newParkingSpot.name = "Parking Spot " + mathProblem.targetPosition;

            newParkingSpot.GetComponent<HoloParkingSpot>().SetMathProblem(mathProblem);
        }

        void SpawnParkingSpot(Vector3 position)
        {
            Vector3 spawnPosition = MapProblemSpaceToCookieSpace(position);

            GameObject newParkingSpot = Instantiate(parkingSpotPF, spawnPosition, Quaternion.identity, parkingSpotsParentTF);
            newParkingSpot.name = "Parking Spot " + position;
        }

        Vector3 MapProblemSpaceToCookieSpace(Vector3 problemPosition)
        {
            Vector2 spaceBounds = new Vector2(-problemSpaceMaxCoordinate, problemSpaceMaxCoordinate);

            float x = PositionGenerator.Map(problemPosition.x, spaceBounds.x, spaceBounds.y, minHoloDistance, maxHoloDistance);
            float y = PositionGenerator.Map(problemPosition.y, spaceBounds.x, spaceBounds.y,
                                            hologramSpawnAreaGO.transform.position.y - (bounds.size.y / 2),
                                            hologramSpawnAreaGO.transform.position.y + (bounds.size.y / 2));
            float z = PositionGenerator.Map(problemPosition.z, spaceBounds.x, spaceBounds.y, minHoloDistance, maxHoloDistance);

            return new Vector3(x, y, z);
        }

        /*
        public void CreateNumberOfProblems(int numberOfProblems)
        {
            RemoveAllProblems();

            FixedPositionsContainer.instance.CreateNewSampleParkingPositions(numberOfProblems);
            FixedPositionsContainer.instance.CreateNewSampleAsteroidPositions(numberOfProblems);

            for (int i = 0; i < numberOfProblems; i++)
            {
                CreateProblem();
            }
        }

        void CreateProblem()
        {
            MathProblem newMathProblem;
            if (mathProblemType == MathProblemType.Random)
            {
                newMathProblem = MathProblemGenerator.GenerateMathProblem(minDistance, maxDistance);
            }
            else
            {
                newMathProblem = MathProblemGenerator.GenerateMathProblem(minDistance, maxDistance, false, (int)mathProblemType);
            }

            mathProblems.Add(newMathProblem);

            HoloObjectManager.instance.SpawnHoloObjects(newMathProblem);
        }

        void RemoveAllProblems()
        {
            var count = mathProblems.Count;

            for (int i = 0; i < count; i++)
            {
                RemoveProblem(0);
            }
        }

        void RemoveProblem(int index)
        {
            HoloObjectManager.instance.DespawnHoloObjects(index);
            mathProblems.RemoveAt(index);
        }*/
    }
}