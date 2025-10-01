using System;
using UnityEngine;

public class Turret : BulletSpawnerParent
{
    // Store this script in the turret chamber
    // Inherits from the bullet spawner parent script, similar to BulletSpawnerScript

    [Header("Object Components")]

    [SerializeField] private GameObject turretHolder;
    [SerializeField] private GameObject turretChamber;

    [Header("Enemy Specifications")]

    [Tooltip("Angle of turret's base")]
    [SerializeField] private float baseAngle;

    protected float shootingAngle = 0f;

    [SerializeField] private float timeBetweenShots;
    private float timer;
    [SerializeField] private float missileForce;

    private void Start()
    {
        SetUpTurret();
    }

    public void SetUpTurret()
    {
        // Set the base and shooting angles based on specified values
        turretHolder.transform.eulerAngles = new Vector3(0f, 0f, baseAngle);
        transform.eulerAngles = new Vector3(0f, 0f, shootingAngle);

        // increase bullet force to account for large missile mass for letting players walk on bullet
        bulletForce = missileForce * 10000;
        timer = timeBetweenShots;
        audioBox = gameObject.GetComponent<AudioSource>();
        parentObject = turretChamber;
        SetUpGameObjects();
    }

    private void FixedUpdate()
    {
        if (timer <= 0f && bulletsQueue.Count != 0)
        {
            SpawnBullet();
            SpawnMuzzleEffect();
            audioBox.Play();
            timer = timeBetweenShots;
        }

        timer -= Time.fixedDeltaTime;
    }
}
