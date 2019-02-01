using Core;
using UnityEngine;

[RequireComponent (typeof (Collider2D))]
public class ResourceDropBehaviour : MonoBehaviour
{
    public float MaxVelocity = 5.0f;

    private Rigidbody2D rigidbody;
    private GameObject player;

    void Start ()
    {
        rigidbody = GetComponent<Rigidbody2D> ();
        player = GameObject.FindGameObjectWithTag ("Player");
    }

    void FixedUpdate ()
    {
        if (Vector3.Distance (transform.position, player.transform.position) <= Const.ResourceAttractionDistance)
        {
            rigidbody.velocity += (Vector2) (player.transform.position - transform.position);

            if (Vector3.Distance (transform.position, player.transform.position) <= Const.ResourcePickupRadius)
            {

            }
        }

        if (rigidbody.velocity.sqrMagnitude >= MaxVelocity * MaxVelocity)
        {
            rigidbody.velocity = rigidbody.velocity.normalized * MaxVelocity;
        }
    }
}
