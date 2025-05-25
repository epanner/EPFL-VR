using UnityEngine;

public class LaneObject : MonoBehaviour
{
    protected bool inLane = true;
    protected Lanes lanes;
    protected float health;

    public bool IsInLane()
    {
        return inLane;
    }

    public void AddedToLane(Lanes lanes)
    {
        this.lanes = lanes;
    }

    protected void DestroyLaneObject()
    {
        lanes.RemoveLaneObject(this);
        Destroy(gameObject);
    }
}