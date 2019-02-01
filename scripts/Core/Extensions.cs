using System;
using UnityEngine;

namespace Core
{
    public static class Extensions
    {
        /// <summary>
        /// creates a vector pointing in this direction 
        /// </summary>
        public static Vector3 GetVector (this Direction dir, float mag = 1.0f)
        {
            var x = 0f;
            var y = 0f;

            switch (dir)
            {
                case Direction.None:
                    break;
                case Direction.Up:
                    y = 1f;
                    break;
                case Direction.Down:
                    y = -1f;
                    break;
                case Direction.Left:
                    x = -1f;
                    break;
                case Direction.Right:
                    x = 1f;
                    break;
                default:
                    throw new ArgumentOutOfRangeException ("dir", dir, null);
            }

            return new Vector3 (x, y, 0).normalized * mag;
        }

        /// <summary>
        /// Maps a value from one range to another.
        /// </summary>
        /// <param name="value">value to map</param>
        /// <param name="from">start of range to map from</param>
        /// <param name="to">end of range to map from</param>
        /// <param name="from2">start of range to map to</param>
        /// <param name="to2">end of range to map to</param>
        /// <returns>value in from2-to2 range equivalent to value in from-to range</returns>
        public static float Map (this float value, float from, float to, float from2, float to2)
        {
            return from2 + (to2 - from2) * ((value - from) / (to - from));
        }

        /// <summary>
        /// Safely gets an element from an array (if out of bounds, returns null).
        /// Please be aware that it will return null as well if the object at that 
        /// index is actually null.
        /// </summary>
        /// <typeparam name="T">any type that can be null</typeparam>
        /// <param name="index">index to try to access</param>
        /// <returns>Element at index or null if index is out of bounds</returns>
        public static T GetElementOrNull<T> (this T[] array, int index) where T : class
        {
            if (index < 0 || index >= array.Length)
            {
                return null;
            }
            else
            {
                return array[index];
            }
        }

        /// <summary>
        /// Safely gets an element from a 2D array (if out of bounds, returns null).
        /// Please be aware that it will return null as well if the object at that 
        /// index is actually null.
        /// </summary>
        /// <typeparam name="T">any type that can be null</typeparam>
        /// <param name="array">2D array</param>
        /// <param name="index0">first dimension index</param>
        /// <param name="index1">second dimension index</param>
        /// <returns>Element at index or null if any index is out of bounds</returns>
        public static T GetElementOrNull<T> (this T[,] array, int index0, int index1) where T : class
        {
            if (index0 < 0 || index0 >= array.GetLength (0) || index1 < 0 || index1 >= array.GetLength (1))
            {
                return null;
            }
            else
            {
                return array[index0, index1];
            }
        }
    }

}