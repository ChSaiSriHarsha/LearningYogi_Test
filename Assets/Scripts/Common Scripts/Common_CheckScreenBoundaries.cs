using UnityEngine;

public class Common_CheckScreenBoundaries : MonoBehaviour
{
    private void Update ()
    {
        SetPlayerAdjustment ();
    }

    private void SetPlayerAdjustment ()
    {
        //We convert world point to view port first
        Vector3 viewPortPosition = Camera.main.WorldToViewportPoint (transform.position);

        //If ship goes out of screen, we re position that in opposite side
        Vector3 spaceshipPosition = Vector3.zero;

        if (viewPortPosition.x < 0)
            spaceshipPosition.x += 1;
        else if (viewPortPosition.x > 1)
            spaceshipPosition.x -= 1;
        else if (viewPortPosition.y < 0)
            spaceshipPosition.y += 1;
        else if (viewPortPosition.y > 1)
            spaceshipPosition.y -= 1;

        //We convert to world co-ordinates now and assign to player
        transform.position = Camera.main.ViewportToWorldPoint (viewPortPosition + spaceshipPosition);
    }
}
