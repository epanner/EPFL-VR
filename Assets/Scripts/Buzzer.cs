using System;
using System.Threading;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class Buzzer : MonoBehaviour
{
    public Material inactiveMaterial;
    private bool isActive;
    private float activeDelay = 10f;
    [HideInInspector] public float remainingTime;
    private void Awake()
    {
        var grabInteractable = GetComponent<XRSimpleInteractable>();
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.AddListener(OnPoke);
        }
    }

    private void Update()
    {
        if (!GameManager.Instance.inGame) return;
        if (isActive)
        {
            remainingTime -= Time.deltaTime;
            if (remainingTime < 0)
            {
                Timeout();
            }
        }
    }

    public void InitGame(int level)
    {
        switch (level)
        {
            case 1:
                activeDelay = 20f;
                break;
            case 2:
                activeDelay = 10f;
                break;
            case 3:
                activeDelay = 8f;
                break;
        }
        isActive = false;
        transform.Find("Visuals/Base").GetComponent<MeshRenderer>().material = inactiveMaterial;
    }

    private void Timeout()
    {
        isActive = false;
        transform.Find("Visuals/Base").GetComponent<MeshRenderer>().material = inactiveMaterial;
        GameManager.Instance.RemoveBuzzer();
    }

    private void OnPoke(SelectEnterEventArgs arg0)
    {
        isActive = true;
        remainingTime = activeDelay;
        GameManager.Instance.SetBuzzer(this);
    }

    public void BothBuzzed()
    {
        isActive = false;
        transform.Find("Visuals/Base").GetComponent<MeshRenderer>().material = inactiveMaterial;
    }
}