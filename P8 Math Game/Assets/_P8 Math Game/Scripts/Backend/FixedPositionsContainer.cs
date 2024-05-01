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
            Asteroid = 1
        }
        public Type type;
        public string name;
        public Vector3 position;
    }

    public class FixedPositionsContainer : MonoBehaviour
    {
        public static FixedPositionsContainer instance;

        public int numberOfSamplesToCreate;

        [SerializeField] HoloTarget[] parkingTargets;
        [SerializeField] HoloTarget[] asteroidTargets;

        public List<HoloTarget> sampledParkingTargets;
        public List<HoloTarget> sampledAsteroidTargets;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            //> THIS SHOULD BE CHANGED SO THAT IT HAPPENS WHENEVER A MATH PROBLEM IS GENERATED
            CreateSampleParkingTargets();
            CreateSampleAsteroidTargets();
        }

        public HoloTarget TakeSampleTarget(int type = 0, bool random = true, int index = 0)
        {
            var list = sampledParkingTargets;
            if(type == 1) { list = sampledAsteroidTargets; }

            var chosenIndex = 0;
            if(random) { chosenIndex = UnityEngine.Random.Range(0, list.Count); }
            else       { chosenIndex = index; }

            var sample = list[chosenIndex];
            list.RemoveAt(chosenIndex);

            return sample;
        }

        public void CreateSampleParkingTargets()
        {
            sampledParkingTargets = GetSampleTargets(numberOfSamplesToCreate, HoloTarget.Type.Parking).ToList();
        }

        public void CreateSampleAsteroidTargets()
        {
            sampledAsteroidTargets = GetSampleTargets(numberOfSamplesToCreate, HoloTarget.Type.Asteroid).ToList();
        }

        HoloTarget[] GetSampleTargets(int sampleSize, HoloTarget.Type type)
        {
            HoloTarget[] array = new HoloTarget[sampleSize];
            List<HoloTarget> list = type == HoloTarget.Type.Parking ? parkingTargets.ToList() : asteroidTargets.ToList();

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