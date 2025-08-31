using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetProjectileScript : MonoBehaviour
{
    //code for the magnet projectile
    private float deathTime;
    private float startCollidingTime;
    private Rigidbody2D myRigidBody;
    private void Awake()
    {
        deathTime = Time.time + 10;
        myRigidBody = GetComponent<Rigidbody2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6 || collision.gameObject.layer == 17)
        {
            myRigidBody.linearVelocity = Vector3.zero;
            GetComponent<CapsuleCollider2D>().enabled = false;
        }
    }
    public void DestroyThis()
    {
        Destroy(gameObject);
    }
}
