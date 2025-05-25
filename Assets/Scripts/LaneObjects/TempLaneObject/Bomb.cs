using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class Bomb : TempLaneObject
{
    public float bombRadius = 5.0f;

    private Rigidbody rb;

    private bool armed = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        switch (GameManager.Instance.level)
        {
            case 1:
                bombRadius = 15.0f;
                selfDestructionDelay = 30.0f;
                break;
            case 2:
                bombRadius = 10.0f;
                selfDestructionDelay = 15.0f;
                break;
            case 3:
                bombRadius = 5.0f;
                selfDestructionDelay = 10f;
                break;
        }
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.inGame) return;
        
        if (isGrabbed)
        {
            grabbingTime += Time.deltaTime;
        }
        if (grabbingTime > selfDestructionDelay)
        {
            Destroy(gameObject);
        }
    }

    public void SelectEntered(SelectEnterEventArgs args)
    {
        isGrabbed = true;
        currentInteractor = args.interactorObject;

        // Determine which hand is grabbing the gun

        if (currentInteractor.handedness == InteractorHandedness.Left)
        {
            GameManager.Instance.SetLeftItem(this);
        }
        else if (currentInteractor.handedness == InteractorHandedness.Right)
        {
            GameManager.Instance.SetRightItem(this);
        }

        inLane = false;
        GameManager.Instance.AddToDestroy(gameObject);
        armed = true;
        rb.isKinematic = false;
    }

    public void SelectExited(SelectExitEventArgs args)
    {
        isGrabbed = false;
        if (currentInteractor.handedness == InteractorHandedness.Left)
        {
            GameManager.Instance.RemoveLeftItem();
        }
        else if (currentInteractor.handedness == InteractorHandedness.Right)
        {
            GameManager.Instance.RemoveRightItem();
        }
        
        if (currentInteractor == args.interactorObject)
            currentInteractor = null;

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
                hit.collider.gameObject.GetComponent<Asteroid>().FractureObject();
                Destroy(hit.collider.gameObject);
            }
        }
        GameManager.Instance.GrenadeExploded();
        Destroy(gameObject);
    }
}