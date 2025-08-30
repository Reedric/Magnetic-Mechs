using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockSpawnerScript : MonoBehaviour
{
    [Header("Components")]
    public GameObject rockPrefab;
    [Header("Manage Spawn")]
    public float timeToSpawn = 3;
    private float timer;
    [Header("Stages")]
    private bool notStarted = true;
    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (notStarted)
        {
            return;
        }
        if (timer < timeToSpawn)
        {
            timer = timer + Time.fixedDeltaTime;
        }
        else
        {
            float spawnX = Random.value*48;
            SpawnRock(spawnX);
            timer = 0;
        }
    }
    void SpawnRock(float spawnX)
    {
        Instantiate(rockPrefab,new Vector3(spawnX, transform.position.y,.5f), Quaternion.Euler(0,0,0));
    }
    public void TriggerStage2()
    {
        notStarted = false;
    }
}
