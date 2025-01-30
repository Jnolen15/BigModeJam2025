using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class StreamerDialogueManager : MonoBehaviour
{
    [Header("References")]
    public GameObject DialogueBox;
    public GameObject DialogueTextBox;


    private TextMeshProUGUI _textMessaage;


    [SerializeField] private List<StreamerTalkTopicSO> _streamerTalkTopicList = new List<StreamerTalkTopicSO>();
    // Start is called before the first frame update
    void Start()
    {
        _textMessaage = DialogueTextBox.GetComponent<TextMeshProUGUI>();
        StartCoroutine("showDialogue");

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator showDialogue()
    {
        
        while (true)
        {
            int _randNumber = Random.Range(3, 10);
            int randTopic = Random.Range(0, _streamerTalkTopicList.Count);
            yield return new WaitForSeconds(_randNumber);
            //Use Scriptable Object here
            _textMessaage.text = _streamerTalkTopicList[randTopic].StreamerDialogue;
            DialogueBox.SetActive(true);

            yield return new WaitForSeconds(5);

            DialogueBox.SetActive(false);
            yield return null;
        }
    }
}
