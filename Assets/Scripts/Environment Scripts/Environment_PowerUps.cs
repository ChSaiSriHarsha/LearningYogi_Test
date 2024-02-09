using UnityEngine;

public class Environment_PowerUps : MonoBehaviour
{
    [Header ("References")]
    [SerializeField] private Player_SpaceshipScriptableObject spaceshipScriptableObject;

    [Header ("GameObjects")]
    [SerializeField] private GameObject bulletPowerUp;
    [SerializeField] private GameObject shieldPowerUp;

    [Header ("Transforms")]
    [SerializeField] private Transform parentTransform;

    #region INITIALIZATION

    private void Start ()
    {
        InvokeRepeating
            ("SpawnPowerUps", spaceshipScriptableObject.so_TimeToInvoke, spaceshipScriptableObject.so_RepeatRate);
    }

    #endregion

    #region SPAWN AND MOVE LOGIC

    private void SpawnPowerUps ()
    {
        int randomPowerup = Random.Range (0, 5);

        if (randomPowerup == 0)
        {
            //We spawn bullet power up
            GameObject powerupGO = Instantiate (bulletPowerUp, parentTransform);

            SetObjectVelocity (powerupGO.GetComponent<Rigidbody2D> ());
        }
        else if (randomPowerup >= 1)
        {
            //We spawn Shield power up
            GameObject powerupGO = Instantiate (shieldPowerUp, parentTransform);

            SetObjectVelocity (powerupGO.GetComponent<Rigidbody2D> ());
        }
    }

    private void SetObjectVelocity (Rigidbody2D rigidbody2D)
    {
        //Set Direction
        Vector2 direction = new Vector2 (Random.value, Random.value).normalized;

        //Set speed of PowerUp
        float startSpeed = Random.Range (0.1f, 0.75f);

        //Set force on rigidbody
        rigidbody2D.AddForce (direction * startSpeed, ForceMode2D.Impulse);
    }

    #endregion
}
