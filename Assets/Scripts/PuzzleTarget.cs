using UnityEngine;

public class PuzzleTarget : MonoBehaviour
{
    public Material activatedMaterial;
    [HideInInspector] public bool isActivated = false;

    public void SetTargetActive()
    {
        isActivated = true;
        GetComponent<MeshRenderer>().material = activatedMaterial;
    }
}