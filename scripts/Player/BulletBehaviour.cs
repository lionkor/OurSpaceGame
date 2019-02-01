using UnityEngine;
using System;
using Core;

public class BulletBehaviour : MonoBehaviour
{
	public float DestroyDistance = 30;
    public int HittableLayer;
	public GameObject Initator;
    public float Damage = 100;
    public float Radius = 10;
	
    private UniqueID id;

    // set to true on hit so that it does not hit things multiple times
    private bool used;

	// Use this for initialization
	void Start () 
	{
        if (Initator == null)
        {
            throw new Exception ("Player null!");
        }
        id = new UniqueID ();
        used = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
        transform.position = new Vector3 (transform.position.x, transform.position.y, -0.1f);

        if (Initator == null)
        {
            Destroy (gameObject);
        }

		else if (Vector3.Distance (Initator.transform.position, transform.position) > DestroyDistance)
		{
			Destroy (gameObject);
		}
	}

    void OnTriggerEnter2D (Collider2D other)
    {
        if (used) return;
        else used = true;

        // layer 8 is Hittable layer
        if (other.gameObject.layer == gameObject.layer && other.gameObject != Initator)
        {
            if (Initator ==  null)
            {
                Debug.Log ("player is null");
            }
            var hit = new Hit (Damage, Initator, Radius, id);
            other.SendMessage (Const.TakeDamage, hit, 
                SendMessageOptions.DontRequireReceiver);

            Destroy (gameObject);
        }
    }
}
