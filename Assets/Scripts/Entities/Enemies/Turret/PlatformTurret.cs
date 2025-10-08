using UnityEngine;

public class PlatformTurret : Turret
{
    [Tooltip("Is the turret shooting left?")]
    [SerializeField] public bool facingLeft;
    void Start()
    {
        shootingAngle = facingLeft ? 180 : 0;
        SetUpTurret();
    }
}
