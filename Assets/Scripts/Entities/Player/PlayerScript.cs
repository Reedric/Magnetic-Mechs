using System;
using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.U2D;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    //main script for managing the player
    [Header("Components")]
    public Rigidbody2D myRigidbody2D;
    public Animator animator;
    public SpriteRenderer sprite;
    public PlayerHealthScript healthScript;
    public AudioSource jumpSound;
    public CutsceneManager cutsceneManagerScript;
    public PlankScript handlePlanks;
    public MultiSceneVariables savedVariables;
    public PlayerInput myInput;
    public GameObject pointerArrow;
    public LogicScript logic;
    public Image remainingFuelImage;
    public GameObject remainingFuelParent;
    public MagnetVisualEffectScript magnetVisualEffectScript;

    [Header("Logic")]
    private bool playerAlive = true;
    public bool gamePadNotMouse = false;

    [Header("Timers")]
    public float remainingFuelTimer = 0;
    private float remainingFuelTimeToDisappear = .5f;
    [Header("Drag Values")]
    private float defaultDrag = .05f;
    private float clampXDrag = 2.5f;
    private float clampYDrag = 3.0f;

    [Header("Horizontal Movement")]
    public float direction;
    private float baseSpeed = 15f;
    public float maxSpeed = 10f;
    public float horizontalSpeed;
    public bool facingRight = true;
    public bool movementDisabled;

    [Header("Vertical Movement")]
    public float verticalDirection;
    private float maxYSpeed = 20f;

    [Header("Handle Hits")]
    private float knockbackTime = 0.25f;
    const float invincibilityTimeDefault = .5f;


    [Header("Collision")]
    public bool onGround = false;
    public bool trulyOnGround = false;
    public float groundLength = .9f;
    private float legLength = .78f;
    public Vector3 distanceToLeg = new Vector3(.52f, 0, 0);
    public LayerMask groundLayer;

    [Header("Jumping")]
    public bool jumpPressed = false;
    public float jumpTimer;
    public float jumpDelay = .15f;
    public float maxYSpeedTimer;
    public float maxYSpeedDelay = .6f;
    public float jumpForce = 10f;

    [Header("Charging")]
    public bool chargePressed = false;
    public bool isCharging = false;
    public float chargeTime = 1f;
    private float chargeTimer = 0f;
    public float chargeCooldown = 3f;
    private float chargeCooldownTimer = 0f;
    public float chargeSpeed = 22f;
    public Sprite chargeIndicatorImage;
    public Sprite invincibilityIndicatorImage;
    public Sprite chargeCooldownIndicatorImage;
    public SpriteRenderer chargeIndicator;

    [Header("Jetpack")]
    public float jetpackTotalTime = 5f;
    public float jetpackCurrentTime = 0f;
    public float jetPackForce = 2f;
    public float maxJetSpeed = 5f;
    private float jetpackRecoveryTimer = 0f;
    private float jetpackRecoveryTime = 0.9f;
    private float jetPackTimeRecoveryMultiplier = .85f;
    private bool jetpackOn;
    public AudioSource jetpackAudio;

    [Header("Jetpack Components")]
    public GameObject jetpackLowerLeft;
    public GameObject jetpackLowerRight;
    public GameObject jetpackBackwards;
    public bool jetpackBackwardsOn;

    [Header("Physics")]
    public float linearDrag = 3f;
    public float gravity = 1f;
    public float fallMultiplier = 3f;
    public bool repelOn = false;
    public bool attractOn = false;

    [Header("Orientation")]
    public Camera virtualCamera;
    public Vector2 mousePosition;
    public Vector2 mouseRelativePosition;

    [Header("Input")]
    public GameObject BulletSpawner;
    public BulletSpawnerScript bulletSpawnerScript;
    public bool shootingInput = false;
    public Vector2 rightJoystick = Vector2.left;

    [Header("Magnet")]
    public GameObject MagnetSpawner;
    public MagnetSpawnerScript magnetSpawnerScript;
    private bool launchMagnet = false;
    private GameObject myMagnet;

    [Header("Dust Effects")]
    public ParticleSystem changeDirectionDust;
    public ParticleSystem DashingDust;

    [Header("Magnet")]
    private float magnetBaseForce = 75;
    private float maximumMagnetDistance = 30;
    public AudioSource magnetAudio;
    private void Awake()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        myInput = GetComponent<PlayerInput>();
        healthScript = GameObject.FindGameObjectWithTag("PlayerHealth").GetComponent<PlayerHealthScript>();
        virtualCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        GameObject savedVariablesObject = GameObject.FindGameObjectWithTag("MultiSceneVariables");
        if(savedVariablesObject != null)
        {
            gamePadNotMouse = savedVariablesObject.GetComponent<MultiSceneVariables>().gamePadNotMouse;
        }
        if (gamePadNotMouse)
        {
            myInput.defaultControlScheme = "Gamepad";
            pointerArrow.GetComponent<SpriteRenderer>().enabled = true;
        }
        else myInput.defaultControlScheme = "KeyboardMouse";
        BulletSpawner = GameObject.FindGameObjectWithTag("BulletSpawner");
        bulletSpawnerScript = BulletSpawner.GetComponent<BulletSpawnerScript>();
        MagnetSpawner = GameObject.FindGameObjectWithTag("MagnetSpawner");
        magnetSpawnerScript = MagnetSpawner.GetComponent<MagnetSpawnerScript>();
        groundLayer = LayerMask.GetMask("Ground","Plank Ground");
        jetpackCurrentTime = jetpackTotalTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerAlive || logic.IsPaused)
        {
            return;
        }
        mousePosition = virtualCamera.ScreenToWorldPoint(Input.mousePosition);//orientation
        if (verticalDirection <= -.25f && handlePlanks != null)
            {
            groundLayer = LayerMask.GetMask("Ground");
            handlePlanks.disablePlanks();
        }
        else
        {
            {
                groundLayer = LayerMask.GetMask("Ground", "Plank Ground");
            }
        }
        onGround = (Physics2D.Raycast(transform.position - distanceToLeg, Vector2.down, groundLength, groundLayer) || Physics2D.Raycast(transform.position + distanceToLeg, Vector2.down, groundLength, groundLayer));
        trulyOnGround = onGround && !(Physics2D.Raycast(transform.position - distanceToLeg, Vector2.down, legLength, groundLayer) || Physics2D.Raycast(transform.position + distanceToLeg, Vector2.down, legLength, groundLayer));
        animator.SetBool("OnGround", trulyOnGround);
        animator.SetBool("LandingFast", onGround && myRigidbody2D.linearVelocity.y < -5);
        //Method One Recover on Ground
        if (trulyOnGround)
        {
            jetpackCurrentTime = jetpackTotalTime;
            handleJetPackTime();
        }
        jetpackOn = jumpPressed && (!trulyOnGround);
        if (jetpackOn)
        {
            if (jetpackCurrentTime <= 0)
            {
                jetpackOn = false;
            }
            else
            {
                jetpackCurrentTime -= Time.deltaTime;
                jetpackRecoveryTimer = 0f;
                handleJetPackTime();
            }
        }
        //Method Two Recover whenever jetpack isn't in use
        if (!jetpackOn && jetpackCurrentTime < jetpackTotalTime)
        {
            jetpackRecoveryTimer += Time.deltaTime;
            if (jetpackRecoveryTimer > jetpackRecoveryTime)
            {
                jetpackCurrentTime += Time.deltaTime * 1.4f;
            }
            handleJetPackTime() ;
        }
        if (jumpPressed)
        {
            jumpTimer = Time.time + jumpDelay;
            if (trulyOnGround)
            {
                maxYSpeedTimer = Time.time + maxYSpeedDelay;
            }
        }
        if (jetpackAudio!= null) 
        {
            if (jetpackOn)
            {
                if(!jetpackAudio.isPlaying) jetpackAudio.Play();
            }
            else
            {
                jetpackAudio.Stop();
            }
        }
        if (magnetAudio != null)
        {
            if (repelOn ^ attractOn)
            {
                if (!magnetAudio.isPlaying) magnetAudio.Play();
            }
            else
            {
                magnetAudio.Stop();
            }
        }
        if (shootingInput)
        {
            bulletSpawnerScript.Shoot();
        }
        if (launchMagnet)
        {
            magnetSpawnerScript.Launch();
        }
        jetpackLowerLeft.GetComponent<JetpackScript>().setJetpack(jetpackOn);
        jetpackLowerRight.GetComponent<JetpackScript>().setJetpack(jetpackOn);
        jetpackBackwardsOn = !trulyOnGround && Mathf.Abs(direction) > 0;
        jetpackBackwards.GetComponent<JetpackScript>().setJetpack(jetpackBackwardsOn);
        //jumpPressed = false;
    }
    private void FixedUpdate()
    {
        handleGunOrientation();
        if (!playerAlive || movementDisabled)
        {
            animator.SetBool("hasDied", false);
            return;
        }
        modifyPhysics(jetpackOn);
        handleHorizontalMovement();
        handleVerticalMovement();
        handleMagneticRepulsion();
        handleRemainingFuelBar();
        handleCharging();
    }

    void handleHorizontalMovement()
    {
        animator.SetFloat("HorizontalInput", Mathf.Abs(direction));
        horizontalSpeed = baseSpeed * direction;
        myRigidbody2D.AddForce(Vector2.right * horizontalSpeed);
        if ((horizontalSpeed > 0 && !facingRight) || (horizontalSpeed < 0 && facingRight))
        {
            Flip();
        }
        adjustMaxXSpeed();
    }
    void adjustMaxXSpeed()
    {
        //turns on damping if the player is above a certain x speed
        float currentMaxSpeed = maxSpeed;
        if (repelOn ^ attractOn)
        {
            if (magnetSpawnerScript != null && magnetSpawnerScript.magnetActive)
            {
                Vector2 magnetRelativePosition = transform.position - myMagnet.transform.position;
                float magnetDistance = magnetRelativePosition.magnitude;
                if (magnetDistance < (maximumMagnetDistance))
                {
                    float angle = Mathf.Atan2(magnetRelativePosition.y, magnetRelativePosition.x);
                    float cosAngle = Mathf.Cos(angle);
                    float sign = MathF.Abs(myRigidbody2D.linearVelocity.x) / myRigidbody2D.linearVelocity.x;
                    if (attractOn) sign *= -1;//if attract is on then we want the + in the next step to be a minus
                    currentMaxSpeed = maxSpeed * MathF.Abs(sign + 4 * cosAngle * MathF.Sqrt((maximumMagnetDistance - magnetDistance) / maximumMagnetDistance));
                }
            }
        }
        if (MathF.Abs(myRigidbody2D.linearVelocity.x) > currentMaxSpeed && myRigidbody2D.linearDamping < clampXDrag)
        {
            //myRigidbody2D.velocity = new Vector2(MathF.Sign(myRigidbody2D.velocity.x) * currentMaxSpeed, myRigidbody2D.velocity.y);
            myRigidbody2D.linearDamping = clampXDrag;
        }
    }
    void adjustMaxYSpeed()
    {
        //turns on damping if the player is above a certain y speed
        float currentMaxSpeed = maxYSpeed;
        if (repelOn ^ attractOn)
        {
            if (myMagnet != null && magnetSpawnerScript.magnetActive)
            {
                Vector2 magnetRelativePosition = transform.position - myMagnet.transform.position;
                float magnetDistance = magnetRelativePosition.magnitude;
                if (magnetDistance < (maximumMagnetDistance))
                {
                    float angle = Mathf.Atan2(magnetRelativePosition.y, magnetRelativePosition.x);
                    float sinAngle = Mathf.Sin(angle);
                    float sign = MathF.Abs(myRigidbody2D.linearVelocity.y) / myRigidbody2D.linearVelocity.y;
                    if (attractOn) sign *= -1;//if attract is on then we want the + in the next step to be a minus
                    currentMaxSpeed = maxYSpeed * MathF.Abs(sign + 4 * sinAngle * MathF.Sqrt((maximumMagnetDistance - magnetDistance) / maximumMagnetDistance));
                }
            }
        }
        if (MathF.Abs(myRigidbody2D.linearVelocity.y) > currentMaxSpeed && myRigidbody2D.linearDamping < clampYDrag)
        {
            //myRigidbody2D.velocity = new Vector2(myRigidbody2D.velocity.x, MathF.Sign(myRigidbody2D.velocity.y) * currentMaxSpeed);
            myRigidbody2D.linearDamping = clampYDrag;
        }
    }
    private void Flip()
    {
        facingRight = !facingRight;
        transform.rotation = Quaternion.Euler(0, facingRight ? 0 : 180, 0);
        remainingFuelImage.transform.rotation = Quaternion.Euler(0, 0, 0);
    }
    public void DisableMovement()
    {
        movementDisabled = true;
        myRigidbody2D.linearVelocity = Vector3.zero;
        animator.SetFloat("HorizontalInput", 0);
    }
    public void EnableMovement()
    {
        movementDisabled = false;
    }
    void handleVerticalMovement()
    {
        //handles checks and related to vertical movement once every Update cycle
        if (trulyOnGround && jumpTimer > Time.time)
        {
            jump();
        }
        if (!trulyOnGround && jetpackOn)
        {
            myRigidbody2D.AddForce(new Vector2(0, jetPackForce));

            if (myRigidbody2D.linearVelocity.y > maxJetSpeed && maxYSpeedTimer < Time.time)
            {
                if (repelOn || attractOn)
                {
                    return;
                }
                if (myRigidbody2D.linearDamping < clampYDrag) myRigidbody2D.linearDamping = clampYDrag/2;
                //myRigidbody2D.velocity = new Vector2(myRigidbody2D.velocity.x, maxJetSpeed);
            }
        }
        adjustMaxYSpeed();
    }
    private void jump()
    {
        myRigidbody2D.linearVelocity = new Vector2(myRigidbody2D.linearVelocity.x, 0);
        myRigidbody2D.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        jumpSound.Play();
        //Instantiate(JumpDust, new Vector3(transform.position.x, transform.position.y - groundLength * 3 / 4, transform.position.z), transform.rotation);
        jumpTimer = 0;
    }
    private void handleGunOrientation()
    {
        if (gamePadNotMouse)
        {
            BulletSpawner.transform.right = rightJoystick;
            MagnetSpawner.transform.right = rightJoystick;
        }
        else
        {
            mouseRelativePosition = mousePosition - myRigidbody2D.position;
            BulletSpawner.transform.right = mouseRelativePosition;
            MagnetSpawner.transform.right = mouseRelativePosition;
        }
    }
    private void modifyPhysics(bool jetpackOn)
    {
        bool changingDirection = (direction > 0 && myRigidbody2D.linearVelocity.x < 0) || (direction < 0 && myRigidbody2D.linearVelocity.x > 0);
        if (trulyOnGround)
        {
            myRigidbody2D.gravityScale = 0;
            if (MathF.Abs(direction) == 0f || changingDirection)
            {
                myRigidbody2D.linearDamping = linearDrag;
                if (changingDirection)
                {
                    CreateDust();
                }
            }
            else
            {
                myRigidbody2D.linearDamping = defaultDrag;
            }
        }
        else
        {
            myRigidbody2D.gravityScale = gravity;
            myRigidbody2D.linearDamping = linearDrag * .15f;
            if (jetpackOn)
            {
                myRigidbody2D.gravityScale = gravity / 2;
            }
            if (myRigidbody2D.linearVelocity.y < 0f)
            {
                if (jetpackOn)
                {
                    myRigidbody2D.linearDamping = linearDrag;
                }
                else
                {
                    myRigidbody2D.gravityScale = gravity * fallMultiplier;
                }
            }
            else if (myRigidbody2D.linearVelocity.y > 0f && !jetpackOn)
            {
                myRigidbody2D.gravityScale = gravity * fallMultiplier / 2;
            }

        }
    }

    private void handleCharging()
    {
        // check if we have hit charging speed
        if (myRigidbody2D.linearVelocity.magnitude >= chargeSpeed && !healthScript.invincible)
        {
            // if charging cooldown is over and we aren't charging
            if (!isCharging && chargeCooldownTimer < 0f)
            {
                chargeIndicator.sprite = chargeIndicatorImage;

                // start charging if we press the button
                if (chargePressed)
                {
                    chargeTimer = chargeTime;

                    isCharging = true;
                    chargeIndicator.sprite = invincibilityIndicatorImage;
                }
            }
        }
        else
        {
            // too slow for charge speed and not charging
            if (!isCharging)
            {
                chargeIndicator.sprite = null;
            }
        }
        
        // if we stop charging
        if (chargeTimer <= 0f && isCharging)
        {
            isCharging = false;
            chargeCooldownTimer = chargeCooldown;
        }

        // if cooldown is happening
        if (chargeCooldownTimer > 0f)
        {
            chargeIndicator.sprite = chargeCooldownIndicatorImage;
        }

        chargeTimer -= Time.deltaTime;
        chargeCooldownTimer -= Time.deltaTime;
    }

    private void CreateDust()
    {
        changeDirectionDust.Play();
    }
    public void DamagePlayer(float Damage, Vector2 knockbackDirection, float knockback = 0, float invincibilityTime = invincibilityTimeDefault)
    {
        healthScript.takeDamage(Damage, knockbackDirection, knockback,invincibilityTime);
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!isCharging)
        {
            if (collision.gameObject.layer == 7) // enemy
            {
                Vector2 relativePosition = transform.position - collision.transform.position;
                float knockbackVal = 1;
                if (collision.gameObject.tag == "RobotSpiderQueen")
                {
                    knockbackVal = 1.5f;
                    if (relativePosition.y > Math.Abs(relativePosition.x) / .9f)
                    {
                        knockbackVal = 3.25f;
                    }
                }
                DamagePlayer(1, relativePosition.normalized, knockbackVal);
            }
            if (collision.gameObject.layer == 12) // death pit
            {
                //Vector2 relativePosition = transform.position - collision.transform.position;
                DamagePlayer(16, new Vector2(0, 0));
            }
            if (collision.gameObject.layer == 19) // spike
            {
                Vector2 relativePosition = transform.position - collision.transform.position;
                float knockbackVal = .5f;
                if (relativePosition.y > Math.Abs(relativePosition.x) / .9f)
                {
                    knockbackVal = 1.25f;
                }
                DamagePlayer(1, relativePosition.normalized, knockbackVal);
            }
        }
        else
        {
            if (collision.gameObject.layer == 7) // enemy
            {
                Vector2 relativePosition = transform.position - collision.transform.position;
                float knockbackVal = 1;
                if (collision.gameObject.tag == "RobotSpiderQueen")
                {
                    knockbackVal = 1.5f;
                    if (relativePosition.y > Math.Abs(relativePosition.x) / .9f)
                    {
                        knockbackVal = 3.25f;
                    }
                }

                StartCoroutine(handleKnockback(knockbackVal, relativePosition.normalized));
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isCharging)
        {
            if (collision.gameObject.layer == 13) //enemy bullet
            {
                Vector2 relativePosition = transform.position - collision.transform.position;
                DamagePlayer(1, relativePosition.normalized, .5f);
            }
            if (collision.gameObject.layer == 16) // rock
            {
                Vector2 relativePosition = transform.position - collision.transform.position;
                DamagePlayer(1, relativePosition.normalized, 1f);
            }
            if (collision.gameObject.layer == 7) // enemy
            {
                Vector2 relativePosition = transform.position - collision.transform.position;
                DamagePlayer(1, relativePosition.normalized, .5f);
            }
        }
    }
    public IEnumerator handleKnockback(float knockback, Vector2 knockbackDirection)
    {
        //movementEnabled = false;
        myRigidbody2D.AddForce(knockbackDirection * knockback * 10, ForceMode2D.Impulse);
        sprite.color = Color.red;
        yield return new WaitForSeconds(knockbackTime);
        //movementEnabled = true;
        sprite.color = Color.white;
    }
    public void handleJetPackTime()
    {
        int remainingFuelPercent = (int)(100 * jetpackCurrentTime / jetpackTotalTime);
        if (remainingFuelPercent < 0)
        {
            remainingFuelPercent = 0;
        }
        remainingFuelImage.fillAmount = remainingFuelPercent / 100.0f;
    }
    public void handleRemainingFuelBar()
    {
        if (jetpackCurrentTime >= jetpackTotalTime)
        {
            remainingFuelTimer += Time.deltaTime * jetPackTimeRecoveryMultiplier;
            if (remainingFuelTimer > remainingFuelTimeToDisappear)
            {
                remainingFuelParent.SetActive(false);
            }
        }
        else
        {
            remainingFuelParent.SetActive(true);
            remainingFuelTimer = 0;
        }
    }
    private void handleMagneticRepulsion()
    {
        if (myMagnet == null || !(repelOn ^ attractOn) || !magnetSpawnerScript.magnetActive) return;
        Vector2 magnetRelativePosition = transform.position - myMagnet.transform.position;
        float magnetDistance = magnetRelativePosition.magnitude;
        if (magnetDistance < 1.1f)
        {
            magnetDistance = 1.1f;
        }
        if (magnetDistance < (maximumMagnetDistance))
        {
            magnetVisualEffectScript.StartMagnetEffect(repelOn);
            repulse(magnetRelativePosition.normalized, magnetBaseForce / (float)Math.Sqrt(magnetDistance));
        }
    }
    void repulse(Vector2 forceDirection, float forceMagnitude)
    {
        if (attractOn) forceMagnitude *= -1.25f;
        myRigidbody2D.AddForce(forceDirection * forceMagnitude, ForceMode2D.Force);
    }
    public void KillPlayer()
    {
        if (!playerAlive)
        {
            return;
        }
        playerAlive = false;
        animator.SetBool("hasDied", true);
        myRigidbody2D.linearVelocity = new Vector3(0, 0, 0);
        StartCoroutine(HandleDeath());
    }
    IEnumerator HandleDeath()
    {
        yield return new WaitUntil(() => gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Dead"));
        gameObject.SetActive(false);
    }
    public void setMagnet(GameObject magnet)
    {
        myMagnet = magnet;
        magnetVisualEffectScript.Magnet = magnet;
    }
    public void Move(InputAction.CallbackContext context)
    {
       direction = context.ReadValue<Vector2>().x;
       verticalDirection = context.ReadValue<Vector2>().y;
    }
    public void Aim(InputAction.CallbackContext context)
    {
        if(Mathf.Abs(context.ReadValue<Vector2>().x) > .1 || Mathf.Abs(context.ReadValue<Vector2>().y) > .1)
        {
            rightJoystick = context.ReadValue<Vector2>();
        }
    }
    public void JumpInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            jumpPressed = true;
            if (cutsceneManagerScript != null)
            {
                cutsceneManagerScript.SkipCutscene();
            }
        }
        if (context.canceled)
        {
            jumpPressed = false;
        }
    }

    public void ChargeInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            chargePressed = true;
        }
        if (context.canceled)
        {
            chargePressed = false;
        }
    }

    public void ShootingInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            shootingInput = true;
        }
        if (context.canceled)
        {
            shootingInput = false;
        }
    }
    public void LaunchMagnet(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            launchMagnet = true;
        }
        if (context.canceled)
        {
            launchMagnet = false;
        }
    }
    public void MagnetRepel(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            repelOn = true;
        }
        if (context.canceled)
        {
            repelOn = false;
        }
    }
    public void MagnetAttract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            attractOn = true;
        }
        if (context.canceled)
        {
            attractOn = false;
        }
    }
    public void Pause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            logic.Pause();
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position - distanceToLeg, transform.position - distanceToLeg + Vector3.down * legLength);
        Gizmos.DrawLine(transform.position + distanceToLeg, transform.position + distanceToLeg + Vector3.down * groundLength);
    }
}
