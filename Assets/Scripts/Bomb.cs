using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Bomb : LaneObject
{
    public float bombRadius = 5.0f;

    private bool armed = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Selected(SelectEnterEventArgs args)
    {
        Debug.Log("Bomb picked up");
        inLane = false;
        armed = true;
        GetComponent<Rigidbody>().useGravity = true;
    }

    // On a collision do a physics spherecast
    private void OnTriggerEnter(Collider collision)
    {
        if (!armed) return;

        RaycastHit[] hits = Physics.SphereCastAll(transform.position, bombRadius, Vector3.up, 1.0f);

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.CompareTag("Obstacle"))
            {
                Destroy(hit.collider.gameObject);
            }
        }
    }
}