using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class MagnetSpawnerScript : MonoBehaviour
{
    //script for launching the magnet from the player
    private float LaunchForce = 35f;
    public float reloadTime = 1f;
    private float timer;
    private bool magnetDisabled = false;
    [Header("Components")]
    public GameObject magnetPrefab;
    public GameObject magnetSpawnpoint;
    //private AudioSource audioBox;

    public GameObject player;
    void Start()
    {
        timer = reloadTime;
        //audioBox = gameObject.GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player");
    }
    private void FixedUpdate()
    {
        timer = timer + Time.fixedDeltaTime;
    }
    public GameObject Launch()
    {
        if (player == null)
        {
            Debug.Log("The player could not be found");
            return null;
        }
        if (timer < reloadTime || magnetDisabled) return null;
        GameObject magnet = Instantiate(magnetPrefab, magnetSpawnpoint.transform.position + new Vector3(0,0,-1), transform.rotation);
        magnet.transform.Rotate(new Vector3(0, 0, 90));
        Rigidbody2D magnetRB = magnet.GetComponent<Rigidbody2D>();
        magnetRB.AddForce(transform.right * LaunchForce, ForceMode2D.Impulse);
        //audioBox.Play();
        timer = 0;
        return magnet;
    }
    //Events
    public void DisableShooting()
    {
        magnetDisabled = true;
    }
    public void EnableShooting()
    {
        magnetDisabled = false;
    }
}
