using System.Collections.Generic;
using NUnit.Framework.Constraints;
using UnityEngine;

public class Lanes : MonoBehaviour
{
    // Lane info
    public Transform obstacleSpawnPoint;
    public Transform playerSpawnPoint;
    private Vector3 despawnPoint;
    public Vector3 direction = Vector3.forward;

    private int lanes = 4;
    private float laneWidth = 1.0f;
    private System.Func<float, float> GetSpeed { get; set; }

    // Teleport function
    public GameObject padContainer;
    private List<GameObject> teleportPads = new List<GameObject>();
    public bool teleportEnabled = false;

    // Poke related
    public GameObject buzzerPrefab;
    private GameObject leftBuzzer;
    private GameObject rightBuzzer;

    // Lane objects
    public List<GameObject> asteroids;
    public GameObject bomb;
    public GameObject wall;
    public GameObject key;
    public GameObject lockObject;
    public GameObject gun;
    public GameObject stick;
    public float bombChance = 0.1f;
    public float gunChance = 0.1f;
    public float stickChance = 0.1f;
    private List<LaneObject> laneObjects = new List<LaneObject>();

    private float spawnInterval = 1.5f;
    private float remainingTime = 0.0f;

    private float wallInterval = 30.0f;
    private float remainingWallTime = 20.0f;
    private bool keySpawned = false;
    private float keyPrevTime = 10.0f;
    private bool lockSpawned = false;
    private float lockPrevTime = 6.0f;
    private List<LaneObject> walls = new List<LaneObject>();

    private float speedModifier = 1.0f;

    private bool active = false;

    void Start()
    {
        despawnPoint = obstacleSpawnPoint.position + direction * 80.0f;

        // Instantiate the 2 buzzers
        leftBuzzer = Instantiate(buzzerPrefab, transform);
        leftBuzzer.transform.position = playerSpawnPoint.position + new Vector3((lanes + 1) * laneWidth / 2, 1, 0);

        rightBuzzer = Instantiate(buzzerPrefab, transform);
        rightBuzzer.transform.position = playerSpawnPoint.position - new Vector3((lanes + 1) * laneWidth / 2, -1, 0);


        // Set the teleport pads depending on active state
        EnableTeleportPads(teleportEnabled);
    }

    public void InitGame(int level)
    {
        if (level == 1)
        {
            GetSpeed = x => 2.0f;
            bombChance = 0.3f;
            gunChance = 0.3f;

            laneObjects = new List<LaneObject>();
            walls = new List<LaneObject>();
            teleportPads = new List<GameObject>();

            spawnInterval = 5.0f;
            wallInterval = 60.0f;

            remainingTime = 0.0f;
            remainingWallTime = 40.0f;

            keySpawned = false;
            keyPrevTime = 30.0f;

            lockSpawned = false;
            lockPrevTime = 26.0f;
        }
        else if (level == 2)
        {
            GetSpeed = x => 4.0f;
            bombChance = 0.1f;
            gunChance = 0.1f;

            laneObjects = new List<LaneObject>();
            walls = new List<LaneObject>();
            teleportPads = new List<GameObject>();

            spawnInterval = 1.5f;
            wallInterval = 30.0f;

            remainingTime = 0.0f;
            remainingWallTime = 20.0f;

            keySpawned = false;
            keyPrevTime = 10.0f;

            lockSpawned = false;
            lockPrevTime = 6.0f;
        }
        else if (level == 3)
        {
            GetSpeed = x => 2f * Mathf.Log(1f + x);
            bombChance = 0.05f;
            gunChance = 0.05f;

            laneObjects = new List<LaneObject>();
            walls = new List<LaneObject>();
            teleportPads = new List<GameObject>();

            spawnInterval = 1f;
            wallInterval = 20.0f;

            remainingTime = 5.0f;
            remainingWallTime = 15.0f;

            keySpawned = false;
            keyPrevTime = 10.0f;

            lockSpawned = false;
            lockPrevTime = 6.0f;
        }

        // Set the teleport pads depending on active state
        EnableTeleportPads(teleportEnabled);

        leftBuzzer.GetComponent<Buzzer>().InitGame(level);
        rightBuzzer.GetComponent<Buzzer>().InitGame(level);
        active = true;
    }

    public void CleanGame()
    {
        foreach (LaneObject item in laneObjects)
        {
            Destroy(item.gameObject);
        }
    }

