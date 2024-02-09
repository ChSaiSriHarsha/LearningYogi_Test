using UnityEngine;

[CreateAssetMenu (fileName = "SpaceShip_Attributes", menuName = "Player_SO/Spaceship_Attributes")]
public class Player_SpaceshipScriptableObject : ScriptableObject
{
    [Header ("Movement Related")]
    public float so_Acceleration;
    public float so_MaximumVelocity;
    public float so_RotationSpeed;

    [Header ("Shooting Related")]
    public float so_BulletSpeed;
    public float so_BulletDeactivateTime;

    [Header ("Health Related")]
    public int so_MaxHealth = 3;

    [Header ("Power Up Related")]
    public float so_TimeToInvoke = 3f;
    public float so_RepeatRate = 3f;
}
