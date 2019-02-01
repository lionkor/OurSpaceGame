using UnityEngine;

namespace Core
{
    public static class Controls
    {
        public static bool RcsKeyDown                 => Input.GetKeyDown (rcsKey);
        public static void SetRcsKey (KeyCode key)    => rcsKey = key;

        public static bool SafetyKeyDown              => Input.GetKeyDown (safetyKey);
        public static void SetSafetyKey (KeyCode key) => safetyKey = key;

        private static KeyCode rcsKey       = KeyCode.R;
        private static KeyCode safetyKey    = KeyCode.S;



        public static void Toggle (
            ref bool rcs,
            ref bool safety)
        {
            rcs     = RcsKeyDown    ? !rcs      : rcs;
            safety  = SafetyKeyDown ? !safety   : safety;
        }
    } 
}