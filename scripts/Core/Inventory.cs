using System.Text;

namespace Core
{
    public class Inventory
    {
        public ResourceStack[] Resources;

        public Inventory (byte inventorySize)
        {
            Resources = new ResourceStack[inventorySize];
        }

        public int GetAmountOf (Resource resourceType)
        {
            int index = GetIndexOf (resourceType);
            if (index != -1)
            {
                return Resources[index].Amount;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Adds the stack to the inventory. Returns false if inventory is full.
        /// </summary>
        public bool Add (ResourceStack stack)
        {
            if (stack == null)
            {
                Info.Log ("stack null");
                return false;
            }

            if (Resources == null)
            {
                Info.Log ("Resources null");
                return false;
            }
            
            foreach (ResourceStack invStack in Resources)
            {
                if (invStack == null)
                {
                    continue;
                }

                if (invStack.Resource == stack.Resource)
                {
                    invStack.Amount += stack.Amount;
                    return true;
                }
            }

            //if arrived here, then Resource type not in array, so try to add it
            for (int i = 0; i < Resources.Length; i++)
            {
                // look for free slot
                if (Resources[i] == null)
                {
                    Resources[i] = stack;
                    // added successfully
                    return true;
                }
            }
            return false;
        }

        public override string ToString ()
        {
            StringBuilder str = new StringBuilder ();
            foreach (var stack in Resources)
            {
                if (stack != null)
                {
                    str.AppendLine ($"\t{stack.ToString ()}");
                }
            }

            return $"Inventory ({Resources.Length}){{\n{str.ToString ()}}}";
        }

        /// <summary>
        /// Checks if the Resource exists in resources array. 
        /// </summary>
        private bool Contains (Resource resource)
        {
            return GetIndexOf (resource) != -1;
        }

        /// <summary>
        /// Finds given Resource in resources array.
        /// </summary>
        /// <returns>index of Resource in array or -1 if not found</returns>
        private int GetIndexOf (Resource resource)
        {
            for (int i = 0; i < Resources.Length; i++)
            {
                if (Resources[i].Resource == resource)
                {
                    return i;
                }
            }

            return -1;
        }
    }
}