    void FixedUpdate()
    {
        if (!active) return;

        remainingTime -= Time.fixedDeltaTime;
        remainingWallTime -= Time.fixedDeltaTime;

        if (remainingWallTime < 0)
        {
            SpawnWall();
            remainingWallTime = wallInterval;
            keySpawned = false;
            lockSpawned = false;
            remainingTime += 2.0f;
        }

        // Spawn obstacles
        if (remainingTime < 0)
        {
            SpawnObjects();
            remainingTime += spawnInterval;
        }

        foreach (LaneObject obstacle in laneObjects)
        {
            if (obstacle != null)  // don't know why the destroy doesn't work as expected, obstacle still in the list after destroy
            {
                // Move the obstacle in the specified direction
                obstacle.transform.position += direction * Time.deltaTime * GetSpeed(GameManager.Instance.gameTimer) * speedModifier;
            }
        }

        laneObjects.RemoveAll(obstacle => obstacle == null); // didn't know how to handle it better

        // Remove picked up items
        laneObjects.RemoveAll(obstacle => !obstacle.IsInLane());

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
            Vector3 spawnPosition = obstacleSpawnPoint.position + new Vector3((laneIndex + 1) * laneWidth, 0, 0) - new Vector3((lanes + 1) * laneWidth / 2, 0, 0);
            // Which asteroids to take
            GameObject asteroid = asteroids[Random.Range(0, asteroids.Count)];

            // Lane object to spawn
            GameObject spawnObject = null;
            if (Random.Range(0.0f, 1.0f) < bombChance)
            {
                spawnObject = bomb;
            }
            else if (Random.Range(0.0f, 1.0f) < gunChance)
            {
                spawnObject = gun;
            }
            else if (Random.Range(0.0f, 1.0f) < stickChance)
            {
                spawnObject = stick;
            }
            else
            {
                spawnObject = asteroid;
            }

            GameObject newObstacle = Instantiate(spawnObject, spawnPosition, Quaternion.identity);
            newObstacle.transform.SetParent(transform);
            LaneObject laneObject = newObstacle.GetComponent<LaneObject>();
            laneObject.AddedToLane(this);
            laneObjects.Add(laneObject);
        }

        // Spawn key
        if (!keySpawned && remainingWallTime < keyPrevTime)
        {
            int laneIndex = Random.Range(0, lanes);
            while (selectedLanes.Contains(laneIndex))
            {
                laneIndex = Random.Range(0, lanes);
            }

            selectedLanes.Add(laneIndex);
            Vector3 spawnPosition = obstacleSpawnPoint.position + new Vector3((laneIndex + 1) * laneWidth, 0, 0) - new Vector3((lanes + 1) * laneWidth / 2, 0, 0);
            GameObject newKey = Instantiate(key, spawnPosition, Quaternion.identity);
            newKey.transform.SetParent(transform);
            LaneObject laneObject = newKey.GetComponent<LaneObject>();
            laneObject.AddedToLane(this);
            laneObjects.Add(laneObject);
            keySpawned = true;
        }

        // Spawn lock
        if (!lockSpawned && remainingWallTime < lockPrevTime)
        {
            int laneIndex = Random.Range(0, lanes);
            while (selectedLanes.Contains(laneIndex))
            {
                laneIndex = Random.Range(0, lanes);
            }

            selectedLanes.Add(laneIndex);
            Vector3 spawnPosition = obstacleSpawnPoint.position + new Vector3((laneIndex + 1) * laneWidth, 0, 0) - new Vector3((lanes + 1) * laneWidth / 2, 0, 0);
            GameObject newLock = Instantiate(lockObject, spawnPosition, Quaternion.identity);
            newLock.transform.SetParent(transform);
            LaneObject laneObject = newLock.GetComponent<LaneObject>();
            laneObject.AddedToLane(this);
            laneObjects.Add(laneObject);
            lockSpawned = true;
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
            GameManager.Instance.BothControllerHaptics(0.8f, 0.2f);
            AudioManager.Instance.PlayBreakSound();
            Destroy(wall.gameObject);
        }
    }

    public void EnableTeleportPads(bool enabled)
    {
        teleportEnabled = enabled;

        padContainer.SetActive(enabled);
    }

    public List<GameObject> GetTeleportPads()
    {
        return teleportPads;
    }

    public void StartLanes(bool active)
    {
        this.active = active;
    }

    public void RemoveLaneObject(LaneObject laneObject)
    {
        laneObjects.Remove(laneObject);
        if (walls.Contains(laneObject))
        {
            walls.Remove(laneObject);
        }
    }

    public void SetSpeedModifier(float speedModifier)
    {
        this.speedModifier = speedModifier;
    }
}
