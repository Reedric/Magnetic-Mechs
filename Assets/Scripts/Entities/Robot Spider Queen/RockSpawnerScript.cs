using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockSpawnerScript : MonoBehaviour
{
    //script for spawning rocks during the robot spider queens second phase
    [Header("Components")]
    public GameObject rockSpawnerPrefab;
    [Header("Manage Spawn")]
    public float timeToSpawn = 3;
    //public float despawnHeight = 0;
    private float timer;
    [Header("Stages")]
    private bool notStarted = false;
    // Start is called before the first frame update
    [Header("Manage Queue")]
    private GameObject[] RocksArray;
    private Queue<int> RocksAvailableQueue;
    private int maxRocks = 10;
    [Header("variables")]
    private int spawnwidth = 48;
    void Start()
    {
        timer = 0;
        SetUpArrays();
    }
    private void SetUpArrays()
    {
        RocksArray = new GameObject[maxRocks];
        for (int i = 0; i < maxRocks; i++)
        {
            GameObject tempRock = Instantiate(rockPrefab, transform.position, Quaternion.Euler(0, 0, 0));
            RocksArray[i] = tempRock;
            tempRock.SetActive(false);
            RockScript rockScript = tempRock.GetComponent<RockScript>();
            rockScript.RockSpawnerScript = this;
            rockScript.index = i;
        }
        RocksAvailableQueue = new Queue<int>();
        for (int i = 0; i < maxRocks; i++)
        {
            RocksAvailableQueue.Enqueue(i);
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (notStarted)
        {
            return;
        }
        if (timer < timeToSpawn || RocksAvailableQueue.Count == 0)
        {
            timer = timer + Time.fixedDeltaTime;
        }
        else
        {
            float spawnX = Random.value * spawnwidth;
            SpawnRock(spawnX);
            timer = 0;
        }
    }
    void SpawnRock(float spawnX)
    {
        int currentIndex = RocksAvailableQueue.Dequeue();
        GameObject tempRock = RocksArray[currentIndex];
        tempRock.SetActive(true);
        tempRock.transform.position = new Vector3(spawnX, transform.position.y, .5f);
    }
    public void RockDestroyed(int index)
    {
        RocksAvailableQueue.Enqueue(index);
    }
    public void TriggerStage2()
    {
        notStarted = false;
    }
}
