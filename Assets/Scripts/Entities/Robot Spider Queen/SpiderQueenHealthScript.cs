using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpiderQueenHealthScript : MonoBehaviour
{
    //manages health for the robot spider queen
    [Header("Components")]
    //public GameObject Player;
    private RobotSpiderQueenScript robotSpiderQueenScript;
    public Image remainingHealth;
    public LogicScript logic;
    [Header("Health")]
    public float currentHealth;
    public float maxHealth = 10;
    public float maxPossibleHealth = 100;
    [Header("Variables")]
    private bool inStage2 = false;
    private void Awake()
    {
        GameObject robotSpiderQueen = GameObject.FindGameObjectWithTag("RobotSpiderQueen");
        if (robotSpiderQueen != null) robotSpiderQueenScript = robotSpiderQueen.GetComponent<RobotSpiderQueenScript>();
        else Debug.Log("Robot Spider Queen could not be found");
        currentHealth = maxHealth;
        logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();
        //invincible = false;
    }
    void Start()
    {

    }
    public void takeDamage(float Damage)
    {
        loseHealth(Damage);
        updateHealthBar();
    }
    private void loseHealth(float Damage)
    {
        currentHealth -= Damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            HandleBossDeath();
        }
        else if(!inStage2 && currentHealth <= maxHealth/2) 
        {
            robotSpiderQueenScript.TriggerAllStage2();
            inStage2 = true;
        }
    }
    private void updateHealthBar()
    {
        remainingHealth.fillAmount = currentHealth/maxHealth;
    }
    public void HandleBossDeath()
    {
        if (robotSpiderQueenScript == null) return;
        logic.StartPostSpiderBossDelay();
        robotSpiderQueenScript.KillBoss();
    }
}
