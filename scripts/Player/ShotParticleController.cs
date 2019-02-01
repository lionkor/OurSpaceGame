using UnityEngine;

public class ShotParticleController : MonoBehaviour 
{
	public Vector3 StartingPosition;
	public Vector3 Velocity;
	public float   MaxDistance;

	void Start ()
	{
		StartingPosition = transform.position;
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		if (Vector3.Distance (transform.position, StartingPosition) > MaxDistance)
		{
			Destroy (gameObject);
			return;
		}
	}
}
