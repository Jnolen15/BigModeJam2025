using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Username")]
public class UsernamesSO : ScriptableObject
{
    public string GetUsername()
    {
        return name;
    }
}
