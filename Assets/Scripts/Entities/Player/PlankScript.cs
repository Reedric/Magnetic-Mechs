using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlankScript : MonoBehaviour
{
    [Header("Layers")]
    public LayerMask playerLayer;
    public LayerMask plankGroundLayer;
    [Header("Variables")]
    public float disableCollisionTimer;
    public float disablePlankTime = .25f;
    void Awake()
    {
        playerLayer = LayerMask.NameToLayer("Player");
        plankGroundLayer = LayerMask.NameToLayer("Plank Ground");
        disableCollisionTimer = disablePlankTime;
    }
    void FixedUpdate()
    {
        if(disableCollisionTimer < disablePlankTime)
        {
            Physics2D.IgnoreLayerCollision(playerLayer, plankGroundLayer, true);
            disableCollisionTimer += Time.fixedDeltaTime;
        }
        else
        {
            Physics2D.IgnoreLayerCollision(playerLayer, plankGroundLayer, false);
        }
    }
    public void disablePlanks()
    {
        disableCollisionTimer = 0;
    }
}
