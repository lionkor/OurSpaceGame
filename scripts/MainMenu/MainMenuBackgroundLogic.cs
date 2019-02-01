using UnityEngine;

public class MainMenuBackgroundLogic : MonoBehaviour 
{
    public GameObject BackgroundPrefab;
    public Vector3    InstantiationPosition;
    public float      YSpeed;
    
    private GameObject[] objects;
    private float        height;

    void Start ()
    {
        height = InstantiationPosition.y;
    }

    void Update ()
    {
        foreach (var obj in objects)
        {
            if (Vector3.Distance (obj.transform.position, InstantiationPosition) >= height)
            {
                obj.transform.position = InstantiationPosition;
            }
            obj.transform.position = new Vector3 (obj.transform.position.x, 
                obj.transform.position.y - YSpeed, obj.transform.position.z);
        }
    }
}
