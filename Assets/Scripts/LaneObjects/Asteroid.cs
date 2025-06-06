using UnityEngine;

public class Asteroid : LaneObject
{
    [Tooltip("object this will break into")]
    public GameObject fractured;


    public void FractureObject()
    {
        Instantiate(fractured, transform.position, transform.rotation); //Spawn in the broken version
        AudioManager.Instance.PlayBreakSound();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.PlayerHit();
            DestroyLaneObject();
        }
    }
}