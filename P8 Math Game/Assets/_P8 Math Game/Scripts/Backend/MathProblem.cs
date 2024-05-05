using Unity.VisualScripting;
using UnityEngine;

namespace AstroMath
{
    public class MathProblem : MonoBehaviour
    {
        public enum Type
        {
            Direction = 0,
            Collision = 1,
            Scale = 2
        }
        public enum Cargo
        {
            Water = 0,
            Food = 1,
            Fuel = 2
        }

        #region Characteristics
        [Header("Characteristics")]
        [SerializeField] Type m_type;
        [SerializeField] Cargo m_cargo;

        [SerializeField]
        static string[,] m_pinCodes = new string[3, 3]
        {
        {"9264", "5738", "3487"},
        {"8093", "6152", "4276"},
        {"7029", "1845", "2361"}
        };
        [SerializeField] string m_pinCode;

        [SerializeField] Vector3 m_spaceshipPosition;
        [SerializeField] Vector3 m_targetPosition;
        [SerializeField] string m_targetName;

        [SerializeField] Vector3 m_directionSolution;
        [SerializeField] bool m_collisionSolution;
        [SerializeField] int m_scaleSolution;
        #endregion

        #region Holo Objects
        [Header("Holo Objects")]
        [SerializeField] GameObject m_spaceshipGO;
        [Tooltip("Could be a Parking Spot or an Asteroid")]
        [SerializeField] GameObject m_targetGO;
        [SerializeField] Transform m_parkingSpotsParentTF; //TF = Transform
        [SerializeField] Transform m_asteroidsParentTF;
        [SerializeField] GameObject faultyAsteroidPF; //PF = Prefab
        #endregion

        public void New(bool random = true, Type type = 0)
        {
            GenerateNewCharacteristics(random, type);
            UpdateHoloObjects();
        }

        public void GenerateNewCharacteristics(bool random = true, Type type = 0)
        {
            //Debug.Log("Generating new Characteristics for Math Problem...");

            ClearCharacteristics();

            FixedPositionsContainer.instance.CreateSampleParkingTargets();
            FixedPositionsContainer.instance.CreateSampleAsteroidTargets();

            #region Setting Type and Cargo
            if(random)
            {
                m_type = RandomType();
            }
            else
            {
                m_type = type;
            }
            m_cargo = RandomCargo();
            #endregion

            #region Setting Pin Code
            m_pinCode = m_pinCodes[(int)m_type, (int)m_cargo];
            #endregion

            #region Setting Spaceship Position
            switch (m_type)
            {
                case Type.Direction:
                    m_spaceshipPosition = PositionGenerator.DiscreteRingPosition(MathProblemManager.instance.minDirectionDistance,
                                                                                 MathProblemManager.instance.maxDirectionDistance);
                    break;
                case Type.Collision:
                    m_spaceshipPosition = PositionGenerator.DiscreteRingPosition(MathProblemManager.instance.minCollisionDistance,
                                                                                 MathProblemManager.instance.maxCollisionDistance);
                    break;
                case Type.Scale:
                    m_spaceshipPosition = PositionGenerator.DiscreteRingPosition(MathProblemManager.instance.minScaleDistance,
                                                                                 MathProblemManager.instance.maxScaleDistance);
                    break;
            }
            #endregion

            #region Setting Target and Name
            HoloTarget target;
            var targetIndex = 0;
            switch (m_type)
            {
                case Type.Direction:
                    targetIndex = 0;
                    break;
                case Type.Collision:
                    targetIndex = 1;
                    break;
                case Type.Scale:
                    targetIndex = 1;
                    break;
            }

            target = FixedPositionsContainer.instance.TakeSampleTarget(targetIndex);
            m_targetPosition = target.position;
            m_targetName = target.name;
            //Debug.Log("Target Name: " + m_targetName + ", Target Position: " + m_targetPosition);
            #endregion

            #region Setting Solutions
            switch (m_type)
            {
                case Type.Direction:
                    m_directionSolution = m_targetPosition - m_spaceshipPosition;
                    m_collisionSolution = true;
                    m_scaleSolution = 1;
                    break;
                case Type.Collision:
                    var chance = Random.Range(0, 2);
                    if (chance == 0)
                    {
                        m_collisionSolution = false;
                        m_scaleSolution = 1;
                        var faultySample = FixedPositionsContainer.instance.TakeSampleTarget(1);
                        m_directionSolution = faultySample.position - m_spaceshipPosition;
                    }
                    else
                    {
                        m_collisionSolution = true;
                        m_scaleSolution = GenerateTScalar(false);
                    }
                    break;
                case Type.Scale:
                    m_collisionSolution = true;
                    m_scaleSolution = GenerateTScalar(false);
                    break;
            }

            #endregion
        }

        void ClearCharacteristics()
        {
            if (m_type == Type.Collision && m_collisionSolution == false && m_targetGO != null)
            {
                Destroy(m_targetGO);
            }

            m_type = Type.Direction;
            m_cargo = Cargo.Water;
            m_spaceshipPosition = Vector3.zero;
            m_targetPosition = Vector3.zero;
            m_targetName = "";
            m_directionSolution = Vector3.zero;
            m_collisionSolution = false;
            m_scaleSolution = 0;
            m_pinCode = "";
        }

