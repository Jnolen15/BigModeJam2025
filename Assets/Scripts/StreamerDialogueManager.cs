using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class StreamerDialogueManager : MonoBehaviour
{
    [Header("References")]
    public GameObject Streamer;
    public GameObject DialogueBox;
    public GameObject DialogueTextBox;


    private TextMeshProUGUI _textMessaage;
    private Sprite _image;


    [SerializeField] private List<StreamerTalkTopicSO> _streamerTalkTopicList = new List<StreamerTalkTopicSO>();
    // Start is called before the first frame update
    void Start()
    {
        _textMessaage = DialogueTextBox.GetComponent<TextMeshProUGUI>();
        _image = Streamer.GetComponent<UnityEngine.UI.Image>().overrideSprite;
        StartCoroutine("showDialogue");

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Take a random amount of seconds for streamer to talk
    //Take a random SO from the list and get the dialogue
    //Activate the dialogue box that is off and put the SO inside of it
    //wait for certain amount of seconds
    //set the dialogue active false again and reset the while loop
    IEnumerator showDialogue()
    {
        
        while (true)
        {
            Streamer.GetComponent<UnityEngine.UI.Image>().sprite = _image;
            int _randNumber = Random.Range(3, 10);
            int randTopic = Random.Range(0, _streamerTalkTopicList.Count);
            yield return new WaitForSeconds(_randNumber);
            //Use Scriptable Object here
            _textMessaage.text = _streamerTalkTopicList[randTopic].StreamerDialogue;
            
            Streamer.GetComponent<PlayableDirector>().enabled = true;
            DialogueBox.SetActive(true);

            yield return new WaitForSeconds(5);
            Streamer.GetComponent<PlayableDirector>().initialTime = 0;
            Streamer.GetComponent<PlayableDirector>().enabled = false;
            DialogueBox.SetActive(false);
            
            
            yield return null;
        }
    }
}
