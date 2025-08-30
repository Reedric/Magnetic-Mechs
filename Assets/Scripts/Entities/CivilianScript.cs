using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CivilianScript : MonoBehaviour
{
    [Header("Components")]
    public GameObject targetingReticle;

    public void turnOnTargetingReticle()
    {
        targetingReticle.GetComponent<SpriteRenderer>().enabled = true;
    }
}
