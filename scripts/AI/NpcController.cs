using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;
using Random = UnityEngine.Random;

/// <summary>
/// Class handling ENEMY NPC behaviour
/// </summary>
[RequireComponent (typeof(Rigidbody2D))]
public class NpcController : MonoBehaviour
{
    public GameObject BulletPrefab;

    public float    MaxViewDistance         = 100f;
    [Range (0, 360)]
    public float    ViewConeAngle           = 90f;
    public float    DeltaDegrees            = 2f;
    [Range (1f, 45f)]
    public float    ShootingAccuracy        = 4f;
    public int      DidSeePlayerCooldown    = 40;
    public int      ShotCooldown            = 14;
    public float    BulletSpeed             = 11f;
    public float    AllroundViewDistance    = 5f;
    public float    Health                  = 1000;
    public float    BackoffDistance         = 3.0f;

    private AiState         state = AiState.Idle;
    private GameObject      player;
    private Vector3         faceDirection;
    private bool            canSeePlayer;
    private bool            didSeePlayer;
    private new Rigidbody2D rigidbody;
    private Vector3         lastKnownPlayerPosition;
    private Vector3         assumedPlayerPosition;
    private Vector3         backoffPosition;
    private float           resumeAttackDistance;
    private int             currentCooldown = 0;
    private int             currentShotCooldown = 0;
    private float           targetHealth;

    void Start ()
    {
        rigidbody = GetComponent<Rigidbody2D> ();
        targetHealth = (int) (Health * 0.6f);
    }

    void FixedUpdate ()
    {
        UpdateAllInformation ();

        canSeePlayer = CanSeePlayer ();

        Debug.Log ("Can see player: " + canSeePlayer);

        switch (state)
        {
            case AiState.Idle:
                break;
            case AiState.Searching:
                break;
            case AiState.CombatSearching:
                LookForPlayer ();
                AttemptMoveToPosition (assumedPlayerPosition, 5.5f);
                break;
            case AiState.Combat:
                TrackPlayer ();
                AttemptMoveToPosition (lastKnownPlayerPosition, 5.0f);
                break;
            case AiState.Backoff:
                AttemptMoveToPosition (backoffPosition, 6.0f);
                ShootAtPlayer ();
                break;
            case AiState.Travelling:
                break;
            default:
                throw new ArgumentOutOfRangeException ();
        }

        currentCooldown++;
        currentShotCooldown++;
    }

    /// <summary>
    /// Updates all the information that the NPC uses to navigate.
    /// Doing this only once every update will, long term, increase
    /// performance and avoid false/outdated information being kept 
    /// around.
    /// </summary>
    private void UpdateAllInformation ()
    {
        // assuming we only need to set this once per scene
        if (player == null)
        {
            player = GameObject.FindWithTag ("Player");
        }

        canSeePlayer = CanSeePlayer ();

        if (canSeePlayer)
        {
            // remember direction of player
            faceDirection = player.transform.position - transform.position;

            // set in case we lose the player
            didSeePlayer = true;

            // so we can seek the player when we lost them
            lastKnownPlayerPosition = player.transform.position;

            state = AiState.Combat;
        }
        else if (didSeePlayer)
        {
            state = AiState.CombatSearching;
        }

        if (currentCooldown >= DidSeePlayerCooldown)
        {
            currentCooldown = 0;
            didSeePlayer = canSeePlayer;
        }

        if (Vector3.Distance (transform.position, player.transform.position) < BackoffDistance)
        {
            if (state != AiState.Backoff)
            {
                backoffPosition = lastKnownPlayerPosition + (Vector3) Random.insideUnitCircle * 3 * BackoffDistance;
                faceDirection = lastKnownPlayerPosition - transform.position;
                resumeAttackDistance = Vector3.Distance (player.transform.position, backoffPosition);
            }
            state = AiState.Backoff;
        }

        if (Vector3.Distance (transform.position, player.transform.position) < resumeAttackDistance)
        {
            state = AiState.Backoff;
        }
        else
        {
            resumeAttackDistance = -1;
        }
    }

    void TakeDamage (Hit hit)
    {
        Debug.Log ("Took " + hit.Damage + " damage.");
        
        if (hit.Initiator == gameObject)
        {
            // self damage
            Health -= (int) (hit.Damage * .33f);
        }
        else
        {
            Health -= (int) hit.Damage;
        }

        if (Health <= 0)
        {
            Debug.Log ("Dead");
            Destroy (gameObject);
        }
        
        // if not in combat, do combat search in direction of the attacker
        if (state != AiState.Combat)
        {
            state = AiState.CombatSearching;
            // look in direction of shot
            faceDirection = player.transform.position - transform.position;
            // 
            lastKnownPlayerPosition = faceDirection.normalized * Helper.RandomInt (10, 50);
        }
    }

    /// <summary>
    /// Combat state method
    /// </summary>
    private void TrackPlayer ()
    {
        transform.rotation = Quaternion.RotateTowards (transform.rotation, Quaternion.LookRotation (transform.forward, faceDirection), DeltaDegrees);
        ShootAtPlayer ();
    }

    private void ShootAtPlayer ()
    {
        if (currentShotCooldown >= ShotCooldown)
        {
            if (Vector2.Angle (transform.up, faceDirection) <= 3f)
            {
                // instantiate a bullet
                var bullet = Instantiate (BulletPrefab, transform.position, transform.rotation);
                bullet.transform.rotation = Quaternion.LookRotation (bullet.transform.forward, transform.up);
                bullet.GetComponent<Rigidbody2D> ().velocity =
                    (Vector3) rigidbody.velocity + transform.up.normalized * BulletSpeed;
                var bulletBehaviour = bullet.GetComponent<BulletBehaviour> ();
                bulletBehaviour.Initator = gameObject;
                bulletBehaviour.Damage = 80;
                bullet.GetComponent<SpriteRenderer> ().color = Color.red;

                // reset shooting cooldown
                currentShotCooldown = 0;
            }
        }
    }

    /// <summary>
    /// CombatSearching state method
    /// </summary>
    private void LookForPlayer ()
    {
        if (Vector3.Distance (transform.position, assumedPlayerPosition) < AllroundViewDistance)
        {
            assumedPlayerPosition = lastKnownPlayerPosition + (Vector3) (Random.insideUnitCircle * Random.Range (5, 20));
            faceDirection = assumedPlayerPosition - transform.position;
        }
    }

    /// <summary>
    /// Attempts to move to given position
    /// </summary>
    private void AttemptMoveToPosition (Vector3 target, float speed)
    {
        transform.rotation = Quaternion.RotateTowards (transform.rotation, Quaternion.LookRotation (transform.forward, target - transform.position), DeltaDegrees);
        rigidbody.velocity = transform.up * speed;
    }

    private bool CanSeePlayer ()
    {

        var playerDirection = player.transform.position - transform.position;

        // angle always between 0 (front) and 180 (back)
        var angle = Vector3.Angle (transform.up, playerDirection);

        // if not inside view cone and assume that nearby targets can always be seeked (?)
        if (angle > ViewConeAngle * .5f && Vector2.Distance (transform.position, player.transform.position) > AllroundViewDistance) 
        {
            return false;
        }

        var hits = Physics2D.RaycastAll (transform.position, playerDirection, 15);
        if (hits.Length < 2) return false;
        return hits[1].collider.gameObject == player;
    }
}
