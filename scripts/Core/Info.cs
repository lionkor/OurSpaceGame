using System;
using UnityEngine;

namespace Core
{
    public static class Info
    {
        public static void WIP ()
        {
            Log ("INFO: This feature is not yet implemented.");
        }

        public static void WIP (string featureName)
        {
            Log ("INFO: This feature (\"" + featureName + "\") is not yet implemented.");
        }

        public static string ComposeNullRefString (string nameofObject)
        {
            return $"The field \"{nameofObject}\" is null. " +
                $"It has to be set in order for the script to continue.";
        }

        internal delegate void LogOp (object message);
        /// <summary>
        /// Calls <see cref="Debug.Log(object)"/> in a seperate thread.
        /// </summary>
        /// <param name="message"></param>
        public static void Log (object message)
        {
            new LogOp (Debug.Log).BeginInvoke (message, null, null);
        }

        internal delegate void LogWarningOp (object message);
        /// <summary>
        /// Calls <see cref="Debug.LogWarning(object)"/> in a seperate thread.
        /// </summary>
        /// <param name="message"></param>
        public static void LogWarning (object message)
        {
            new LogOp (Debug.LogWarning).BeginInvoke (message, null, null);
        }
    } 
}
