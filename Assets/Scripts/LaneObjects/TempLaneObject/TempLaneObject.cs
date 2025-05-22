using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class TempLaneObject : LaneObject
{
    protected IXRSelectInteractor currentInteractor = null;
    protected float selfDestructionDelay = 15.0f;
    protected float grabbingTime = 0.0f;
    protected bool isGrabbed = false;

    public float GetPercentage()
    {
        return Mathf.Clamp01(1.0f - (grabbingTime / selfDestructionDelay));
    }
}