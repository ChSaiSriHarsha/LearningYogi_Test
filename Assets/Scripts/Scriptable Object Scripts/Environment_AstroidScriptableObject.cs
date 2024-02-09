using UnityEngine;

[CreateAssetMenu(fileName = "Astroid_Attributes", menuName = "Enviroment_SO/Astroid_Attributes")]
public class Environment_AstroidScriptableObject : ScriptableObject
{
    [Header ("Astroid Spawn")]
    public int noOfAstroids = 0;
}
