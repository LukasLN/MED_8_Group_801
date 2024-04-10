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
        public Vector3 position;
    }

    public class FixedPositionsContainer : MonoBehaviour
    {
        public static FixedPositionsContainer instance;

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
            CreateNewSampleParkingPositions(5);
            CreateNewSampleAsteroidPositions(5);
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.N))
            {
                CreateNewSampleParkingPositions(5);
                CreateNewSampleAsteroidPositions(5);
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                if(sampledParkingPositions.Count > 0)
                {
                    var position = TakeSampleParkingPosition();
                    Debug.Log(position);
                }
            }
            if(Input.GetKeyDown(KeyCode.A))
            {
                if (sampledAsteroidPositions.Count > 0)
                {
                    var position = TakeSampleAsteroidPosition();
                    Debug.Log(position);
                }
            }
        }

        public Vector3 TakeSampleParkingPosition()
        {
            var randomIndex = UnityEngine.Random.Range(0, sampledParkingPositions.Count);
            var sample = sampledParkingPositions[randomIndex];
            sampledParkingPositions.RemoveAt(randomIndex);

            return sample.position;
        }

        public Vector3 TakeSampleAsteroidPosition()
        {
            var randomIndex = UnityEngine.Random.Range(0, sampledAsteroidPositions.Count);
            var sample = sampledAsteroidPositions[randomIndex];
            sampledAsteroidPositions.RemoveAt(randomIndex);

            return sample.position;
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