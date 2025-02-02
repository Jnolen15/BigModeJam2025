using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Username")]
public class UsernamesSO : ScriptableObject
{
    public Color ChatterColor;
    public int BuddyLevel;

    public string GetUsername()
    {
        return name;
    }

    public void Reset()
    {
        RandomizeColor();
        RandomizeBuddyBadge();
    }

    private void RandomizeColor()
    {
        ChatterColor = Random.ColorHSV();
    }

    private void RandomizeBuddyBadge()
    {
        int rand = Random.Range(0, 100);

        if (rand < 70)
            BuddyLevel = 1;
        else if (rand >= 70 || rand <= 90)
            BuddyLevel = 2;
        else if (rand < 91)
            BuddyLevel = 3;
    }
}
