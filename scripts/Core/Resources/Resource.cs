using System.Linq;
using UnityEngine;

namespace Core
{
    [CreateAssetMenu (fileName = "MyResource", menuName = "Core/Resource")]
    public class Resource : ScriptableObject
    {
        public string Name;

        public override bool Equals (object obj)
        {
            return base.Equals (obj);
        }

        public override int GetHashCode ()
        {
            return base.GetHashCode ();
        }

        public override string ToString ()
        {
            return Name.ToString ();
        }

        public static bool operator ==(Resource res0, Resource res1)
        {
            string res0Name = res0?.Name ?? string.Empty;
            string res1Name = res1?.Name ?? string.Empty;
            return res0Name.Equals (res1Name);
        }

        public static bool operator !=(Resource res0, Resource res1)
        {
            return !(res0 == res1);
        }
    } 
}
