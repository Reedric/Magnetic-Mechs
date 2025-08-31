using System.Collections;
using System.Collections.Generic;
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
        deathTime = Time.time + 3;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //GameObject effect = Instantiate(explosionEffect, transform.position + explosionOffset, Quaternion.identity);
        //Destroy(effect, effect.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
        if(parent!= null && collision.gameObject != parent && collision.gameObject.layer != 8 && collision.gameObject.layer != 13 && collision.gameObject.layer != 14 && collision.gameObject.layer != 5)
        {
            KillBullet();
        }
    }
    public void KillBullet()
    {
        if (bulletSpawnerParent != null) 
        {
            bulletSpawnerParent.BulletKilled(index);
        }
        gameObject.SetActive(false);
    }
}
