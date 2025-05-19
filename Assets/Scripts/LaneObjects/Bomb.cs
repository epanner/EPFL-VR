using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Bomb : LaneObject
{
    public float bombRadius = 5.0f;

    private Rigidbody rb;

    private bool armed = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SelectEntered(SelectEnterEventArgs args)
    {
        inLane = false;
        armed = true;
        rb.isKinematic = false;
    }

    public void SelectExited(SelectExitEventArgs args)
    {
        rb.isKinematic = false;
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
        Destroy(gameObject);
    }
}