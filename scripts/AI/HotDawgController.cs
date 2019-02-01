using System;
using UnityEngine;
using Core;

[Obsolete]
public class HotDawgController : MonoBehaviour
{
    private int health = 20;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log (health);
		if (health <= 0)
		{
			Destroy (gameObject);
			
		}
	}

    void TakeDamage (Hit hit)
    {
        Debug.Log ("OOF, I took " + hit.Damage+ " damage!");
        health -= (int) hit.Damage;
    }
}
