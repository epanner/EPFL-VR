using UnityEngine;
using UnityEngine.InputSystem;
using System;


public class LaserGun : MonoBehaviour
{
    private LineRenderer lineRenderer;
    public InputActionProperty shootAction;
    public GameObject laserOrigin;

    public float laserLength = 50f;
    public float destructionDelay = 1.0f;
    private GameObject lastHit;
    private float hittingTime = 0.0f;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        shootAction.action.Enable();
    }

    private void Update()
    {
        bool isShooting = shootAction.action.IsPressed();
        Vector3 start = laserOrigin.transform.position;
        Vector3 dir = laserOrigin.transform.forward;
        Vector3 end = start + dir * laserLength;

        if (isShooting)
        {
            lineRenderer.SetPosition(0, start);
            lineRenderer.SetPosition(1, end);
            lineRenderer.enabled = true;

            if (Physics.Raycast(start, dir, out RaycastHit hit, laserLength))
            {
                end = hit.point;

                // If it hits something tagged "Obstacle", destroy it
                if (hit.collider.CompareTag("Obstacle"))
                {
                    if (hit.collider.gameObject == lastHit)
                    {
                        hittingTime += Time.deltaTime;
                        if (hittingTime >= destructionDelay)
                        {
                            Destroy(lastHit);
                            hittingTime = 0.0f;
                        }
                    }
                    else {
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
}
