using UnityEngine;

public class Weapon_BulletController : MonoBehaviour
{
    [Header ("References")]
    [SerializeField] private Player_SpaceshipScriptableObject spaceshipScriptableObject;

    private void Start ()
    {
        InvokeRepeating ("DeactivateBullet", spaceshipScriptableObject.so_BulletDeactivateTime, 2f);
    }

    private void DeactivateBullet ()
    {
        gameObject.SetActive (false);
    }
}
