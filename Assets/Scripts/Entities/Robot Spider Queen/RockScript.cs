using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RockScript : MonoBehaviour
{
    //script for the rocks which spawn during the robot spider queens second phase
    [Header("Components")]
    //public Rigidbody2D myRigidbody2D;
    public RockSpawnerScript RockSpawnerScript;
    public GameObject IndividualRockSpawner;

    public int index;
    [Header("variables")]
    public float speed = 3;
    public LayerMask destroyLayers;
    public float minHeight = -3;
    void Awake()
    {
        //myRigidbody2D = GetComponent<Rigidbody2D>();
        destroyLayers = LayerMask.GetMask("Player", "Player Bullet");
    }
    private void Update()
    {
        if (transform.position.y < minHeight)
        {
            HandleDeath();
        }
    }
    void FixedUpdate()
    {
        transform.position = new Vector3(IndividualRockSpawner.transform.position.x, transform.position.y - speed * Time.fixedDeltaTime , transform.position.z);
        //myRigidbody2D.MovePosition(myRigidbody2D.position + speed * Vector2.down * Time.fixedDeltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (destroyLayers == (destroyLayers | (1 << collision.gameObject.layer)))
        {
            HandleDeath();
        }
    }
    void HandleDeath()
    {
        if (RockSpawnerScript != null) RockSpawnerScript.RockDestroyed(index);
        gameObject.SetActive(false);
    }
}
