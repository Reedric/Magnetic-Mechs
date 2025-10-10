using UnityEngine;

public class FirewallScript : MonoBehaviour
{
    [Header("Components")]
    public PlayerScript playerScript;
    [Header("variables")]
    private float defaultDamage = .5f;
    private float defaultInvincibleTime = .25f;
    private void Awake()
    {
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 3)
        {
            DealDamage(collision.gameObject);
        }
    }
    void DealDamage(GameObject ObjectHit)
    {
        if (playerScript != null)
        {
            playerScript.DamagePlayer(defaultDamage, new Vector2(1, 1), 1, defaultInvincibleTime);
        }
    }
}
