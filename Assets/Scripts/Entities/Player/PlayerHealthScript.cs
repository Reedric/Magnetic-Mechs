using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class PlayerHealthScript : MonoBehaviour
{
    //script for managing the players health
    [Header("Components")]
    public GameObject Player;
    private PlayerScript playerScript;
    public GameObject HealthSystem;
    public LogicScript Logic;
    [Header("Health")]
    public float currentHealth;
    private float maxHealth = 10;
    public float maxPossibleHealth = 16;
    [Header("Invincibility")]
    const float invincibilityTimeDefault = .5f;
    public bool invincible;
    private void Awake()
    {
        Logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();
        currentHealth = maxHealth;
        invincible = false;
    }
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        if(Player!=null) playerScript = Player.GetComponent<PlayerScript>();
    }
    public void takeDamage(float Damage, Vector2 knockbackDirection, float knockback = 0, float invincibilityTime = invincibilityTimeDefault)
    {
        if(playerScript==null)
        {
            Debug.Log("Player could not be found");
            return;
        }
        if (!invincible)
        {
            StartCoroutine(handleDamage(Damage, invincibilityTime));
            StartCoroutine(playerScript.handleKnockback(knockback, knockbackDirection));
        }
    }
    IEnumerator handleDamage(float Damage, float invincibilityTime)
    {
        invincible = true;
        loseHealth(Damage);
        yield return new WaitForSeconds(invincibilityTime);
        invincible = false;
    }
    private void loseHealth(float Damage)
    {
        currentHealth -= Damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            HandlePlayerDeath();
        }
        HealthSystem.GetComponent<HealthHeartScript>().UpdateHeartsHUD();
    }
    public void HandlePlayerDeath()
    {
        playerScript.KillPlayer();
        Logic.GameOver();
    }
    public float getMaxHealth()
    {
        return maxHealth;
    }
    public void healToFull()
    {
        currentHealth = maxHealth;
        HealthSystem.GetComponent<HealthHeartScript>().UpdateHeartsHUD();
    }
}
