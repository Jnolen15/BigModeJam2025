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
        NoProfanity,
        NoLinks,
        NoPromoting,
        NoAllCaps,
        NoMean,
        NoMessagesOver30,
    }

    public List<Violations> violations;

    public List<Violations> GetViolationList()
    {
        return violations;
    }
}


