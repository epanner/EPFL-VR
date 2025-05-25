using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;


public class LaserGun : TempLaneObject
{
    private LineRenderer lineRenderer;
    public InputActionProperty leftShootAction;
    public InputActionProperty rightShootAction;
    private InputActionProperty currentShootAction;
    public GameObject laserOrigin;
    private float laserLength = 50f;
    private float destructionDelay = 1.0f;
    private GameObject lastHit;
    private float hittingTime = 0.0f;
    private Rigidbody rb;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();

        // Register to grab/release events
        var grabInteractable = GetComponent<XRGrabInteractable>();
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.AddListener(OnGrab);
            grabInteractable.selectExited.AddListener(OnRelease);
        }

        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        switch (GameManager.Instance.level)
        {
            case 1:
                laserLength = 100f;
                selfDestructionDelay = 30.0f;
                break;
            case 2:
                laserLength = 50f;
                selfDestructionDelay = 15.0f;
                break;
            case 3:
                laserLength = 30f;
                selfDestructionDelay = 10f;
                break;
        }
    }

    private void Update()
    {
        if (GameManager.Instance.inGame)
        {
            if (isGrabbed)
            {
                grabbingTime += Time.deltaTime;
            }
            if (grabbingTime > selfDestructionDelay)
            {
                Destroy(gameObject);
            }
            if (currentInteractor != null && currentShootAction.action.IsPressed())
            {
                Vector3 start = laserOrigin.transform.position;
                Vector3 dir = laserOrigin.transform.forward;
                Vector3 end = start + dir * laserLength;

                lineRenderer.SetPosition(0, start);
                lineRenderer.SetPosition(1, end);
                lineRenderer.enabled = true;
                AudioManager.Instance.PlayShootSound();

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
                    if (hit.collider.CompareTag("PuzzleTarget"))
                    {
                        SendHaptic(0.2f, 0.1f);
                        AudioManager.Instance.PlayTargetPopSound();
                        hit.collider.gameObject.GetComponent<PuzzleTarget>().SetTargetActive();
                        hit.collider.enabled = false;
                    }
                }
            }
            else
            {
                lineRenderer.enabled = false;
            }
        }
    }

    private void OnGrab(SelectEnterEventArgs arg0)
    {
        isGrabbed = true;
        currentInteractor = arg0.interactorObject;

        // Determine which hand is grabbing the gun

        if (currentInteractor.handedness == InteractorHandedness.Left)
        {
            currentShootAction = leftShootAction;
            GameManager.Instance.SetLeftItem(this);
        }
        else if (currentInteractor.handedness == InteractorHandedness.Right)
        {
            currentShootAction = rightShootAction;
            GameManager.Instance.SetRightItem(this);
        }

        currentShootAction.action.Enable(); // Re-enable after rebinding

        inLane = false;
        GameManager.Instance.AddToDestroy(gameObject);
        rb.isKinematic = false;
    }

    private void OnRelease(SelectExitEventArgs args)
    {
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

        currentShootAction.action.Disable();

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
