using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Talk Topic")]
public class StreamerTalkTopicSO : ScriptableObject
{


    [TextArea]
    public string StreamerDialogue;

    public enum Topic
    {
        Winnig,
        Losing,
        Hungry,
        RandomThought,
        MakeJoke,

    }

    public Topic Topics;
}
