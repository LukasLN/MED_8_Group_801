using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace AstroMath
{
    public class MathProblemGeneratorPanel : MonoBehaviour
    {
        public static MathProblemGeneratorPanel instance;

        [SerializeField] int numberOfProblemsToCreate;
        [SerializeField] TMP_Text numberOfProblemsToCreateText;

        public List<GameObject> spaceships;
        [SerializeField] GameObject spaceshipPF; //PF = Prefab
        [SerializeField] Transform spaceshipParentTF; //TF = Transform

        public List<GameObject> parkings;
        [SerializeField] GameObject parkingPF; //PF = Prefab
        [SerializeField] Transform parkingParentTF; //TF = Transform

        [SerializeField] int minSP, maxSP, minPP, maxPP;
        [SerializeField, Range(-100, 1)] float worldToHologramScale;
        [SerializeField] Vector3 spawnOffset;

        private void Awake()
        {
            instance = this;
        }

        void Start()
        {
            numberOfProblemsToCreateText.text = numberOfProblemsToCreate.ToString(); ;
        }

        public void CreateMathProblems()
        {
            ClearMathProblems();

            for (int i = 0; i < numberOfProblemsToCreate; i++)
            {
                var newMathProblem = MathProblemGenerator.GenerateMathProblem(minSP, maxSP, minPP, maxPP, false, 0);
                MathProblemHolder.instance.mathProblems.Add(newMathProblem);

                Vector3 mathProblemSpaceshipPosition = MathProblemHolder.instance.mathProblems[i].spaceshipPosition;
                Vector3 positionToSpawnSpaceshipIn = new Vector3(mathProblemSpaceshipPosition.x, mathProblemSpaceshipPosition.y, mathProblemSpaceshipPosition.z);

                Vector3 mathProblemParkingPosition = MathProblemHolder.instance.mathProblems[i].targetPosition;
                Vector3 positionToSpawnParkingIn = new Vector3(mathProblemParkingPosition.x, mathProblemParkingPosition.y, mathProblemParkingPosition.z);

                var scalar = worldToHologramScale;
                if (scalar < 0) scalar *= -1;
                positionToSpawnSpaceshipIn /= scalar;
                positionToSpawnParkingIn /= scalar;

                var newSpaceship = Instantiate(spaceshipPF, positionToSpawnSpaceshipIn + spawnOffset, Quaternion.identity, spaceshipParentTF);
                Vector3 randomRotation = new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));

                var newParking = Instantiate(parkingPF, positionToSpawnParkingIn + spawnOffset, Quaternion.Euler(randomRotation), parkingParentTF);

                if(newSpaceship == null) { Debug.LogError("<<SPACESHIP>> DOES NOT EXIST!"); }
                if(newSpaceship.GetComponent<HologramSpaceship>() == null) { Debug.LogError("<<HOLOGRAM COMPONENT>> DOES NOT EXIST!"); }

                spaceships.Add(newSpaceship);
                parkings.Add(newParking);
            }
        }

        public void ClearMathProblems()
        {
            if(MathProblemHolder.instance.mathProblems.Count > 0)
            {
                for (int i = 0; i < MathProblemHolder.instance.mathProblems.Count; i++)
                {
                    Destroy(spaceships[i]);
                }

                MathProblemHolder.instance.mathProblems.Clear();
                spaceships.Clear();
            }
        }

        public void IncreaseNumberOfProblemToCreate(int increaseAmount)
        {
            numberOfProblemsToCreate += increaseAmount;
            numberOfProblemsToCreateText.text = numberOfProblemsToCreate.ToString();
        }

        public void DecreaseNumberOfProblemToCreate(int decreaseAmount)
        {
            numberOfProblemsToCreate -= decreaseAmount;
            numberOfProblemsToCreateText.text = numberOfProblemsToCreate.ToString();//
        }
    }
}