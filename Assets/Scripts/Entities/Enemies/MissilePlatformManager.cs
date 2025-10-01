using UnityEngine;

public class MissilePlatformManager : MonoBehaviour
{
    private BoxCollider2D platformCollider;
    [SerializeField] private BulletScript bulletScript;
    void Start()
    {
        platformCollider = GetComponent<BoxCollider2D>();
        platformCollider.enabled = (transform.eulerAngles.z == 0 || transform.eulerAngles.z == 180);
        if (bulletScript.parent.GetComponent<Turret>() != null)
        {
            platformCollider.enabled = bulletScript.parent.GetComponent<Turret>().canStandOnMissiles;
            gameObject.tag = bulletScript.parent.GetComponent<Turret>().canStickMagnets ? "MovingPlatform" : "NonStickPlatform";
        }
    }

    private void FixedUpdate()
    {
        transform.localPosition = Vector3.zero;
    }
}
