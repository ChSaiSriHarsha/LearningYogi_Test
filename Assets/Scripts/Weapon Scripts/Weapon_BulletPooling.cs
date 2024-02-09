using System.Collections.Generic;
using UnityEngine;

public class Weapon_BulletPooling : MonoBehaviour
{
    [Header ("GameObjects")]
    [SerializeField] private List<GameObject> pooledBulletObjects;
    [SerializeField] private GameObject bulletObjectToPool;

    [Header ("Pooling Attributes")]
    [SerializeField] private int noOfBulletsToPool;

    [Header ("Transforms")]
    [SerializeField] private Transform parentTransform;

    public static Weapon_BulletPooling SharedInstance;

    #region INITIALIZATION

    private void Awake ()
    {
        SharedInstance = this;
    }

    private void Start ()
    {
        //Pool Bullets
        pooledBulletObjects = new List<GameObject> ();

        GameObject bulletObject;

        for (int i = 0; i < noOfBulletsToPool; i++)
        {
            bulletObject = Instantiate (bulletObjectToPool, parentTransform);
            bulletObject.SetActive (false);
            pooledBulletObjects.Add (bulletObject);
        }
    }

    #endregion

    #region POOLING LOGIC

    public GameObject GetPooledBulletObjects ()
    {
        //Call pooled bullets from here
        for (int i = 0; i < noOfBulletsToPool; i++)
        {
            if (!pooledBulletObjects [i].activeInHierarchy)
                return pooledBulletObjects [i];
        }
        return null;
    }

    #endregion
}
