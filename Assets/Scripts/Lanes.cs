using System.Collections.Generic;
using NUnit.Framework.Constraints;
using UnityEngine;

public class Lanes : MonoBehaviour
{
    // Lane info
    public Transform obstacleSpawnPoint;
    public Transform playerSpawnPoint;
    public Vector3 direction = Vector3.forward;

    // Teleport pads
    public GameObject teleportPads;

    // Lane objects
    public List<GameObject> asteroids;
    public GameObject bomb;
    public GameObject wall;
    public float bombChance = 0.1f;
    private List<LaneObject> laneObjects = new List<LaneObject>();
    private int lanes = 4;

    private float laneWidth = 1.2f;
    private float speed = 5.0f;

    private float spawnInterval = 1.2f;
    private float remainingTime = 0.0f;

    private float wallInterval = 30.0f;
    private float remainingWallTime = 20.0f;
    private List<LaneObject> walls = new List<LaneObject>();

    private Vector3 despawnPoint;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        despawnPoint = obstacleSpawnPoint.position + direction * 100.0f;

        // Initialize teleport pads
        // Get the only children of the teleport pads
        GameObject pad = teleportPads.transform.GetChild(0).gameObject;

        // Create lanes - 1 copies of it and place them in the correct position
        List<GameObject> pads = new List<GameObject>();
        pads.Add(pad);
        for (int i = 1; i < lanes; i++)
        {
            GameObject newPad = Instantiate(pad, teleportPads.transform);
            pads.Add(newPad);
        }

        // Set the position of the teleport pads
        for (int i = 0; i < pads.Count; i++)
        {
            GameObject padObject = pads[i];
            Vector3 padPosition = playerSpawnPoint.position + new Vector3(i * laneWidth, 0, 0) - new Vector3(lanes * laneWidth / 2, 0, 0);
            padObject.transform.position = padPosition;
        }
    }

    // Update is called once per frame
    void Update()
    {
        remainingTime -= Time.deltaTime;
        remainingWallTime -= Time.deltaTime;

        // Spawn obstacles
        if (remainingTime < 0)
        {
            if (remainingWallTime < 0)
            {
                SpawnWall();
                remainingWallTime = wallInterval;
                remainingTime += 5.0f;
            }


            SpawnObjects();
            remainingTime += spawnInterval;
        }


        foreach (LaneObject obstacle in laneObjects)
        {
            if (obstacle != null)  // don't know why the destroy doesn't work as expected, obstacle still in the list after destroy
            {
                // Move the obstacle in the specified direction
                obstacle.transform.position += direction * Time.deltaTime * speed;
            }
        }

        laneObjects.RemoveAll(obstacle => obstacle == null); // didn't know how to handle it better

        // Despawn obstacles that are beyond the despawn point
        List<LaneObject> objectsToRemove = new List<LaneObject>();
        foreach (LaneObject obstacle in laneObjects)
        {
            if (obstacle.transform.position.z > despawnPoint.z)
            {
                objectsToRemove.Add(obstacle);
            }
        }

        foreach (LaneObject obstacle in objectsToRemove)
        {
            laneObjects.Remove(obstacle);
            Destroy(obstacle.gameObject);
        }
    }

    private void SpawnObjects()
    {
        int numObjects = Random.Range(1, 4);

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
            Vector3 spawnPosition = obstacleSpawnPoint.position + new Vector3(laneIndex * laneWidth, 0, 0) - new Vector3(lanes * laneWidth / 2, 0, 0);
            // Which asteroids to take
            GameObject asteroid = asteroids[Random.Range(0, asteroids.Count)];

            // Lane object to spawn
            GameObject spawnObject = Random.Range(0.0f, 1.0f) < bombChance ? bomb : asteroid;

            GameObject newObstacle = Instantiate(spawnObject, spawnPosition, Quaternion.identity);
            newObstacle.transform.SetParent(transform);
            LaneObject laneObject = newObstacle.GetComponent<LaneObject>();
            laneObject.AddedToLane(this);
            laneObjects.Add(laneObject);
        }
    }

    private void SpawnWall()
    {
        GameObject newWall = Instantiate(wall, obstacleSpawnPoint.position, Quaternion.identity);
        newWall.transform.SetParent(transform);
        LaneObject laneObject = newWall.GetComponent<LaneObject>();
        laneObject.AddedToLane(this);
        laneObjects.Add(laneObject);
        walls.Add(laneObject);
    }

    public void WallUnlocked()
    {
        if (walls.Count > 0)
        {
            LaneObject wall = walls[0];
            walls.RemoveAt(0);
            laneObjects.Remove(wall);
            Destroy(wall.gameObject);
        }
    }

    public void EndGame()
    {
        foreach (LaneObject item in laneObjects)
        {
            Destroy(item.gameObject);
        }

        foreach (LaneObject item in walls)
        {
            Destroy(item.gameObject);
        }
    }

    public void ReInitGame()
    {
        remainingWallTime = 20.0f;
        remainingTime = 0.0f;
        laneObjects = new List<LaneObject>();
        walls = new List<LaneObject>();
    }
}
