using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DroneHorizontalScript : MonoBehaviour
{
    //the script for individual drones which fly horizontally
    [Header("Horizontal Movement")]
    public float speed = 4;
    public float speedModifier = 2;
    public bool facingRight;

    [Header("Components")]
    public Rigidbody2D myRigidBody2D;
    public SpriteRenderer sprite;
    public LayerMask enemyLayer;
    public BoxCollider2D myCollider;
    public Animator animator;
    public AudioSource DeathSound;
    public Transform DroneSpawnerTransform;
    public DroneSpawnerScript DroneSpawnerScript;

    [Header("Statistics")]
    public float health;
    private float startingHealth = 1;
    private bool isAlive = true;
    public int index;

    [Header("Knockback")]
    private float knockbackTime = 0.20f;
    public bool movementEnabled;

    [Header("Variables")]
    public float MaxXDistance = 15f;
    public float MaxYDistance = 15f;
    private void Awake()
    {
        myRigidBody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        myCollider = GetComponent<BoxCollider2D>();
        facingRight = true;
        health = startingHealth;
        movementEnabled = true;
    }
    public void restartDrone()
    {
        health = startingHealth;
        animator.Play("Movement");
        isAlive = true;
        myCollider.enabled = true;
        movementEnabled = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (movementEnabled)
        {
            myRigidBody2D.linearVelocity = new Vector3(speed * (facingRight ? 1 : -1) + speedModifier, myRigidBody2D.linearVelocity.y, 0);
        }
        CheckDespawn();
    }
    private void FixedUpdate()
    {
        animator.SetBool("hasDied", false);
    }
    public void Flip()
    {
        facingRight = !facingRight;
        transform.rotation = Quaternion.Euler(0, facingRight ? 0 : 180, 0);
        myRigidBody2D.linearVelocity = new Vector3(-myRigidBody2D.linearVelocity.x, myRigidBody2D.linearVelocity.y, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            TakeDamage(1, collision.transform.up.normalized, .25f);
        }
        if (collision.gameObject.layer == 18)
        {
            DespawnDrone();
        }
    }
    void TakeDamage(float Damage, Vector2 knockbackDirection, float knockback)
    {
        health -= Damage;
        if (health <= 0)
        {
            KillDrone();
        }
        else
        {
            StartCoroutine(handleKnockback(knockback, knockbackDirection));
        }
    }
    IEnumerator handleKnockback(float knockback, Vector2 knockbackDirection)
    {
        movementEnabled = false;
        myRigidBody2D.AddForce(knockbackDirection * knockback, ForceMode2D.Impulse);
        sprite.color = Color.red;
        yield return new WaitForSeconds(knockbackTime);
        movementEnabled = true;
        sprite.color = Color.white;
    }
    void CheckDespawn()
    {
        if(DroneSpawnerTransform == null)
        {
            Debug.Log("Drone Spawned without a drone spawner");
            return;
        }
        float leftDespawn = DroneSpawnerTransform.position.x - MaxXDistance;
        float rightDespawn = DroneSpawnerTransform.position.x + MaxXDistance;
        float bottomDespawn = DroneSpawnerTransform.position.x - MaxYDistance;
        float topDespawn = DroneSpawnerTransform.position.x + MaxYDistance;
        if (transform.position.x < leftDespawn || transform.position.x > rightDespawn || transform.position.x < bottomDespawn || transform.position.x > topDespawn)
        {
            DespawnDrone();
        }
    }
    void KillDrone()
    {
        if (!isAlive)
        {
            return;
        }
        movementEnabled = false;
        isAlive = false;
        animator.SetBool("hasDied", true);
        myRigidBody2D.linearVelocity = new Vector3(0, 0, 0);
        DeathSound.Play();
        //gameObject.layer = 9;
        //Debug.Log(gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
        //Destroy(gameObject, gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
        myCollider.enabled = false;
        StartCoroutine(HandleDeath());
    }
    void DespawnDrone()
    {
        
        if (DroneSpawnerScript != null)
        {
            DroneSpawnerScript.DroneKilled(index);
        }
        gameObject.SetActive(false);
    }
    IEnumerator HandleDeath()
    {
        yield return new WaitUntil(() => gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Dead"));
        animator.SetBool("hasDied", false);
        if (DroneSpawnerScript != null)
        {
            DroneSpawnerScript.DroneKilled(index);
        }
        gameObject.SetActive(false);
    }
}
