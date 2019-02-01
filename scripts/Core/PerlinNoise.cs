using UnityEngine;


namespace Core
{
    public static class PerlinNoise
    {
        // based on http://flafla2.github.io/2014/08/09/perlinnoise.html
        public static float OctavePerlin (float x, float y, int octaves, float persistence)
        {
            float total = 0;
            float frequency = 1;
            float amplitude = 1;
            float maxValue = 0;  // Used for normalizing result to 0.0 - 1.0
            for (int i = 0; i < octaves; i++)
            {
                total += Mathf.PerlinNoise (x * frequency, y * frequency) * amplitude;

                maxValue += amplitude;

                amplitude *= persistence;
                frequency *= 2;
            }

            return total / maxValue;
        }
    } 
}