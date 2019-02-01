using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;


namespace Core
{
    public static class Helper
    {
        private static System.Random random;

        public static T ChooseRandom<T> (this T[] array)
        {
            return array[Random.Range (0, array.Length)];
        }

        public static int RandomInt ()
        {
            if (random == null)
            {
                random = new System.Random ();
            }

            return random.Next ();
        }

        public static int RandomInt (int min, int max)
        {
            if (random == null)
            {
                random = new System.Random ();
            }

            return random.Next (min, max);
        }
    } 
}

