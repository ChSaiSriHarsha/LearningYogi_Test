using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_SpaceshipController : MonoBehaviour
{
    [Header ("References")]
    [SerializeField] private Player_SpaceshipScriptableObject spaceshipScriptableObject;

    [Header ("Physics Related")]
    [SerializeField] private Rigidbody2D spaceshipRigidBody2D;
    [SerializeField] private Rigidbody2D powerBulletPrefabRigidBody2D;
    [SerializeField] private LayerMask whatIsAstroid;

    [Header ("Transforms")]
    [SerializeField] private Transform [] bulletSpawnTransforms;

    [Header ("Images")]
    [SerializeField] private List<Image> healthImages;

    [Header ("GameObjects")]
    [SerializeField] private GameObject shieldObject;

    internal bool isBulletPoweredUp = false;
    internal bool isShieldPoweredUp = false;
    internal bool gameStarted = false;

    private bool isAccelerating = false;
    private bool isDead = false;

    private int currentHealth;

    private float timer = 0f;

    public static Player_SpaceshipController SharedInstance;

    private void Awake ()
    {
        SharedInstance = this;
    }

    private void Start ()
    {
        currentHealth = spaceshipScriptableObject.so_MaxHealth;

        isDead = false;
        isBulletPoweredUp = false;
        isShieldPoweredUp = false;
        gameStarted = false;

        shieldObject.SetActive (false);
    }

    #region UPDATE HANDLING

    private void Update ()
    {
        if (!isDead && gameStarted)
        {
            GetPlayerInputs ();
            GetShooting ();

            if (isBulletPoweredUp)
                SetBulletPowerUp ();

            if (isShieldPoweredUp)
                SetPlayerShield ();
        }

        if (!gameStarted && Input.GetKeyDown (KeyCode.S))
            gameStarted = true;
    }

    private void FixedUpdate ()
    {
        //We handle physics related code in this
        if (!isDead && isAccelerating)
        {
            //Add force to spaceship for accelerating
            spaceshipRigidBody2D.AddForce (spaceshipScriptableObject.so_Acceleration * transform.up);
            spaceshipRigidBody2D.velocity =
                Vector2.ClampMagnitude (spaceshipRigidBody2D.velocity, spaceshipScriptableObject.so_MaximumVelocity);
        }
    }

    #endregion

    #region PLAYER MOVEMENT INPUTS

    private void GetPlayerInputs ()
    {
        //Acceleration Code
        isAccelerating = Input.GetKey (KeyCode.W) || Input.GetKey (KeyCode.UpArrow);

        //Rotation Code
        if (Input.GetKey (KeyCode.A) || Input.GetKey (KeyCode.LeftArrow))
        {
            //Rotate left side
            transform.Rotate (spaceshipScriptableObject.so_RotationSpeed * Time.deltaTime * transform.forward);
        }
        else if (Input.GetKey (KeyCode.D) || Input.GetKey (KeyCode.RightArrow))
        {
            //Rotate right side
            transform.Rotate (-spaceshipScriptableObject.so_RotationSpeed * Time.deltaTime * transform.forward);
        }
    }

    #endregion

    #region PLAYER SHOOTING LOGIC

    private void GetShooting ()
    {
        if (Input.GetKeyDown (KeyCode.Space) && !isBulletPoweredUp)
        {
            //Get Pooled Bullet to spawn here
            Vector3 [] offsetPositions = new Vector3 []
            {
                //Set Spawn Bullet Positions
                new Vector3(bulletSpawnTransforms[0].position.x, bulletSpawnTransforms[0].position.y, 0f),
                new Vector3(bulletSpawnTransforms[1].position.x, bulletSpawnTransforms[1].position.y, 0f),
                new Vector3(bulletSpawnTransforms[2].position.x, bulletSpawnTransforms[2].position.y, 0f)
            };

            for (int i = 0; i < offsetPositions.Length; i++)
            {
                GameObject bulletGO = Weapon_BulletPooling.SharedInstance.GetPooledBulletObjects ();
                if (bulletGO != null)
                {
                    bulletGO.transform.position = offsetPositions [i];
                    bulletGO.transform.rotation = Quaternion.identity;

                    bulletGO.SetActive (true);
                }

                //Shoot Bullets
                Rigidbody2D bulletRigidbody2D = bulletGO.GetComponent<Rigidbody2D> ();

                BulletRigidBodyParams (bulletRigidbody2D, bulletGO);
            }
        }
    }

    private void SetBulletPowerUp ()
    {
        //We Hold this for 10 seconds and shoot
        timer += Time.deltaTime;

        if (timer < 10f && Input.GetKeyDown (KeyCode.Space))
        {
            //We Start Spawning Bullets
            Rigidbody2D poweredBulletRB =
            Instantiate (powerBulletPrefabRigidBody2D, bulletSpawnTransforms [0].position, Quaternion.identity);

            BulletRigidBodyParams (poweredBulletRB, poweredBulletRB.gameObject);
        }
        else if (timer >= 10f)
        {
            //We set flag to false
            timer = 0f;

            isBulletPoweredUp = false;
        }
    }

    private void BulletRigidBodyParams (Rigidbody2D rigidbody2D, GameObject gameObject)
    {
        //Shoot in forward direction
        Vector2 spaceshipVelocity = spaceshipRigidBody2D.velocity;
        Vector2 spaceshipDirection = gameObject.transform.forward;

        float spaceshipSpeed = Vector2.Dot (spaceshipVelocity, spaceshipDirection);

        //Check if bullet is spawning in opposite direction
        if (spaceshipSpeed < 0)
            spaceshipSpeed = 0;

        rigidbody2D.velocity = spaceshipDirection * spaceshipSpeed;

        //Now we add force to move bullet
        rigidbody2D.AddForce
            (spaceshipScriptableObject.so_BulletSpeed * transform.up, ForceMode2D.Impulse);
    }

    #endregion

    #region PLAYER HEALTH & COLLISION LOGIC

    private void OnCollisionEnter2D (Collision2D collision)
    {
        if ((whatIsAstroid.value & (1 << collision.gameObject.layer)) > 0 && !isShieldPoweredUp)
        {
            //We colliding with astroid, so we have reduce the health of player
            currentHealth--;

            if (healthImages.Count >= 1 && !isDead)
            {
                healthImages [0].DOFade (0f, 0.25f).SetEase (Ease.Linear).OnComplete (() =>
                healthImages.Remove (healthImages [0]));
            }

            //We reduce images on top as well

            if (currentHealth <= 0)
            {
                isDead = true;

                //Call Game Over Method
                GameManager.SharedInstance.GameOver ();

                //Destroy the player
                Destroy (gameObject);
            }
        }
    }

    private void SetPlayerShield ()
    {
        //We Hold this for 10 seconds and have shield for player
        timer += Time.deltaTime;

        if (timer < 10f)
        {
            //We activate shield
            if (!shieldObject.activeInHierarchy)
                shieldObject.SetActive (true);
        }
        else if (timer > 10f)
        {
            //We set flag to false and deactivate shield
            timer = 0f;

            isShieldPoweredUp = false;
            shieldObject.SetActive (false);
        }
    }

    #endregion
}