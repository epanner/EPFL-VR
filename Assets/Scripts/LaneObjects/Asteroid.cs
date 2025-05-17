using UnityEngine;

public class Asteroid : LaneObject
{
    [Tooltip("object this will break into")]
    public GameObject fractured;


    public void FractureObject()
    {
        Instantiate(fractured, transform.position, transform.rotation); //Spawn in the broken version
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);
            GameManager.Instance.GameOver();
        }
    }
}