using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Comment")]
public class Comments : ScriptableObject
{
    [TextArea]
    public string message;

    public enum Violations
    {
        NoSpamming,
        NoProfanity,
        NoLinks,
        NoPromoting,
        NoAllCaps,

    }

    public List<Violations> violations;
}


