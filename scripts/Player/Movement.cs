using UnityEngine;
using Core;

[RequireComponent (typeof(Rigidbody2D), typeof(PlayerActions))]
public class Movement : MonoBehaviour
{
    #region Fields
    [Tooltip ("prefab to be instantiated on bullet fire")]
    public GameObject   BulletPrefab;
    [Tooltip ("ship movement speed factor")]
    public float        MovementSpeed = 0.005f;
    [Tooltip ("bullet speed factor")] 
    public float        BulletSpeed   = 9.0f;
    [Tooltip ("factor of reduction of speed/inertia if rcs on")]
    public float        RcsFactor     = 0.965f;
    [Tooltip ("maximum speed for the player ship")]
    public float        MaxSpeed      = 8f;
    [Tooltip ("cooldown between shots (in physics frames)")]
    public int          ShotCooldown  = 14;
    [Tooltip ("magnitude of the force that pushed the player back on each shot")]
    public float        ShotRecoil    = 0.01f;
    [Tooltip ("[WIP] prefab used for the shooting particle effect")]
    public GameObject   ShotParticlePrefab;
    [Tooltip ("[WIP] spread of the shooting particle effect")]
    public int          ParticleSpread = 10;
    [Tooltip ("[WIP] variance (randomness) of the shooting particle effect")]
    public float        ParticleVariance = 0.1f;
    
    private Vector3     velocity;
    private Vector3     inertia;
    private bool        rcsOn      = true;
    private bool        safetyOn   = false;
    private bool        rmbPressed = false;
    private float       deltaRotation;
    private float       maxDistanceFromMiddle;
    private int         cooldown = 0;
    private GameObject  engine;
    private GameObject  bullet;
    private Rigidbody2D rigid;
    private GameObject  rcsIndicator;
    private Animator    rcsIndicatorAnimator;
    private Animator    safetyIndicatorAnimator;

    private PlayerActions playerActions;
    private System.Random random;
    
    private const float EngineY4 = -0.26f;
    private const float EngineY3 = -0.22f;
    private const float EngineY2 = -0.18f;
    private const float EngineY1 = -0.14f;    
    private const float EngineY0 = -0.10f;

    private const float Epsilon  = 0.0002f;

    private const bool EnableRecoil     = true;
    private const bool EnableParticles  = true;
    
    // this is needed because mainCamera is very slow
    private Camera mainCamera;

    private new Rigidbody2D rigidbody;
    #endregion
    
    void Start ()
    {
        // doing this once for the scene
        mainCamera              = Camera.main;
        rigidbody               = gameObject.GetComponent<Rigidbody2D> ();
        engine                  = GameObject.FindWithTag ("PlayerShipEngine");
        inertia                 = Vector3.zero;
        rcsIndicatorAnimator    = GameObject.Find ("RcsIndicator").GetComponent<Animator> ();
        safetyIndicatorAnimator = GameObject.Find ("SafetyIndicator").GetComponent<Animator> ();
        maxDistanceFromMiddle   = Vector2.Distance(mainCamera.WorldToScreenPoint (transform.position), 
            new Vector2 ((float)mainCamera.scaledPixelWidth / 2, 0));

        random                  = new System.Random ();
        playerActions           = GetComponent<PlayerActions> ();

    }

    #region Updates
    // called on physics frames -> no need for Time.DeltaTime
    void FixedUpdate ()
    {
        cooldown++;

        var mousePos = mainCamera.ScreenToWorldPoint (Input.mousePosition);
        velocity = Input.mousePosition - mainCamera.WorldToScreenPoint (transform.position);
        velocity = new Vector3 (velocity.x, velocity.y, 0);

        // LEFT MOUSE BUTTON
        if (Input.GetMouseButton (0))
        {
            if (cooldown >= ShotCooldown && !safetyOn)
            {
                // instantiate a bullet
                bullet = Instantiate (BulletPrefab, transform.position, transform.rotation);
                bullet.transform.rotation = Quaternion.LookRotation (bullet.transform.forward, transform.up);
                bullet.GetComponent<Rigidbody2D> ().velocity = (Vector3) rigidbody.velocity + 
                                                               velocity.normalized * BulletSpeed;
                bullet.GetComponent<BulletBehaviour> ().Initator = gameObject;
                
                // apply recoil
                if (EnableRecoil && rcsOn == false)
                {
                    rigidbody.velocity += (Vector2) (ShotRecoil * (-transform.up));
                }

                /*
                 * Particles are WIP
                if (EnableParticles)
                {
                    GenerateShotParticles ();
                }
                */
                
                // reset shooting cooldown
                cooldown = 0;
            }
        }
        
        // RIGHT MOUSE BUTTON
        if (Input.GetMouseButton (1))
        {
            rigidbody.velocity += (Vector2) (velocity * MovementSpeed);
            rmbPressed = true;
        }
        else
        {
            rmbPressed = false;
        }
        
        ApplyEnvironmentalForces ();

        transform.rotation = Quaternion.LookRotation (transform.forward, velocity);
        engine.transform.localPosition = 
            new Vector3 (engine.transform.localPosition.x, GetEngineY (), engine.transform.localPosition.z);
    }
    
    private void GenerateShotParticles ()
    {
        return;
        // PLANNED Particles
        // dont forget to also uncomment the appropiate code in FixedUpdate
    }

    void Update ()
    {
        Controls.Toggle (ref rcsOn, ref safetyOn);

        rcsIndicatorAnimator   .SetBool ("rcsOn", rcsOn);
        safetyIndicatorAnimator.SetBool ("safetyOn", safetyOn);
    }
    
    void LateUpdate ()
    {
        mainCamera.transform.position = new Vector3 (transform.position.x, transform.position.y, -10);
    }
    #endregion

    /// <summary>
    /// Caps speed to MaxSpeed and applies RCS if enabled
    /// </summary>
    private void ApplyEnvironmentalForces ()
    {
        if (rigidbody.velocity.magnitude > MaxSpeed && !rcsOn)
        {
            rigidbody.velocity = rigidbody.velocity.normalized * MaxSpeed;
        }
        else if (rigidbody.velocity.magnitude > MaxSpeed && rcsOn)
        {
            rigidbody.velocity = (rigidbody.velocity.normalized * MaxSpeed);
        }
        if (rcsOn)
        {
            rigidbody.velocity *= RcsFactor;
            if (rigidbody.velocity.magnitude <= Epsilon)
            {
                rigidbody.velocity = new Vector3 (0, 0, 0);
            }
        }
    }

    /// <summary>
    /// Calculates Engine Y position (visual flame size)
    /// </summary>
    /// <returns>one of ENGINE_Y_*</returns>
    private float GetEngineY ()
    {
        if (!rmbPressed) return EngineY0;
        var dist = Vector2.Distance (mainCamera.WorldToScreenPoint (transform.position),
            Input.mousePosition);
        
        if (!(dist >= float.Epsilon))
        {
            return EngineY0;
        }

        if (!(dist >= (float) 1 / 4 * maxDistanceFromMiddle))
        {
            return EngineY1;
        }

        if (!(dist >= (float) 2 / 4 * maxDistanceFromMiddle))
        {
            return EngineY2;
        }

        if (dist >= (float) 3 / 4 * maxDistanceFromMiddle)
        {
            return EngineY4;
        }
        
        return EngineY3;
    }
}
