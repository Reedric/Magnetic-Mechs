using UnityEngine;

public class MissilePlatformManager : MonoBehaviour
{
    private BoxCollider2D platformCollider;
    void Start()
    {
        platformCollider = GetComponent<BoxCollider2D>();
        platformCollider.enabled = (transform.eulerAngles.z == 0 || transform.eulerAngles.z == 180);
    }

    private void FixedUpdate()
    {
        transform.localPosition = Vector3.zero;
    }
}
