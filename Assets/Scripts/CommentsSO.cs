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
        Profanity,
        Links,
        Promoting,
        AllCaps,
        Mean,
        MessagesOver30,
    }

    public Violations violation;
}


