using UnityEngine;

public class Environment_PowerUpSetter : MonoBehaviour
{
    [Header ("Physics Related")]
    [SerializeField] private LayerMask whatIsBullet;

    enum PowerupType
    {
        bulletPowerup,
        shieldPowerup
    }

    [Header ("Enums")]
    [SerializeField] private PowerupType powerupType;

    private bool isCollided;

    private void Start ()
    {
        isCollided = false;

        InvokeRepeating ("DestroyBulletPowerUp", 4f, 4f);
    }

    private void OnTriggerEnter2D (Collider2D collision)
    {
        if ((whatIsBullet.value & (1 << collision.gameObject.layer)) > 0 && !isCollided)
        {
            isCollided = true;

            //Power up is colliding with bullet, give player power up now
            if (powerupType == PowerupType.bulletPowerup)
                Player_SpaceshipController.SharedInstance.isBulletPoweredUp = true;
            else if (powerupType == PowerupType.shieldPowerup)
                Player_SpaceshipController.SharedInstance.isShieldPoweredUp = true;

            //Disable the bullet
            collision.gameObject.SetActive (false);

            Destroy (gameObject);
        }
    }

    private void DestroyBulletPowerUp ()
    {
        Destroy (gameObject);
    }
}