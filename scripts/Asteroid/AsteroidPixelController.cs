using UnityEngine;
using Core;
using System.Threading;
using System;

// To be used for Pixel Initialisation
public struct AsteroidPixelInitParams
{
    public AsteroidBuilder  Parent;
    public ResourceStack    YieldResourceStack;
    public Index2D          Index;
    public float            Health;
}


public class AsteroidPixelController : MonoBehaviour
{
    public AsteroidBuilder  Parent;
    public ResourceStack    YieldResourceStack;
    public Index2D          Index;
    public float            Health;

    private float healthMax;
    private float healthPercent;
    private bool colorChanged;
    private Color originalColor;
    private UniqueID lastHitID;

    void Init (AsteroidPixelInitParams initParams)
    {
        Parent              = initParams.Parent;
        Health              = initParams.Health;
        YieldResourceStack  = initParams.YieldResourceStack;
        Index               = initParams.Index;

        healthMax = Parent.DefaultHealth;
        lastHitID = new UniqueID ();
        originalColor = GetComponent<SpriteRenderer> ().color;
        colorChanged = true;
    }

    void LateUpdate ()
    {
        if (colorChanged)
        {
            healthPercent = Health / healthMax;
            gameObject.GetComponent<SpriteRenderer> ().color = new Color (
                originalColor.r * healthPercent,
                originalColor.g * healthPercent,
                originalColor.b * healthPercent,
                originalColor.a);
        }
    }

    void TakeDamage (Hit hit)
    {
        if (hit.BulletID == lastHitID) return;
        else lastHitID = hit.BulletID;

        TakeDamageHelper (hit, true);
    }

    void TakeDamageCollateral (Hit hit)
    {
        if (hit.BulletID == lastHitID) return;
        else lastHitID = hit.BulletID;

        TakeDamageHelper (hit, false);
    }

    private void TakeDamageHelper (Hit hit, bool spread)
    {
        if (hit.Initiator == null)
        {
            Info.LogWarning ($"Invalid Hit: {hit.Damage}, {hit.Initiator}, {hit.Radius}, {spread}");
            return;
        }
        // UNDONE add tests for weapon type, hit amount, maybe harder materials take more hits

        if (hit.Damage < 1f)
        {
            return;
        }

        Health -= (int) hit.Damage;


        // after this has been damaged, damage nearby,
        // if not collateral damage already
        if (spread)
        {
            Parent.UpdateNearby (Index, hit);
        }

        if (Health <= 0)
        {
            YieldAndDestroy (hit);
        }
    }

    public void YieldAndDestroy (Hit hit)
    {
        hit.Initiator.SendMessage (Const.Gather, YieldResourceStack, SendMessageOptions.DontRequireReceiver);
        Destroy (gameObject);
    }
}
