using UnityEngine;

public class Asteroid : LaneObject
{
    [Tooltip("object this will break into")]
    public GameObject fractured;

    public void FractureObject()
    {
        Instantiate(fractured, transform.position, transform.rotation); //Spawn in the broken version
        Destroy(gameObject); //Destroy the object to stop it getting in the way
    }
}