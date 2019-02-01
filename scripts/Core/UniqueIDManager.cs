using System;
using System.Collections.Generic;

namespace Core
{
    internal static class UniqueIDManager
    {
        // global unique id counter
        private static ulong counter = ulong.MinValue;
        private static SortedSet<ulong> usedIDs = new SortedSet<ulong> ();
        private static bool reachedMax = false;
        private static ulong max = ulong.MaxValue;

        internal static ulong Next ()
        {
            if (counter >= max - 1)
            {
                counter = ulong.MinValue;
                reachedMax = true;
            }

            while (usedIDs.Contains (counter))
            {
                if (counter >= max)
                {
                    if (reachedMax)
                    {
                        throw new Exception ("! UniqueIDManager ran out of values.");
                    }
                    else
                    {
                        counter = ulong.MinValue;
                        reachedMax = true;
                    }
                }
                counter++;
            }

            usedIDs.Add (counter);

            return counter;
        }

        internal static void Free (ulong id)
        {
            usedIDs.Remove (id);
        }
    }
}