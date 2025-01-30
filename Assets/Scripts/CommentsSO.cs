using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Comment")]
public class CommentsSO : ScriptableObject
{
    [TextArea]
    public string Message;

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


