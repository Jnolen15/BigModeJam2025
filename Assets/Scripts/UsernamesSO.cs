using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Username")]
public class UsernamesSO : ScriptableObject
{
    public Color ChatterColor;

    public string GetUsername()
    {
        return name;
    }

    public void Reset()
    {
        RandomizeColor();
    }

    private void RandomizeColor()
    {
        ChatterColor = Random.ColorHSV();
    }
}
