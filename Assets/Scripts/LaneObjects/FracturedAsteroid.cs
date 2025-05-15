using UnityEngine;

public class FracturedAsteroid : MonoBehaviour
{
    private float timeSinceFracture;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timeSinceFracture = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceFracture += Time.deltaTime;
        if (timeSinceFracture >= 2.0f)
        {
            Destroy(gameObject);
        }
    }
}