        public void UpdateHoloObjects()
        {
            #region Enabling/Disabling Target Objects (Parking Spots or Asteroids)
            if (m_type == Type.Direction)
            {
                m_targetGO = m_parkingSpotsParentTF.GetChild(0).gameObject;
            }
            else if(m_type == Type.Collision || m_type == Type.Scale)
            {
                m_targetGO = m_asteroidsParentTF.GetChild(0).gameObject;
            }

            m_parkingSpotsParentTF.gameObject.SetActive(m_type == Type.Direction);
            m_asteroidsParentTF.gameObject.SetActive(m_type == Type.Collision || m_type == Type.Scale);
            #endregion

            #region Setting Graphics of Spaceship and Info Panel
            m_spaceshipGO.GetComponent<Spaceship>().UpdateGraphics((int)m_type, (int)m_cargo);
            #endregion

            #region Setting Spaceship and Info Panel Information
            m_spaceshipGO.GetComponent<Spaceship>().UpdateInteractions();
            m_spaceshipGO.GetComponent<Spaceship>().UpdatePuzzleInformation(this);
            m_spaceshipGO.GetComponent<Spaceship>().SetCorrectDirectionVector(m_directionSolution);
            m_spaceshipGO.GetComponent<Spaceship>().SetCorrectCollisionAnswer(m_collisionSolution);
            //Debug.Log("Scale Solution: " + m_scaleSolution);
            //m_spaceshipGO.GetComponent<Spaceship>().SetCorrectTScalar(m_scaleSolution);
            m_spaceshipGO.GetComponent<Spaceship>().SetProblemPosition(m_spaceshipPosition);

            
            #endregion

            #region Setting Position and Rotation
            m_spaceshipGO.transform.position = MathProblemManager.instance.MapProblemSpaceToCookieSpace(m_spaceshipPosition);
            if (m_type == Type.Direction)
            {
                m_spaceshipGO.GetComponent<Spaceship>().SetRandomRotation();
            }
            else if(m_type == Type.Collision)
            {
                m_spaceshipGO.GetComponent<Spaceship>().LookUp();
            }
            else if(m_type == Type.Scale)
            {
                m_spaceshipGO.GetComponent<Spaceship>().LookAt(m_targetGO.transform);
            }

            m_targetGO.transform.position = MathProblemManager.instance.MapProblemSpaceToCookieSpace(m_targetPosition);
            m_targetGO.GetComponent<TargetGameObject>().problemPosition = m_targetPosition;

            //> parking spots
            for (int i = 0; i < m_parkingSpotsParentTF.childCount; i++)
            {
                if(i == 0 && m_type == Type.Direction)
                {
                    continue;
                }

                var parkingSpot = m_parkingSpotsParentTF.GetChild(i);
                var holoTarget = FixedPositionsContainer.instance.TakeSampleTarget(0); //0 = Parking Spot Sample
                parkingSpot.position = MathProblemManager.instance.MapProblemSpaceToCookieSpace(holoTarget.position);
                parkingSpot.gameObject.GetComponent<TargetGameObject>().problemPosition = holoTarget.position;
                parkingSpot.gameObject.name = holoTarget.name;
            }

            //> asteroids
            for (int i = 0; i < m_asteroidsParentTF.childCount; i++)
            {
                if (i == 0 && m_type != Type.Direction)
                {
                    continue;
                }

                //Debug.Log("i: " + i);               
                var asteroid = m_asteroidsParentTF.GetChild(i);
                var holoTarget = FixedPositionsContainer.instance.TakeSampleTarget(1); //1 = Asteroid Sample
                asteroid.position = MathProblemManager.instance.MapProblemSpaceToCookieSpace(holoTarget.position);
                asteroid.gameObject.GetComponent<TargetGameObject>().problemPosition = holoTarget.position;
                asteroid.gameObject.name = holoTarget.name;
            }
            #endregion

            #region Setting Target Information
            if(m_type == Type.Collision)
            {
                if(m_collisionSolution == false)
                {
                    m_targetGO = Instantiate(faultyAsteroidPF, MathProblemManager.instance.MapProblemSpaceToCookieSpace(m_directionSolution + m_spaceshipPosition), Quaternion.identity);
                    m_targetGO.name = "Faulty Asteroid (" + m_targetPosition + ")";
                    m_targetGO.SetActive(false);
                }

                m_spaceshipGO.GetComponent<Spaceship>().SetCorrectTargetGO(m_targetGO);
                m_spaceshipGO.GetComponent<Spaceship>().SetTargetGO(m_targetGO);
            }
            else
            {
                m_targetGO.name = m_targetName + " (Correct)";
            }
            #endregion
        }

        Type RandomType()
        {
            var numberOfTypes = System.Enum.GetNames(typeof(Type)).Length;

            return (Type)(Random.Range(0, numberOfTypes));
        }

        Cargo RandomCargo()
        {
            var numberOfCargos = System.Enum.GetNames(typeof(Cargo)).Length;

            return (Cargo)(Random.Range(0, numberOfCargos));
        }

