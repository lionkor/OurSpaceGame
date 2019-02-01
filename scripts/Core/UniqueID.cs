using System;

namespace Core
{
    public class UniqueID
    {
        private ulong id;

        /// <summary>
        /// Creates a new Unique ID.
        /// </summary>
        public UniqueID ()
        {
            id = UniqueIDManager.Next ();
        }

        /// <summary>
        /// Explicitly frees this UniqueID.
        /// The ID will no longer be unique.
        /// </summary>
        private void Free ()
        {
            UniqueIDManager.Free (id);
        }

        ~UniqueID ()
        {
            Free ();
        }

        /// <summary>
        /// Returns the underlying numeric id as a ulong
        /// </summary>
        public ulong GetULong ()
        {
            return id;
        }

        public override bool Equals (object obj)
        {
            if (obj is UniqueID)
            {
                return (obj as UniqueID) == this;
            }
            else
	        {
                return base.Equals (obj); 
            }
        }

        public override int GetHashCode ()
        {
            return base.GetHashCode ();
        }

        public override string ToString ()
        {
            return $"UniqueID#{id.ToString ()}";
        }

        public static bool operator ==(UniqueID id0, UniqueID id1)
        {
            return id0?.id == id1?.id;
        }

        public static bool operator !=(UniqueID id0, UniqueID id1)
        {
            return !(id0 == id1);
        }
    }
}
