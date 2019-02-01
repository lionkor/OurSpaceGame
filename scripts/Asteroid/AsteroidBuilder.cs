using UnityEngine;
using Core;

public class AsteroidBuilder : MonoBehaviour 
{
    public GameObject   PixelPrefab;
    public int          Size                    = 100;
    public float        PrefabPixelSize         = 0.1f;
    public float        DefaultHealth           = 300.0f;
    public Resource     Resource;

    [Header ("Generation Customization")]
    public float        PerlinMagnitude         = 0.2f;
    [Range (0, 1)]
    public float        RoughFactor             = 1.0f;
    [Range (0, 1)]
    public float        MidFactor               = 1.0f;
    [Range (0, 1)]
    public float        FineFactor              = 1.0f;

    [HideInInspector]
    public bool         AsteroidChanged         = false;

    // holds all pixels & nulls
    private GameObject[,] pixels;


    void Start ()
    {
        pixels = new GameObject[Size, Size];

        int halfSize = Size / 2;

        var maxGen = halfSize;

        for (int x = -halfSize; x <= halfSize; x++)
        {
            for (int y = -halfSize; y <= halfSize; y++)
            {
                float distanceFromCenter = Vector2.Distance (transform.position + new Vector3 (x, y), transform.position);
                //float fact = PerlinNoise.OctavePerlin ((x + halfSize) * PerlinMagnitude, (y + halfSize) * PerlinMagnitude, Octaves, Persistence);

                float factFine  = PerlinNoise.OctavePerlin ((transform.position.x + x + halfSize) * PerlinMagnitude, 
                                                            (transform.position.y + y + halfSize) * PerlinMagnitude, 8,  2);
                float factMid   = PerlinNoise.OctavePerlin ((transform.position.x + x + halfSize) * PerlinMagnitude, 
                                                            (transform.position.y + y + halfSize) * PerlinMagnitude, 4,  1);
                float factRough = PerlinNoise.OctavePerlin ((transform.position.x + x + halfSize) * PerlinMagnitude, 
                                                            (transform.position.y + y + halfSize) * PerlinMagnitude, 2, 16);

                float fact = factFine * FineFactor + factMid * MidFactor + factRough * RoughFactor;

                var gen = distanceFromCenter + fact * 16;
                if (gen > maxGen)
                {
                    continue;
                }
                var pixel = Instantiate (PixelPrefab, transform);
                
                pixel.transform.position += new Vector3 (x * PrefabPixelSize, y * PrefabPixelSize, 0);

                // call Init method on asteroid pixel
                pixel.SendMessage (Const.Init, new AsteroidPixelInitParams
                {
                    Health = (DefaultHealth - (gen / maxGen * DefaultHealth)).Map (0, DefaultHealth, 30, DefaultHealth),
                    Parent = this,
                    YieldResourceStack = new ResourceStack (Resource, 2),
                    Index = { x = x + halfSize, y = y + halfSize }
                });

                pixels[x + halfSize, y + halfSize] = pixel;
            }
        }
    }

    void Update ()
    {
    }

    /// <summary>
    /// Updates all pixels next to a pixel that has been hit
    /// </summary>
    /// <param name="x">x position in pixels array</param>
    /// <param name="y">y position in pixels array</param>
    /// <param name="hit">Hit object originally passed to the pixel</param>
    public void UpdateNearby (Index2D index, Hit hit)
    {
        // The damage will decrease with the square of the distance from the 
        // origin. This may be changed later to be linear.

        for (int i = - (int) hit.Radius; i <= hit.Radius; i++)
            for (int j = - (int) hit.Radius; j <= hit.Radius; j++)
            {
                if (i*i + j*j == 0) continue;
                var pixel = pixels.GetElementOrNull (index.x + i, index.y + j);
                var modHit = new Hit (hit, damage: hit.Damage * ((1.0f / (i*i + j*j) )));
                if (pixel != null)
                {
                    pixel.SendMessage (Const.TakeDamageCollateral, modHit);
                }
            }
    }
}
