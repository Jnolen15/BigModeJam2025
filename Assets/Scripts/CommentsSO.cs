using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Comment")]
public class CommentsSO : ScriptableObject
{
    [TextArea(15, 20)]
    public string Message;

    public enum Violations
    {
        None,
        NoProfanity,
        NoLinks,
        NoPromoting,
        NoAllCaps,
        NoMean,
        NoMessagesOver30,
    }

    public Violations violation;
}


