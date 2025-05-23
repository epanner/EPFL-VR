using UnityEngine;

public class LaneObject : MonoBehaviour
{
    protected bool inLane = true;
    protected Lanes lanes;
    protected float health;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

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