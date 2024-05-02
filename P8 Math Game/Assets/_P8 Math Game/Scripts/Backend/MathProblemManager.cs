using System.Collections.Generic;
using UnityEngine;

namespace AstroMath
{
    public class MathProblemManager : MonoBehaviour
    {
        public static MathProblemManager instance;
        [SerializeField] GameObject mathProblemPF;
        [SerializeField] MathProblem currentMathProblem;

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
        public Bounds bounds;
        [SerializeField] int numberOfProblemsToCreate;
        #endregion

        #region Spaceship Spawning Coordinates
        [Header("Spaceship Spawning Coordinates")]
        public float minDirectionDistance;
        public float maxDirectionDistance;
        public float minCollisionDistance;
        public float maxCollisionDistance;
        public float minScaleDistance;
        public float maxScaleDistance;
        #endregion

        #region Parking and Asteroid Spawning Coordinates
        [Header("Parking and Asteroid Spawning Coordinates")]
        public float minParkingDistance;
        public float maxParkingDistance;
        public float minAsteroidDistance;
        public float maxAsteroidDistance;
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

        #region Asteroids
        [Header("Asteroids")]
        [SerializeField] GameObject asteroidPF;
        [SerializeField] Transform asteroidsParentTF;
        [SerializeField] int numberOfDummyAsteroids;
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
            //minDirectionDistance = -bounds.extents.x;
            //maxDirectionDistance = bounds.extents.x;
            #endregion

            if (spawnAtStart == true)
            {
                //CreateNumberOfProblems(numberOfProblemsToCreate);
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                CreateMathProblem();
            }

            //if(Input.GetKeyDown(KeyCode.R))
            //{
            //    //RemoveProblem(0);
            //}
        }

        public void CreateMathProblem()
        {
            if(currentMathProblem != null)
            {
                Destroy(currentMathProblem.gameObject);
            }

            GameObject newMathProblem = Instantiate(mathProblemPF, Vector3.zero, Quaternion.identity);
            currentMathProblem = newMathProblem.GetComponent<MathProblem>();
            currentMathProblem.GetComponent<MathProblem>().New();
        }

        //public void CreateMathProblem()
        //{
        //    DestroyMathProblem(0); //Destroying the previous math problem
            
        //    //> Creating new sample positions for parking spots and asteroids
        //    FixedPositionsContainer.instance.CreateSampleParkingTargets();
        //    FixedPositionsContainer.instance.CreateSampleAsteroidTargets();

        //    #region Generating new MATH PROBLEM
        //    MathProblem newMathProblem;
        //    if (mathProblemType == MathProblemType.Random)
        //    {
        //        newMathProblem = MathProblemGenerator.GenerateMathProblem(0, problemSpaceMaxCoordinate);
        //    }
        //    else
        //    {
        //        newMathProblem = MathProblemGenerator.GenerateMathProblem(0, problemSpaceMaxCoordinate, false, (int)mathProblemType);
        //    }

        //    mathProblems.Add(newMathProblem);

        //    SpawnSpaceship(newMathProblem);
        //    #endregion

        //    //switch (newMathProblem.m_type)
        //    //{
        //    //    case MathProblem.Type.Direction:
        //    //        #region Problem Type 1 (Direction)
        //    //        SpawnParkingSpot(newMathProblem);

        //    //        for (int i = 0; i < numberOfDummyParkingSpots; i++)
        //    //        {
        //    //            var dummyMathProblem = MathProblemGenerator.GenerateMathProblem(0, problemSpaceMaxCoordinate, false, (int)mathProblemType);

        //    //            SpawnParkingSpot(dummyMathProblem);
        //    //        }
        //    //        #endregion
        //    //        break;
        //    //    case MathProblem.Type.Collision:
        //    //        #region Problem Type 2 (Collision)
        //    //        SpawnAsteroid(newMathProblem);

        //    //        for (int i = 0; i < numberOfDummyAsteroids; i++)
        //    //        {
        //    //            var dummyMathProblem = MathProblemGenerator.GenerateMathProblem(0, problemSpaceMaxCoordinate, false, (int)mathProblemType);

        //    //            SpawnAsteroid(dummyMathProblem);
        //    //        }
        //    //        #endregion
        //    //        break;
        //    //    case MathProblem.Type.Scale:
        //    //        #region Problem Type 3 (Scale)
        //    //        SpawnParkingSpot(newMathProblem);
        //    //        #endregion
        //    //        break;
        //    //}
            
        //}

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
            //Vector3 spawnPosition = MapProblemSpaceToCookieSpace(mathProblem.spaceshipPosition);

            //GameObject newSpaceship = Instantiate(spaceshipPF, spawnPosition, Quaternion.identity, spaceshipsParentTF);
            ////newSpaceship.name = "Spaceship " + mathProblem.spaceshipPosition;

            //newSpaceship.transform.GetChild(1).gameObject.GetComponent<Spaceship>().SetMathProblem(mathProblem);
            //newSpaceship.transform.GetChild(1).gameObject.GetComponent<Spaceship>().UpdateInteractions();
            //newSpaceship.transform.GetChild(1).gameObject.GetComponent<Spaceship>().UpdateGraphics();
            //newSpaceship.transform.GetChild(1).gameObject.GetComponent<Spaceship>().UpdatePuzzleInformation();
        }

        void SpawnParkingSpot(MathProblem mathProblem)
        {
            //Vector3 spawnPosition = MapProblemSpaceToCookieSpace(mathProblem.targetPosition);

            //GameObject newParkingSpot = Instantiate(parkingSpotPF, spawnPosition, Quaternion.identity, parkingSpotsParentTF);
            //newParkingSpot.name = "Parking Spot " + mathProblem.targetPosition;

            //newParkingSpot.GetComponent<ParkingSpot>().SetMathProblem(mathProblem);
        }

        void SpawnAsteroid(MathProblem mathProblem)
        {
            //Vector3 spawnPosition = MapProblemSpaceToCookieSpace(mathProblem.targetPosition);

            //GameObject newAsteroid = Instantiate(asteroidPF, spawnPosition, Quaternion.identity, asteroidsParentTF);
            //newAsteroid.name = "Asteroid " + mathProblem.targetName + " " + mathProblem.targetPosition;

            //newAsteroid.GetComponent<HoloAsteroid>().SetMathProblem(mathProblem);
        }

        public Vector3 MapProblemSpaceToCookieSpace(Vector3 problemPosition)
        {
            Vector2 spaceBounds = new Vector2(-maxDirectionDistance, maxDirectionDistance);
            Vector2 cookieBounds = new Vector2(-bounds.extents.x, bounds.extents.x);

            float x = PositionGenerator.Map(problemPosition.x, spaceBounds.x, spaceBounds.y, cookieBounds.x, cookieBounds.y);
            float y = PositionGenerator.Map(problemPosition.y, spaceBounds.x, spaceBounds.y,
                                            hologramSpawnAreaGO.transform.position.y - (bounds.size.y / 2),
                                            hologramSpawnAreaGO.transform.position.y + (bounds.size.y / 2));
            float z = PositionGenerator.Map(problemPosition.z, spaceBounds.x, spaceBounds.y, cookieBounds.x, cookieBounds.y);

            return new Vector3(x, y, z);
        }
    }
}