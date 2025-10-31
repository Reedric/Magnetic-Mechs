using UnityEngine;
using System.Collections;

public class FlyingEnemy : MonoBehaviour
{

    [Header("Vertical Movement")]
    public float verticalSpeed = 2;
    public float startingY;
    public float switchTime = 2;
    public float switchCounter;
    public float verticalDirection = 1;
    public Transform upperVerticalBound;
    public Transform lowerVerticalBound;

    [Header("Tracking Player")]
    public Transform playerTransform;

    [Header("Components")]
    public Rigidbody2D myRigidBody2D;
    public BoxCollider2D myCollider;
    public Animator animator;
    public AudioSource DeathSound;
    public DroneRespawnerScript droneRespawnerScript;

    [Header("Sensors")]
    public float horizontalCheckLength = .65f;
    public float groundCheckHeight = 1f;

    [Header("Statistics")]
    public float health;
    public float startingHealth = 3;
    public int index = 0;
    private bool isAlive = true;
    private bool movementEnabled = true;

    private void Awake()
    {
        myRigidBody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        myCollider = GetComponent<BoxCollider2D>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        health = startingHealth;
        startingY = transform.position.y;
        movementEnabled = true;
        switchCounter = switchTime;
        upperVerticalBound.localPosition = new Vector3(0f, upperVerticalBound.localPosition.y, 0f);
        lowerVerticalBound.localPosition = new Vector3(0f, lowerVerticalBound.localPosition.y, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (movementEnabled)
        {
            if (transform.position.y < lowerVerticalBound.position.y) // too low
            {
                verticalDirection = 1;
                switchCounter = switchTime;
            }
            else if (transform.position.y > upperVerticalBound.position.y) // too high
            {
                verticalDirection = -1;
                switchCounter = switchTime;
            }
            if (droneRespawnerScript == null)
            {
                myRigidBody2D.linearVelocity = new Vector3(4f, verticalSpeed * verticalDirection, 0f);
            }
            else
            {
                myRigidBody2D.linearVelocity = new Vector3(0f, verticalSpeed * verticalDirection, 0f);
            }
        }

        if (switchCounter <= 0)
        {
            switchCounter = switchTime;
            if (Random.Range(0, 2) == 0)
            {
                verticalDirection = -verticalDirection;
            }
        }

        switchCounter -= Time.deltaTime;
    }
    private void FixedUpdate()
    {
        animator.SetBool("hasDied", false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            TakeDamage(1);
        }

        if (collision.gameObject.layer == 3 && playerTransform.GetComponent<PlayerScript>().isCharging)
        {
            TakeDamage(1);
        }
    }
    public void restartDrone()
    {
        health = startingHealth;
        animator.Play("Movement");
        isAlive = true;
        myCollider.enabled = true;
        movementEnabled = true;
        Debug.Log("Drone Restarted.");
    }

    void TakeDamage(float Damage)
    {
        health -= Damage;
        if (health <= 0)
        {
            KillFlyingEnemy();
        }
    }
    void KillFlyingEnemy()
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
        myCollider.enabled = false;
        myRigidBody2D.gravityScale = 0;
        StartCoroutine(HandleDeath());
    }
    IEnumerator HandleDeath()
    {
        yield return new WaitUntil(() => gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Dead"));
        animator.SetBool("hasDied", false);
        gameObject.SetActive(false);
        if (droneRespawnerScript != null)
        {
            droneRespawnerScript.DroneKilled(index);
        }
    }
}
