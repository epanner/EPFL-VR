using System.Collections.Generic;
using UnityEngine;

public class Lanes : MonoBehaviour
{
    public Transform spawnPoint;
    public Vector3 direction = Vector3.forward;
    public GameObject obstacle;
    public GameObject bomb;

    public float bombChance = 0.1f;

    private List<LaneObject> laneObjects = new List<LaneObject>();
    private int lanes = 4;

    private float laneWidth = 1.2f;
    private float speed = 6.0f;

    private float spawnInterval = 1.0f;
    private float elapsedTime = 0.0f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Spawn obstacles
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= spawnInterval)
        {
            int numObjects = Random.Range(1, 4);
            SpawnObjects(numObjects);
            elapsedTime = 0.0f;
        }

        // Move existing obstacles
        foreach (LaneObject obstacle in laneObjects)
        {
            // Move the obstacle in the specified direction
            obstacle.transform.position += direction * Time.deltaTime * speed;
        }
    }

    private void SpawnObjects(int numObjects)
    {
        // Randomly pick numObjects lanes from the max lanes
        List<int> selectedLanes = new List<int>();
        for (int i = 0; i < numObjects; i++)
        {
            int laneIndex = Random.Range(0, lanes);
            while (selectedLanes.Contains(laneIndex))
            {
                laneIndex = Random.Range(0, lanes);
            }
            selectedLanes.Add(laneIndex);
        }

        for (int i = 0; i < selectedLanes.Count; i++)
        {
            int laneIndex = selectedLanes[i];

            // Calculate the spawn position based on the selected lane
            Vector3 spawnPosition = spawnPoint.position + new Vector3(laneIndex * laneWidth, 0, 0) - new Vector3(lanes * laneWidth / 2, 0, 0);

            // Lane object to spawn
            GameObject spawnObject = Random.Range(0.0f, 1.0f) < bombChance ? bomb : obstacle;

            GameObject newObstacle = Instantiate(spawnObject, spawnPosition, Quaternion.identity);
            newObstacle.transform.SetParent(transform);
            laneObjects.Add(newObstacle.GetComponent<LaneObject>());
        }
    }
}
