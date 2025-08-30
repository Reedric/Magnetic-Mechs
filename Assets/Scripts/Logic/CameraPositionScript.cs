using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPositionScript : MonoBehaviour
{
    [Header("Components")]
    public Transform playerTransform;
    public Transform bossTransform;

    private void FixedUpdate()
    {
        if (playerTransform == null || bossTransform == null) return;
        transform.position = (playerTransform.position*2 + bossTransform.position)/3;
    }
}