        int GenerateTScalar(bool debug = false)
        {
            #region Generating a Direction Vector
            m_directionSolution = GenerateDirectionVector();
            if(debug) Debug.Log("Direction Vector: " + m_directionSolution);
            #endregion

            #region Calculating Largest and Smallest Distances between Spaceship and Target positions
            Vector3 smallestDistances = CalculateSpaceshipTargetDistance(0);
            Vector3 largestDistances = CalculateSpaceshipTargetDistance(1);

            if (debug) Debug.Log("Smallest Distances: " + smallestDistances);
            if (debug) Debug.Log("Largest Distances: " + largestDistances);
            #endregion

            #region Calculating Quotients
            Vector3 lowQuotients  = CalculateQuotients(true, smallestDistances, m_directionSolution);
            Vector3 highQuotients = CalculateQuotients(false, largestDistances, m_directionSolution);

            if (debug) Debug.Log("Low Quotients: " + lowQuotients);
            if (debug) Debug.Log("High Quotients: " + highQuotients);
            #endregion

            #region Calculating the final T Scalar
            int t = 0;

            var maxT = Mathf.Min(highQuotients.x, highQuotients.y, highQuotients.z);
            var minT = Mathf.Max(lowQuotients.x, lowQuotients.y, lowQuotients.z);

            if (debug) { Debug.Log("Highest Possible T: " + maxT); }
            if (debug) { Debug.Log("Lowest Possible T: " + minT); }

            t = Random.Range((int)minT, (int)maxT + 1);
            if (debug) Debug.Log("T = " + t);
            #endregion

            #region Calculating Spaceship Position with Direction Vector and T Scalar
            m_spaceshipPosition = m_targetPosition + m_directionSolution * t;
            #endregion

            return t;
        }

        Vector3 GenerateDirectionVector()
        {
            var minDistanceBetweenSpaceshipAndTarget = MathProblemManager.instance.minAsteroidDistance -
                              MathProblemManager.instance.maxCollisionDistance;
            
            var directionVectorX = GenerateDirectionVectorCoordinate((int)m_targetPosition.x, minDistanceBetweenSpaceshipAndTarget);
            var directionVectorY = GenerateDirectionVectorCoordinate((int)m_targetPosition.y, minDistanceBetweenSpaceshipAndTarget);
            var directionVectorZ = GenerateDirectionVectorCoordinate((int)m_targetPosition.z, minDistanceBetweenSpaceshipAndTarget);

            return new Vector3(directionVectorX, directionVectorY, directionVectorZ);
        }

        int GenerateDirectionVectorCoordinate(int targetCoordinate, int minDistance)
        {
            var directionCoordinate = Random.Range(MathProblemManager.instance.minDirectionVectorCoordinate,
                                                   minDistance + 1); //would be 25, but this is exlusive random range, so we add 1 to include 25
            if (targetCoordinate > 0) { directionCoordinate = -directionCoordinate; }

            return directionCoordinate;
        }

        Vector3 CalculateSpaceshipTargetDistance(int type)
        {
            Vector3 distances = Vector3.zero;

            distances.x = CalculateDistance(type, m_targetPosition.x);
            distances.y = CalculateDistance(type, m_targetPosition.y);
            distances.z = CalculateDistance(type, m_targetPosition.z);

            return distances;
        }

        float CalculateDistance(int type, float targetCoordinate)
        {
            float distance = 0;

            var minSpaceshipCoordinate = MathProblemManager.instance.minCollisionDistance;
            var maxSpaceshipCoordinate = MathProblemManager.instance.maxCollisionDistance;

            var selectedCoordinate = targetCoordinate < 0 ? maxSpaceshipCoordinate : minSpaceshipCoordinate; //finding the targetCoordinate to use for calculatingthe largest and smallest selectedCoordinate on the X axis
            if(type == 1) { selectedCoordinate = -selectedCoordinate; }
            distance = Mathf.Abs(targetCoordinate + selectedCoordinate); //the largest selectedCoordinate is target position minus the selected position (e.g. 100 - 50 = 50)

            return distance;
        }

        Vector3 CalculateQuotients(bool ceil, Vector3 distances, Vector3 directions)
        {
            Vector3 quotients = Vector3.zero;

            quotients.x = CalculateSingleQuotientComponent(ceil, distances.x, directions.x);
            quotients.y = CalculateSingleQuotientComponent(ceil, distances.y, directions.y);
            quotients.z = CalculateSingleQuotientComponent(ceil, distances.z, directions.z);

            return quotients;
        }

        float CalculateSingleQuotientComponent(bool ceil, float distance, float direction)
        {
            float quotientComponent = 0f;

            if(ceil)
            {
                quotientComponent = Mathf.Ceil(distance / Mathf.Abs(direction));
            }
            else
            {
                quotientComponent = Mathf.Floor(distance / Mathf.Abs(direction));
            }

            return quotientComponent;
        }

        public string GetPinCode()
        {
            return m_pinCode;
        }

        public Type GetType()
        {
            return m_type;
        }

        public string GetTargetName()
        {
            return m_targetName;
        }
    }
}