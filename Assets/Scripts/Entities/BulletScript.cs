using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    //this is a test
    private float deathTime;
    private float startCollidingTime;
    [Header("Components")]
    public GameObject parent;
    public int index;
    public BulletSpawnerParent bulletSpawnerParent;
    //private GameObject explosionEffect;
    //private Vector3 explosionOffset = new Vector3(0, .05f, 0);

    private bool isPlatformMissile;

    void Start()
    {
        isPlatformMissile = gameObject.GetComponentInChildren<MissilePlatformManager>() != null;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > deathTime)
        {
            KillBullet();
        }
        //if (Time.time > startCollidingTime)
        //{
        //    GetComponent<CapsuleCollider2D>().enabled = true;
        //}
    }
    public void SetDeathTime()
    {
        isPlatformMissile = gameObject.GetComponentInChildren<MissilePlatformManager>() != null; // update this again after starting

        //sets time to kill the bullet if it hasn't hit anything yet
        if (isPlatformMissile)
        {
            deathTime = Time.time + 10;
        }
        else
        {
            deathTime = Time.time + 3;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //GameObject effect = Instantiate(explosionEffect, transform.position + explosionOffset, Quaternion.identity);
        //Destroy(effect, effect.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
        if(parent!= null && collision.gameObject != parent && collision.gameObject.layer != 8 && collision.gameObject.layer != 13 && collision.gameObject.layer != 14 && collision.gameObject.layer != 5)
        {
            if (isPlatformMissile)
            {
                if (collision.gameObject.tag != "Magnet")
                {
                    KillBullet();
                }
            }
            else
            {
                KillBullet();
            }
        }
    }
    public void KillBullet()
    {
        if (bulletSpawnerParent != null) 
        {
            bulletSpawnerParent.BulletKilled(index);
        }

        // if this bullet is a platform turret missile, reset any magnets attached to platforms
        if (isPlatformMissile)
        {
            foreach (Transform child in gameObject.GetComponentInChildren<MissilePlatformManager>().transform)
            {
                if (child.GetComponent<MagnetProjectileScript>() != null)
                {
                    child.GetComponent<MagnetProjectileScript>().DestroyThis();
                }
            }
        }

        gameObject.SetActive(false);
    }
}
