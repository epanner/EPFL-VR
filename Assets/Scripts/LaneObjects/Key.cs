using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Key : LaneObject
{
    private Rigidbody rb;

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
        GameManager.Instance.AddToDestroy(gameObject);
        rb.isKinematic = false;
    }

    public void SelectExited(SelectExitEventArgs args)
    {
        rb.isKinematic = false;
    }
}
