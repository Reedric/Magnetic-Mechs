using UnityEngine;

public class Turret : BulletSpawnerParent
{
    // Store this script in the turret chamber

    [Header("Object Components")]

    [SerializeField] private GameObject turretBase;
    [SerializeField] private GameObject turretHolder;

    [Header("Enemy Specifications")]

    [Tooltip("Angle of turret's base")]
    [SerializeField] private float baseAngle;

    [Tooltip("Angle of turret's chamber (and bullet angle)")]
    [SerializeField] private float shootingAngle;

    [SerializeField] private float timeBetweenShots;
    private float timer;
    [SerializeField] private float missileForce;

    void Start()
    {
        turretBase.transform.eulerAngles = new Vector3(0f, 0f, baseAngle);
        transform.eulerAngles = new Vector3(0f, 0f, shootingAngle);

        bulletForce = missileForce * 10000;
        timer = timeBetweenShots;
        audioBox = gameObject.GetComponent<AudioSource>();
        parentObject = turretHolder;
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
