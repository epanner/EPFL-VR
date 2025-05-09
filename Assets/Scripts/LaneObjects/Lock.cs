using UnityEngine;

public class Lock : LaneObject
{
    public Lanes lanesOverride;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Unlocked()
    {
        if (lanes == null)
        {
            lanes = lanesOverride;
        }

        lanes.WallUnlocked();
    }
}
