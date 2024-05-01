using System;
using UnityEngine;
namespace AstroMath
{
    public static class MathProblemGenerator
    {
        /// <summary>
        /// Returns a math problem of m_type 'number' (0 = direction problem, 1 = collision problem, 2 = scale (t) problem).
        /// </summary>
        /// <param name="minSP">Minimum value for the spaceship position.</param>
        /// <param name="maxSP">Maximum value for the spaceship position.</param>
        /// <param name="minPP">Minimum value for the parking position.</param>
        /// <param name="maxPP">Maximum value for the parking position.</param>
        /// <param name="randomType">Whether or not to generate a random m_type of math problem.</param>
        /// <param name="type">Specifies the m_type of math problem.</param>
        /// <param name="debug">Whether or not to debug relevant information to the console.</param>
        /// <returns></returns>
        public static MathProblem GenerateMathProblem(float minDistance, float maxDistance, bool randomType = true, int type = 0, bool randomCargo = true, int cargo = 0, bool debug = false)
        {
            MathProblem newMathProblem = new MathProblem();

            var mathProblemType = type;
            var cargoType = cargo;

            #region if MATH PROBLEM TYPE is to be random...
            if (randomType)
            {
                var numberOfMathProblemTypes = Enum.GetNames(typeof(MathProblem.Type)).Length;
                mathProblemType = UnityEngine.Random.Range(0, numberOfMathProblemTypes);
            }
            #endregion

            #region if CARGO is to be random...
            if (randomCargo)
            {
                var numberOfCargoTypes = Enum.GetNames(typeof(MathProblem.Cargo)).Length;
                cargoType = UnityEngine.Random.Range(0, numberOfCargoTypes);
            }
            #endregion

            #region Setting MATH PROBLEM TYPE and CARGO
            //newMathProblem.m_type = (MathProblem.Type)mathProblemType;
            //newMathProblem.m_cargo = (MathProblem.Cargo)cargoType;
            #endregion

            #region Setting POSITIONS and DIRECTION answer of the MATH PROBLEM
            //newMathProblem.spaceshipPosition = PositionGenerator.DiscreteRingPosition((int)minDistance, (int)rayMaxDistance);

            var targetIndex = 0;
            //if(newMathProblem.m_type == MathProblem.Type.Collision)
            {
                targetIndex = 1;
            }
            var sampleObject = FixedPositionsContainer.instance.TakeSampleTarget(targetIndex);

            //newMathProblem.targetName = sampleObject.name;
            //newMathProblem.targetPosition = sampleObject.position;

            //newMathProblem.directionSolution = newMathProblem.targetPosition - newMathProblem.spaceshipPosition;
            #endregion

            #region Setting the PUZZLES of the MATH PROBLEM
            //newMathProblem.pinCode = MathProblem.pinCodes[(int)newMathProblem.m_type, (int)newMathProblem.m_cargo];
            #endregion

            /*
            #region Generating answers to MATH PROBLEM TYPE 1 (collision) and 2 (t scalar)
            switch (mathProblemType)
            {
                case 0: //direction m_type math problem
                    break;
                case 1: //collision m_type math problem
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
                        newMathProblem.targetPosition = collisionPosition;
                    }
                    break;
                case 2: //t scalar m_type math problem
                    //generate something else
                    newMathProblem.scaleAnswer = 1;
                    break;
                default:
                    break;
            }
            #endregion
            */

            return newMathProblem;
        }
    }
}