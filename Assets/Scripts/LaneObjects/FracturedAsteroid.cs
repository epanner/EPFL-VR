using UnityEngine;

public class FracturedAsteroid : MonoBehaviour
{
    private float timeSinceFracture;
    void Start()
    {
        timeSinceFracture = 0.0f;
    }

    void Update()
    {
        timeSinceFracture += Time.deltaTime;
        if (timeSinceFracture >= 2.0f)
        {
            Destroy(gameObject);
        }
    }
}
