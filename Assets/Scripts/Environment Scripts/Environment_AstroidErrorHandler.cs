using UnityEngine;

public class Environment_AstroidErrorHandler : MonoBehaviour
{
    [SerializeField] private CircleCollider2D circleCollider2D;

    private void Start ()
    {
        InvokeRepeating ("CheckError", 0.5f, 0.25f);
    }

    private void CheckError ()
    {
        if (!circleCollider2D.enabled)
        {
            Destroy (gameObject);
        }
    }
}
