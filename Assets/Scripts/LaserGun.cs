using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;


public class LaserGun : LaneObject
{
    private LineRenderer lineRenderer;
    public InputActionProperty shootAction;
    public GameObject laserOrigin;
    private IXRSelectInteractor currentInteractor = null;
    public float laserLength = 50f;
    public float destructionDelay = 1.0f;
    private GameObject lastHit;
    private float hittingTime = 0.0f;
    private Rigidbody rb;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        shootAction.action.Enable();

        // Register to grab/release events
        var grabInteractable = GetComponent<XRGrabInteractable>();
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.AddListener(OnGrab);
            grabInteractable.selectExited.AddListener(OnRelease);
        }

        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    private void Update()
    {
        bool isShooting = shootAction.action.IsPressed();
        Vector3 start = laserOrigin.transform.position;
        Vector3 dir = laserOrigin.transform.forward;
        Vector3 end = start + dir * laserLength;

        if (currentInteractor != null && isShooting)
        {
            lineRenderer.SetPosition(0, start);
            lineRenderer.SetPosition(1, end);
            lineRenderer.enabled = true;

            if (Physics.Raycast(start, dir, out RaycastHit hit, laserLength))
            {
                lineRenderer.SetPosition(1, hit.point);

                // If it hits something tagged "Obstacle", destroy it
                if (hit.collider.CompareTag("Obstacle"))
                {
                    SendHaptic(0.1f, 0.05f);
                    if (hit.collider.gameObject == lastHit)
                    {
                        hittingTime += Time.deltaTime;
                        if (hittingTime >= destructionDelay)
                        {
                            SendHaptic(0.5f, 0.2f);
                            lastHit.GetComponent<Asteroid>().FractureObject();
                            hittingTime = 0.0f;
                            Destroy(lastHit);
                        }
                    }
                    else
                    {
                        lastHit = hit.collider.gameObject;
                        hittingTime = 0.0f;
                    }

                }
                else
                {
                    lastHit = null;
                }
            }
        }
        else
        {
            lineRenderer.enabled = false;
        }
    }

    private void OnGrab(SelectEnterEventArgs arg0)
    {
        currentInteractor = arg0.interactorObject;

        inLane = false;
        rb.isKinematic = false;
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        if (currentInteractor == args.interactorObject)
            currentInteractor = null;

        rb.isKinematic = false;
    }

    private void SendHaptic(float amplitude, float duration)
    {
        if (currentInteractor is XRBaseInputInteractor controllerInteractor)
        {
            controllerInteractor.SendHapticImpulse(amplitude, duration);
        }
    }
}
