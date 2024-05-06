using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AstroMath
{
    [Serializable]
    public struct HoloTarget
    {
        public enum Type
        {
            Parking = 0,
            Asteroid = 1,
            Bounty = 2
        }
        public Type type;
        public string name;
        public Vector3 position;
    }

    public class FixedPositionsContainer : MonoBehaviour
    {
        public static FixedPositionsContainer instance;

        public int numberOfParkingSamplesToCreate;
        public int numberOfAsteroidSamplesToCreate;
        public int numberOfBountySamplesToCreate;

        [SerializeField] HoloTarget[] parkingTargets;
        [SerializeField] HoloTarget[] asteroidTargets;
        [SerializeField] HoloTarget[] bountyTargets;

        public List<HoloTarget> sampledParkingTargets;
        public List<HoloTarget> sampledAsteroidTargets;
        public List<HoloTarget> sampledBountyTargets;

        [SerializeField] GameObject asteroidPF;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            //> THIS SHOULD BE CHANGED SO THAT IT HAPPENS WHENEVER A MATH PROBLEM IS GENERATED
            CreateSampleParkingTargets();
            CreateSampleAsteroidTargets();
            CreateSampleBountyTargets();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                SpawnAsteroids();
            }
        }

        void SpawnAsteroids()
        {
            for (int i = 0; i < numberOfAsteroidSamplesToCreate; i++)
            {
                var mappedPosition = sampledAsteroidTargets[i].position;
                mappedPosition = MathProblemManager.instance.MapProblemSpaceToCookieSpace(sampledAsteroidTargets[i].position);
                var asteroid = Instantiate(asteroidPF, mappedPosition, Quaternion.identity);
                asteroid.name = sampledAsteroidTargets[i].name;
                asteroid.transform.SetParent(transform);
            }
        }

        public HoloTarget TakeSampleTarget(int type = 0, bool random = true, int index = 0)
        {
            var list = sampledParkingTargets;
            var listName = "sampledParkingTargets";
            if (type == 1) { list = sampledAsteroidTargets; listName = "sampledAsteroidTargets"; }
            if (type == 2) { list = sampledBountyTargets; listName = "sampledBountyTargets"; }

            var chosenIndex = 0;
            if(random) { chosenIndex = UnityEngine.Random.Range(0, list.Count); }
            else       { chosenIndex = index; }

            var sample = list[chosenIndex];
            list.RemoveAt(chosenIndex);

            //Debug.Log("Took sample " + sample.name + " at index " + chosenIndex + " from " + listName + " with position " + sample.position);

            return sample;
        }

        public void CreateSampleParkingTargets()
        {
            sampledParkingTargets = GetSampleTargets(numberOfParkingSamplesToCreate, HoloTarget.Type.Parking).ToList();
        }

        public void CreateSampleAsteroidTargets()
        {
            sampledAsteroidTargets = GetSampleTargets(numberOfAsteroidSamplesToCreate, HoloTarget.Type.Asteroid).ToList();
        }

        public void CreateSampleBountyTargets()
        {
            sampledBountyTargets = GetSampleTargets(numberOfBountySamplesToCreate, HoloTarget.Type.Bounty).ToList();
        }
        HoloTarget[] GetSampleTargets(int sampleSize, HoloTarget.Type type)
        {
            HoloTarget[] array = new HoloTarget[sampleSize];
            List<HoloTarget> list = new List<HoloTarget>();
            if      (type == HoloTarget.Type.Parking)  { list = parkingTargets.ToList(); }
            else if (type == HoloTarget.Type.Asteroid) { list = asteroidTargets.ToList(); }
            else if (type == HoloTarget.Type.Bounty)   { list = bountyTargets.ToList();}

            for (int i = 0; i < sampleSize; i++)
            {
                var randomIndex = UnityEngine.Random.Range(0, list.Count);
                array[i] = list[randomIndex];
                list.RemoveAt(randomIndex);
            }

            return array;
        }
    }
}