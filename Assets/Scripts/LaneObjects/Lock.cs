using UnityEngine;

public class Lock : LaneObject
{
    public Lanes lanesOverride;

    public void Unlocked()
    {
        if (lanes == null)
        {
            lanes = lanesOverride;
        }

        lanes.WallUnlocked();
    }
}
