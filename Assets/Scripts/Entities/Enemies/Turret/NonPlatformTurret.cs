using UnityEngine;

public class NonPlatformTurret : Turret
{
    [Tooltip("Angle of turret's chamber (and bullet angle);")]
    [SerializeField] public float firingAngle;
    void Start()
    {
        shootingAngle = firingAngle;
        SetUpTurret();
    }
}
