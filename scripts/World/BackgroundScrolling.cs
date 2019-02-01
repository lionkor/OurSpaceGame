using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Handles the infinite scrolling of the background
/// </summary>
[System.Obsolete]
public class BackgroundScrolling : MonoBehaviour
{
    // Width and Height can be calculated from a sprite:
    // Pixel Width / Pixels per Unit
    // Pixel height / Pixels per Unit
    public int          BackgroundWidth;
    public int          BackgroundHeight;
    public GameObject   BackgroundPrefab;

    private GameObject  player;
    private Vector3     lastPos;
    private float       maxDist;

    private Dictionary<Vector3, GameObject> existing;


    void Start ()
    {
        // get the player GO
        player = GameObject.FindWithTag ("Player");

        // dict that will hold all existing backgrounds, indexed by position
        existing = new Dictionary<Vector3, GameObject> ();

        // maximum distance a player can be away from lastPos before the background 
        // will catch up.
        maxDist = Mathf.Min (BackgroundHeight, BackgroundWidth);

        if (!BackgroundPrefab)
        {
            throw new System.NullReferenceException 
                (Core.Info.ComposeNullRefString (nameof (BackgroundPrefab)));
        }

        // initial background
        GenerateBackground (transform.position);
    }

    void FixedUpdate ()
    {
        if (Vector2.Distance ((Vector2) player.transform.position, (Vector2) lastPos) > maxDist)
        {
            GenerateBackground (FindNearestToPlayer ());
        }
    }

    /// <summary>
    /// Finds nearest background object
    /// </summary>
    /// <returns>position usable as index in existing</returns>
    private Vector3 FindNearestToPlayer ()
    {
        Vector3 nearest = lastPos;
        float nearestDistance = int.MaxValue;
        foreach (var kv in existing)
        {
            var dist = Vector3.Distance (player.transform.position, (Vector3) kv.Key);
            if (dist < nearestDistance)
            {
                nearestDistance = dist;
                nearest = kv.Key;
            }
        }
        return nearest;
    }

    /// <summary>
    /// Generates 3x3 background tiles around position,
    /// if not already existing
    /// </summary>
    private void GenerateBackground (Vector3 position)
    {
        for (int i = -1; i < 2; i++)
            for (int j = -1; j < 2; j++)
            {
                var pos = position + new Vector3 (
                    i * BackgroundWidth,
                    j * BackgroundHeight);
                Place (pos);
            }
        lastPos = position;
    }

    /// <summary>
    /// Places a background tile at the given position if that position 
    /// is not already occupied by one
    /// </summary>
    /// <returns>returns true if a new tile has been placed</returns>
    private bool Place (Vector3 position)
    {
        if (!existing.ContainsKey (position))
        {
            existing.Add (
                position, 
                Instantiate (BackgroundPrefab, position, Quaternion.identity));
            return true;
        }
        return false;
    }
}
