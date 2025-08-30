using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using UnityEngine;

public class BulletSpawnerScript : BulletSpawnerParent
{
    [Header("Variables")]
    private float reloadTime = .55f;
    private float timer;
    private bool shootingDisabled = false;
    [Header("Components")]
    //public GameObject bulletPrefab;
    //public GameObject MuzzlePrefab;
    //public GameObject bulletSpawnpoint;
    //public GameObject muzzleSpawnpoint;
    //private AudioSource audioBox;
    public GameObject player;
    //public Animator animator;
    void Start()
    {
        bulletForce = 28f;
        timer = reloadTime;
        audioBox = gameObject.GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player");
        parentObject = player;
        SetUpGameObjects();
    }
    private void FixedUpdate()
    {
        timer = timer + Time.fixedDeltaTime;
    }
    public void Shoot()
    {
        if ( player == null)
        {
            Debug.Log("The player could not be found");
            return;
        }
        if (timer < reloadTime || shootingDisabled || bulletsQueue.Count ==0) return;
        SpawnBullet();
        SpawnMuzzleEffect();
        audioBox.Play();
        timer = 0;
        //animator.SetBool("Shoot", true);
    }
    //Events
    public void DisableShooting()
    {
        shootingDisabled = true;
    }
    public void EnableShooting()
    {
        shootingDisabled = false;
    }

}
