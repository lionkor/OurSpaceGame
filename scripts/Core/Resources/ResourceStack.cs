using System;

namespace Core
{
    public class ResourceStack : IEquatable<ResourceStack>
    {
        public Resource Resource;
        public int Amount;

        public ResourceStack (Resource resource, int amount)
        {
            Resource = resource;
            Amount = amount;
        }

        
        public bool Equals (ResourceStack other)
        {
            if (other == null) return false;
            return other.Resource == this.Resource && 
                   other.Amount   == this.Amount;
        }

        
        
        /// <summary>
        /// Format: Resource Name|Resource Amount
        /// </summary>
        public override string ToString ()
        {
            return Resource.ToString () + "|" + Amount;
        }

    } 
}
