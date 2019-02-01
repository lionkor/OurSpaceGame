using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomingBehaviour : MonoBehaviour
{
    public int FOVDelta = 3;
    public int FOVMax = 60;
    public int FOVMin = 5;
    private Camera main;

    void Start ()
    {
        main = Camera.main;
    }

    void Update()
    {
        var scroll = Input.GetAxis ("Mouse ScrollWheel");

        if (scroll > 0)
        {
            // zoom out
            main.fieldOfView += main.fieldOfView / 10.0f;
        }
        else if (scroll < 0)
        {
            // zoom in
            main.fieldOfView -= main.fieldOfView / 10.0f;
        }

        main.fieldOfView = Mathf.Clamp (main.fieldOfView, FOVMin, FOVMax);
    }
}
