using UnityEngine;

public class Environment_AstroidController : MonoBehaviour
{
    public GameManager gameManager;

    [Header ("References")]
    [Range (0, 5)]
    [SerializeField] private int noOfSplits;
    [SerializeField] private Rigidbody2D astroidRigidBody2D;

    [Header ("Physics Related")]
    [SerializeField] private LayerMask whatIsBullet;

    private bool isCollided = false;

    #region INITIALIZATION

    private void Start ()
    {
        //We set scale based on size of astroid
        transform.localScale = 0.5f * noOfSplits * Vector3.one;

        //Set astroid direction in random
        Vector2 direction = new Vector2 (Random.value, Random.value).normalized;

        //Set speed of astroid
        float startSpeed = Random.Range (4f - noOfSplits, 5f - noOfSplits);

        //Set force on rigidbody
        astroidRigidBody2D.AddForce (direction * startSpeed, ForceMode2D.Impulse);

        gameManager.astroidCount++;

        isCollided = false;
    }

    #endregion

    #region COLLISION AND SPLIT LOGIC

    private void OnCollisionEnter2D (Collision2D collision)
    {
        if ((whatIsBullet.value & (1 << collision.gameObject.layer)) > 0 && !isCollided)
        {
            isCollided = true;

            //If astroid collides with bullet, we split the astroids
            gameManager.astroidCount--;

            //We also deactivate the bullet that hit astroid
            collision.gameObject.SetActive (false);

            //We now call split method
            SplitAstroid ();
        }
    }

    private void SplitAstroid ()
    {
        //We now split the astroid
        if (noOfSplits > 1)
        {
            for (int i = 0; i < 2; i++)
            {
                Environment_AstroidController newAstroid = Instantiate (this, transform.position, Quaternion.identity);

                newAstroid.noOfSplits -= 1;

                newAstroid.gameManager = gameManager;
            }
        }

        //Now we destroy this version of astroid
        Destroy (gameObject);
    }

    #endregion

    #region ON DESTROY HANDLER

    private void OnDestroy ()
    {
        if (noOfSplits <= 1)
            gameManager.SetPlayerScore ();
    }

    #endregion
}
