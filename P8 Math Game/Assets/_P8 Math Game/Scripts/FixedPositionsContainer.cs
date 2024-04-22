using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AstroMath
{
    [Serializable]
    public struct FixedPositionObject
    {
        public enum Type
        {
            Parking = 0,
            Asteroid = 1
        }
        public Type type;
        public string id;
        public Vector3 position;
    }

    public class FixedPositionsContainer : MonoBehaviour
    {
        public static FixedPositionsContainer instance;

        [SerializeField] int numberOfSamplesToCreate;

        [SerializeField] FixedPositionObject[] fixedParkingSpots;
        [SerializeField] FixedPositionObject[] fixedAsteroids;

        public List<FixedPositionObject> sampledParkingPositions;
        public List<FixedPositionObject> sampledAsteroidPositions;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            //> THIS SHOULD BE CHANGED SO THAT IT HAPPENS WHENEVER A MATH PROBLEM IS GENERATED
            CreateNewSampleParkingPositions(numberOfSamplesToCreate);
            CreateNewSampleAsteroidPositions(numberOfSamplesToCreate);
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.N))
            {
                CreateNewSampleParkingPositions(numberOfSamplesToCreate);
                CreateNewSampleAsteroidPositions(numberOfSamplesToCreate);
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                if(sampledParkingPositions.Count > 0)
                {
                    var position = TakeSampleObject();
                    Debug.Log(position);
                }
            }
            if(Input.GetKeyDown(KeyCode.A))
            {
                if (sampledAsteroidPositions.Count > 0)
                {
                    var position = TakeSampleObject(1);
                    Debug.Log(position);
                }
            }
        }

        public FixedPositionObject TakeSampleObject(int type = 0, bool random = true, int index = 0)
        {
            var list = sampledParkingPositions;
            if(type == 1) { list = sampledAsteroidPositions; }

            var chosenIndex = 0;
            if(random) { chosenIndex = UnityEngine.Random.Range(0, list.Count); }
            else       { chosenIndex = index; }

            var sample = list[chosenIndex];
            list.RemoveAt(chosenIndex);

            return sample;
        }

        public void CreateNewSampleParkingPositions(int sampleSize)
        {
            sampledParkingPositions = GetSamplePositions(sampleSize, FixedPositionObject.Type.Parking).ToList();
        }

        public void CreateNewSampleAsteroidPositions(int sampleSize)
        {
            sampledAsteroidPositions = GetSamplePositions(sampleSize, FixedPositionObject.Type.Asteroid).ToList();
        }

        FixedPositionObject[] GetSamplePositions(int sampleSize, FixedPositionObject.Type type)
        {
            FixedPositionObject[] samplePositions = new FixedPositionObject[sampleSize];
            List<FixedPositionObject> fixedPositions = type == FixedPositionObject.Type.Parking ? fixedParkingSpots.ToList() : fixedAsteroids.ToList();

            for (int i = 0; i < sampleSize; i++)
            {
                var randomIndex = UnityEngine.Random.Range(0, fixedPositions.Count);
                samplePositions[i] = fixedPositions[randomIndex];
                fixedPositions.RemoveAt(randomIndex);
            }

            return samplePositions;
        }
    }
}