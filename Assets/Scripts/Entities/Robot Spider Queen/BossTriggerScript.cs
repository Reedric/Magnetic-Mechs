using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTriggerScript : MonoBehaviour
{
    [Header("Components")]
    public RobotSpiderQueenScript RobotSpiderQueenScript;
    //public RockSpawnerScript rockSpawnerScript;
    public GoomechSpawnerScript goomechSpawnerScriptTop;
    public GoomechSpawnerScript goomechSpawnerScriptBottom;
    public SpiderQueenBulletSpawnerScript spiderQueenBulletSpawnerScript;
    public GameObject bossCanvas;
    public GameObject entrances;
    public GameObject virtualCameraPlayer;
    public GameObject virtualCameraBoss;
    //public GameObject cameraPosition;
    public AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (RobotSpiderQueenScript == null || goomechSpawnerScriptTop == null || goomechSpawnerScriptBottom == null || bossCanvas == null || spiderQueenBulletSpawnerScript == null || entrances == null || virtualCameraPlayer == null || virtualCameraBoss == null || audioSource == null) return;
        if (collision.gameObject.layer == 3)
        {
            RobotSpiderQueenScript.bossActive = true;
            goomechSpawnerScriptTop.bossActive = true;
            goomechSpawnerScriptBottom.bossActive = true;
            bossCanvas.SetActive(true);
            spiderQueenBulletSpawnerScript.EnableShooting();
            entrances.SetActive(true);
            //virtualCamera.Follow = cameraPosition.transform;
            //virtualCamera.m_Lens.OrthographicSize = 10;
            virtualCameraPlayer.SetActive(false);
            virtualCameraBoss.SetActive(true);
            AudioClip loadedClip = Resources.Load<AudioClip>("BackgroundMusic/the_robot_spider_queen_invasion_Part2");
            if (loadedClip != null)
            {
                audioSource.clip = loadedClip;
                audioSource.Play();
            }
            gameObject.SetActive(false);
        }
    }
}
