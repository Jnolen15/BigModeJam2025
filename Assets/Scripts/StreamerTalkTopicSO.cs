using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Talk Topic")]
public class StreamerTalkTopicSO : ScriptableObject
{


    [TextArea]
    public string streamerDialogue;

    public enum Topics
    {
        Winnig,
        Losing,
        Hungry,
        RandomThought,
        MakeJoke,

    }

    public List<Topics> topics;
}
