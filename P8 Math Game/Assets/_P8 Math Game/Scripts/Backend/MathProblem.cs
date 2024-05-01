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
        #endregion

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.O))
            {
                GenerateNewCharacteristics();
                UpdateHoloObjects();
            }
        }

        public void New()
        {
            GenerateNewCharacteristics();
            UpdateHoloObjects();
        }

        public void GenerateNewCharacteristics()
        {
            Debug.Log("Generating new Characteristics for Math Problem...");

            ClearCharacteristics();

            FixedPositionsContainer.instance.CreateSampleParkingTargets();
            FixedPositionsContainer.instance.CreateSampleAsteroidTargets();

            #region Setting Type and Cargo
            m_type = RandomType();
            m_cargo = RandomCargo();
            #endregion

            #region Setting Pin Code
            m_pinCode = m_pinCodes[(int)m_type, (int)m_cargo];
            #endregion

            #region Setting Spaceship Position
            switch (m_type)
            {
                case Type.Direction:
                    m_spaceshipPosition = PositionGenerator.DiscreteRingPosition((int)MathProblemManager.instance.minDirectionDistance,
                                                                                 (int)MathProblemManager.instance.maxDirectionDistance);
                    break;
                case Type.Collision:
                    m_spaceshipPosition = PositionGenerator.DiscreteRingPosition((int)MathProblemManager.instance.minCollisionDistance,
                                                                                 (int)MathProblemManager.instance.maxCollisionDistance);
                    break;
                case Type.Scale:
                    m_spaceshipPosition = PositionGenerator.DiscreteRingPosition((int)MathProblemManager.instance.minScaleDistance,
                                                                                 (int)MathProblemManager.instance.maxScaleDistance);
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
            #endregion

            #region Setting Solutions
            switch (m_type)
            {
                case Type.Direction:
                    m_directionSolution = m_targetPosition - m_spaceshipPosition;
                    break;
                case Type.Collision:
                    var chance = Random.Range(0, 2);
                    if (chance == 0)
                    {
                        m_collisionSolution = false;
                    }
                    else
                    {
                        m_collisionSolution = true;
                        m_scaleSolution = GenerateTScalar();
                    }
                    break;
                case Type.Scale:
                    m_scaleSolution = GenerateTScalar();
                    break;
            }

            #endregion
        }

        void ClearCharacteristics()
        {
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
            #region Enabling/Disabling Target Objects
            if(m_type == Type.Direction)
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

            #region Updating Graphics of Spaceship and Info Panel
            m_spaceshipGO.GetComponent<Spaceship>().UpdateGraphics((int)m_type, (int)m_cargo);
            #endregion

            #region Setting Positions
            m_spaceshipGO.transform.position = MathProblemManager.instance.MapProblemSpaceToCookieSpace(m_spaceshipPosition);
            m_targetGO.transform.position    = MathProblemManager.instance.MapProblemSpaceToCookieSpace(m_targetPosition);
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
            var minDistance = (int)(MathProblemManager.instance.minAsteroidDistance -
                              MathProblemManager.instance.maxCollisionDistance) + 1;

            var directionVectorX = Random.Range(1, minDistance);
            if (m_targetPosition.x > 0) { directionVectorX = -directionVectorX; }

            var directionVectorY = Random.Range(1, minDistance);
            if (m_targetPosition.y > 0) { directionVectorY = -directionVectorY; }

            var directionVectorZ = Random.Range(1, minDistance);
            if (m_targetPosition.z > 0) { directionVectorZ = -directionVectorZ; }

            m_directionSolution = new Vector3(directionVectorX, directionVectorY, directionVectorZ);
            if (debug) Debug.Log("Direction Vector: " + m_directionSolution);
            #endregion

            #region Calculating Difference between Asteroid Positions and Direction Vector Extremes
            var minSpaceshipCoordinate = MathProblemManager.instance.minCollisionDistance;
            var maxSpaceshipCoordinate = MathProblemManager.instance.maxCollisionDistance;
            var selectedCoordinate = 0f;

            var highestX = 0f;
            var highestY = 0f;
            var highestZ = 0f;

            var lowestX = 0f;
            var lowestY = 0f;
            var lowestZ = 0f;

            selectedCoordinate = m_targetPosition.x < 0 ? maxSpaceshipCoordinate : minSpaceshipCoordinate;
            highestX = m_targetPosition.x - selectedCoordinate;
            lowestX  = m_targetPosition.x + selectedCoordinate;

            if (debug) Debug.Log("HighestX = " + m_targetPosition.x + " + " + selectedCoordinate);
            if (debug) Debug.Log("HighestX = " + highestX);
            if (debug) Debug.Log("LowestX = " + m_targetPosition.x + " - " + selectedCoordinate);
            if (debug) Debug.Log("LowestX = " + lowestX);

            selectedCoordinate = m_targetPosition.y < 0 ? maxSpaceshipCoordinate : minSpaceshipCoordinate;
            highestY = m_targetPosition.y - selectedCoordinate;
            lowestY  = m_targetPosition.y + selectedCoordinate;

            selectedCoordinate = m_targetPosition.z < 0 ? maxSpaceshipCoordinate : minSpaceshipCoordinate;
            highestZ = m_targetPosition.z - selectedCoordinate;
            lowestZ  = m_targetPosition.z + selectedCoordinate;

            Vector3 highest = new Vector3(highestX, highestY, highestZ);
            Vector3 lowest = new Vector3(lowestX, lowestY, lowestZ);

            if (debug) Debug.Log("Highest: " + highest);
            if (debug) Debug.Log("Lowest: " + lowest);
            #endregion

            #region Calculating Quotients and the final T Scalar
            var highQuotientX = Mathf.Floor(highest.x / Mathf.Abs(m_directionSolution.x));
            var highQuotientY = Mathf.Floor(highest.y / Mathf.Abs(m_directionSolution.y));
            var highQuotientZ = Mathf.Floor(highest.z / Mathf.Abs(m_directionSolution.z));

            var lowQuotientX = Mathf.Ceil(lowest.x / Mathf.Abs(m_directionSolution.x));
            var lowQuotientY = Mathf.Ceil(lowest.y / Mathf.Abs(m_directionSolution.y));
            var lowQuotientZ = Mathf.Ceil(lowest.z / Mathf.Abs(m_directionSolution.z));

            Vector3 highQuotientsVector = new Vector3(highQuotientX, highQuotientY, highQuotientZ);
            Vector3 lowQuotientsVector = new Vector3(lowQuotientX, lowQuotientY, lowQuotientZ);

            if (debug) Debug.Log("High Quotients: " + highQuotientsVector);
            if (debug) Debug.Log("Low Quotients: " + lowQuotientsVector);

            var highestPossibleT = Mathf.Min(highQuotientX, highQuotientY, highQuotientZ);
            var lowestPossibleT = Mathf.Max(lowQuotientX, lowQuotientY, lowQuotientZ);

            if (debug) Debug.Log("Highest Possible T: " + highestPossibleT);
            if (debug) Debug.Log("Lowest Possible T: " + lowestPossibleT);

            var t = 0;
            if (highestPossibleT <= lowestPossibleT)
            {
                t = (int)highestPossibleT;
            }
            else
            {
                t = Random.Range((int)lowestPossibleT, (int)highestPossibleT + 1);
            }
            if (debug) Debug.Log("T = " + t);
            #endregion

            #region Calculating Spaceship Position with Direction Vector and T Scalar
            m_spaceshipPosition = m_targetPosition + m_directionSolution * t;
            #endregion

            return t;
        }
    }
}