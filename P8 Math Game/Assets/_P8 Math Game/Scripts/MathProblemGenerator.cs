using System;
using UnityEngine;

namespace AstroMath
{
    [Serializable]
    public struct MathProblem
    {
        public enum Type
        {
            Direction = 0,
            Collision = 1,
            Scale = 2
        }
        public Type type;

        public Vector3 directionAnswer;
        public bool collisionAnswer;
        public int scaleAnswer;

        public Vector3 spaceshipPosition;
        public Vector3 parkingPosition;
    }

    public static class MathProblemGenerator
    {
        /// <summary>
        /// Returns a math problem of type 'number' (0 = direction problem, 1 = collision problem, 2 = scale (t) problem).
        /// </summary>
        /// <param name="random">Whether or not to generate a random type of math problem.</param>
        /// <param name="number">Specifies the type of math problem.</param>
        /// <param name="debug">Whether or not to debug relevant information to the console.</param>
        /// <returns></returns>
        public static MathProblem GenerateMathProblem(bool random = true, int number = 0, bool debug = false)
        {
            var randomNumber = number;

            if (random)
            {
                var numberOfMathProblemTypes = Enum.GetNames(typeof(MathProblem.Type)).Length;

                Debug.Log($"Number of Math Problem types: {numberOfMathProblemTypes}");

                randomNumber = UnityEngine.Random.Range(0, numberOfMathProblemTypes);
            }

            MathProblem newMathProblem = new MathProblem();
            newMathProblem.type = (MathProblem.Type)randomNumber;

            newMathProblem.spaceshipPosition = PositionGenerator.ContinuousRingPosition(0, 2);
            newMathProblem.parkingPosition = PositionGenerator.ContinuousRingPosition(0, 2);
            newMathProblem.directionAnswer = newMathProblem.parkingPosition - newMathProblem.spaceshipPosition;

            switch (randomNumber)
            {
                case 0:
                    //direction math problem chosen
                    break;
                case 1:
                    //generate whether to produce a collision or no collision
                    var chance = UnityEngine.Random.Range(0, 2);
                    if(chance == 0) //no collision will happen
                    {
                        newMathProblem.collisionAnswer = false;
                    }
                    else //collision will happen
                    {
                        newMathProblem.collisionAnswer = true;

                        var t = UnityEngine.Random.Range(2, 5); //we randomly select a value for 't'
                        var signChance = UnityEngine.Random.Range(0, 2); //we decide whether 't' should be positive or negative
                        if(signChance == 1) t *= -1;
                        newMathProblem.scaleAnswer = t;

                        Vector3 collisionPosition = newMathProblem.spaceshipPosition + t * newMathProblem.directionAnswer;
                        newMathProblem.parkingPosition = collisionPosition;
                    }
                    break;
                case 2:
                    //generate something else
                    newMathProblem.scaleAnswer = 1;
                    break;
                default:
                    break;
            }

            return newMathProblem;
        }
    }
}