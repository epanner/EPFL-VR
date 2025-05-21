using UnityEngine;

public class TempLaneObject : LaneObject
{
    protected float selfDestructionDelay = 15.0f;
    protected float grabbingTime = 0.0f;
    protected bool isGrabbed = false;

    public float GetPercentage()
    {
        return Mathf.Clamp01(1.0f - (grabbingTime / selfDestructionDelay));
    }
}