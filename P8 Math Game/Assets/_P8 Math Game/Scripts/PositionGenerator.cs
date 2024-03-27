using UnityEngine;

namespace AstroMath
{
    public static class PositionGenerator
    {
        /// <summary>
        /// Returns a continuous value 3-dimensional position (e.g. <4.0,2.5,1.9>).
        /// </summary>
        /// <para name="min">hroipyh</para>
        public static Vector3 ContinuousPosition(float min, float max, bool debug = false)
        {
            Vector3 position;

            var x = Random.Range(min, max); var randomChance = Random.Range(0, 2); if (randomChance == 1) { x *= -1; }
            var y = Random.Range(min, max); randomChance = Random.Range(0, 2);     if (randomChance == 1) { y *= -1; }
            var z = Random.Range(min, max); randomChance = Random.Range(0, 2);     if (randomChance == 1) { z *= -1; }

            position = new Vector3(x, y, z);

            if(debug)
            {
                Debug.Log("Generated coordinates:");
                Debug.Log($"X: {x}, Y: {y}, Z: {z}");
            }

            return position;
        }

        /// <summary>
        /// Returns a discrete value 3-dimensional position (e.g. <4,2,1>).
        /// </summary>
        public static Vector3 DiscretePosition(int min, int max, bool inclusive = true, bool debug = false)
        {
            //Debug.Log($"Min before: {min}");
            //Debug.Log($"Max before: {max}");

            if(!inclusive) { min += 1; max -= 1; }

            //Debug.Log($"Min after: {min}");
            //Debug.Log($"Max after: {max}");

            Vector3 position = ContinuousPosition(min, max, debug);
            position.x = Mathf.RoundToInt(position.x);
            position.y = Mathf.RoundToInt(position.y);
            position.z = Mathf.RoundToInt(position.z);

            return position;
        }

        /// <summary>
        /// Returns a continuous value 3-dimensional position, within a ring around the origin.
        /// </summary>
        public static Vector3 ContinuousRingPosition(float inner, float outer, bool debug = false)
        {
            Vector3 position;

            var random = Random.Range(0, 2);
            if (random != 0) { inner *= -1; outer *= -1; }

            position = ContinuousPosition(inner, outer, debug);

            return position;
        }

        /// <summary>
        /// Returns a discrete value 3-dimensional position, within a ring around the origin.
        /// </summary>
        public static Vector3 DiscreteRingPosition(float inner, float outer, bool inclusive = true, bool debug = false)
        {
            //Debug.Log($"Min before: {min}");
            //Debug.Log($"Max before: {max}");

            if (!inclusive) { inner += 1; outer -= 1; }

            //Debug.Log($"Min after: {min}");
            //Debug.Log($"Max after: {max}");

            Vector3 position = ContinuousRingPosition(inner, outer, debug);
            position.x = Mathf.RoundToInt(position.x);
            position.y = Mathf.RoundToInt(position.y);
            position.z = Mathf.RoundToInt(position.z);

            return position;
        }
    }
}