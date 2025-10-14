using UnityEngine;

public class MissilePlatformManager : MonoBehaviour
{
    private BoxCollider2D platformCollider;
    [SerializeField] private BulletScript bulletScript;
    void Start()
    {
        platformCollider = GetComponent<BoxCollider2D>();
        platformCollider.enabled = (transform.eulerAngles.z == 0 || transform.eulerAngles.z == 180);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (bulletScript.parent != null && (collision.gameObject.layer == 6 || collision.gameObject.layer == 17))
        {
            bulletScript.KillBullet();
        }
    }
}
