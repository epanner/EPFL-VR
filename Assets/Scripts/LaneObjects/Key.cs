using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Key : LaneObject
{
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    void Update()
    {

    }

    public void SelectEntered(SelectEnterEventArgs args)
    {
        AudioManager.Instance.PlayGrabSound();
        inLane = false;
        GameManager.Instance.AddToDestroy(gameObject);
        rb.isKinematic = false;
    }

    public void SelectExited(SelectExitEventArgs args)
    {
        rb.isKinematic = false;
    }
}
