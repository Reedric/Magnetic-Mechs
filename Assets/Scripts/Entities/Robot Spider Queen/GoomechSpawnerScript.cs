using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoomechSpawnerScript : MonoBehaviour
{
    [Header("Number of Goomechs")]
    private GameObject[] GoomechsArray;
    private Queue<int> GoomechsAvailableQueue;
    [Header("Manage Spawn")]
    private float timeToSpawn;
    private float timer;
    public bool bossActive;
    [Header("Components")]
    public GameObject goomech;
    [Header("Spawn Points")]
    public Transform spawnLeftTransform;
    public Transform spawnRightTransform;
    public Vector3 spawnPosition;
    public bool spawnRight;
    private float spawnRightChance = .5f;
    private float spawnRightChanceModifier = .25f;
    [Header("Stages")]
    public float timeToSpawnStage1 = 10;
    public int maxGoomechsStage1 = 2;
    public float timeToSpawnStage2 = 8;
    public int maxGoomechsStage2 = 3;
    private void Start()
    {
        timeToSpawn = 2;
        TriggerStage1();
        bossActive = false;
        SetUpArrays();
    }
    private void SetUpArrays()
    {
        GoomechsArray = new GameObject[maxGoomechsStage2];
        for (int i = 0; i < maxGoomechsStage2; i++)
        {
            GameObject tempGoomech = Instantiate(goomech, spawnRightTransform.position, Quaternion.Euler(0, 0, 0));
            GoomechsArray[i] = tempGoomech;
            tempGoomech.SetActive(false);
        }
        GoomechsAvailableQueue = new Queue<int>();
        for (int i = 0; i < maxGoomechsStage1; i++)
        {
            GoomechsAvailableQueue.Enqueue(i);
        }
    }
    private void TriggerStage1()
    {
        timeToSpawn = timeToSpawnStage1;
    }
    public void TriggerStage2()
    {
        timeToSpawn = timeToSpawnStage2;
        for (int i = maxGoomechsStage1; i < maxGoomechsStage2; i++)
        {
            GoomechsAvailableQueue.Enqueue(i);
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (!bossActive) return;
        if (GoomechsAvailableQueue.Count == 0)
        {
            timer = 0;
        }
        if (timer < timeToSpawn)
        {
            timer = timer + Time.fixedDeltaTime;
        }
        else
        {
            spawnRight = UnityEngine.Random.Range(0f, 1f) < spawnRightChance;
            if (spawnRight)
            {
                spawnPosition = spawnRightTransform.position;
                if(spawnRightChance < .5f)
                {
                    spawnRightChance = .5f - spawnRightChanceModifier;
                }
                else
                {
                    spawnRightChance -= spawnRightChanceModifier;
                }
            }
            else
            {
                spawnPosition = spawnLeftTransform.position;
                if (spawnRightChance > .5f)
                {
                    spawnRightChance = .5f + spawnRightChanceModifier;
                }
                else
                {
                    spawnRightChance += spawnRightChanceModifier;
                }
            }
            spawnGoomech();
            timer = 0;
        }
    }
    void spawnGoomech()
    {
        int currentIndex = GoomechsAvailableQueue.Dequeue();
        GameObject tempGoomech = GoomechsArray[currentIndex];
        tempGoomech.SetActive(true);
        tempGoomech.transform.position = spawnPosition;
        GoomechScript tempGoomechScript = tempGoomech.GetComponent<GoomechScript>();
        tempGoomechScript.health = 1;
        tempGoomechScript.startSpawnBehavior();
        StartCoroutine(tempGoomechScript.endSpawnBehavior());
        tempGoomechScript.GoomechSpawnerScript = this;
        tempGoomechScript.index = currentIndex;
        if (spawnRight ^ !tempGoomechScript.facingRight) 
        {
            tempGoomech.GetComponent<GoomechScript>().Flip();
        }
    }

    public void GoomechKilled(int index)
    {
        GoomechsAvailableQueue.Enqueue(index);
    }
}
