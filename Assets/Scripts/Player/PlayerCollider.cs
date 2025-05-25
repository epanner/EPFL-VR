using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
    public Transform head;
    public Transform floor;
    CapsuleCollider capsuleCollider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        float height = head.position.y - floor.position.y;
        capsuleCollider.height = height;
        transform.position = head.position - Vector3.up * (height / 2);
    }
}
