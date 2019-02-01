using System.Collections.Generic;
using UnityEngine;
using Core;

public class Actor : MonoBehaviour
{
    [Tooltip ("health of the player on startup (may be changed by other logic)")]
    public float        Health = 2000.0f;
    public Inventory    Inventory;
    public byte         InventorySize;
    public bool DebugNextFrame = false;
    
    // TODO use start or awake?
    void Awake ()
    {
        Inventory = new Inventory (InventorySize);
    }
    
    void Update ()
    {
        if (DebugNextFrame)
        {
            Info.Log (Inventory.ToString ());
            DebugNextFrame = false;
        }
    }

    void TakeDamage (Hit hit)
    {
        Info.Log ("Took " + hit.Damage + " damage.");
        if (hit.Initiator == gameObject)
        {
            // PLANNED self damage
            Health -= (int) (hit.Damage * .33f);
        }
        else
        {
            Health -= hit.Damage;
        }
    }

    public void Gather (ResourceStack stack)
    {
        if (!Inventory.Add (stack))
        {
            Info.Log ("Inventory full!");
        }
        else
        {
            Info.Log ("Gathered " + stack.Amount + " of " + stack.Resource);
        }
    }
}
