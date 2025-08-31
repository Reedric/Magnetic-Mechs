using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RockScript : MonoBehaviour
{
    //script for the rocks which spawn during the robot spider queens second phase
    [Header("Components")]
    public Rigidbody2D myRigidbody2D;
    [Header("variables")]
    public float speed = 3;
    public LayerMask destroyLayers;
    void Awake()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
        destroyLayers = LayerMask.GetMask("Player", "Player Bullet");
    }
    private void Update()
    {
        if (transform.position.y < -3)
        {
            Destroy(gameObject);
        }
    }
    void FixedUpdate()
    {
        myRigidbody2D.MovePosition(myRigidbody2D.position + speed * Vector2.down * Time.fixedDeltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (destroyLayers == (destroyLayers | (1 << collision.gameObject.layer)))
        {
            Destroy(gameObject);
        }
    }
}
