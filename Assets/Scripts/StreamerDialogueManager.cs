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
    public GameObject GameWonTimeline;
    public GameObject GameLoseTimeline;


    private TextMeshProUGUI _textMessaage;
    private Sprite _image;


    [SerializeField] private List<StreamerTalkTopicSO> _streamerTalkTopicList = new List<StreamerTalkTopicSO>();
    // Start is called before the first frame update
    void Start()
    {
        _textMessaage = DialogueTextBox.GetComponent<TextMeshProUGUI>();
        _image = Streamer.GetComponent<UnityEngine.UI.Image>().overrideSprite;
        ChooseTimeline();


        StartCoroutine("ShowDialogue");

    }

    // Update is called once per frame
    void Update()
    {
        //if fighting stream ends, stop showDialogue to start another couritine that shows that streamer's reaction
    }

    //Take a random amount of seconds for streamer to talk
    //Take a random SO from the list and get the dialogue
    //Activate the dialogue box that is off and put the SO inside of it
    //wait for certain amount of seconds
    //set the dialogue active false again and reset the while loop
    public IEnumerator ShowDialogue()
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

    //For the timeline emitter when you win
    public void GameWon()
    {
        StartCoroutine("ReactionDialogueWin");
        
        
    }
    public IEnumerator ReactionDialogueWin()
    {

        StopCoroutine("ShowDialogue");
        Streamer.GetComponent<PlayableDirector>().enabled = true;
        //Use Scriptable Object here
        _textMessaage.text = "OH SHELL YEAH! WE WON BUDDIES";

            
        DialogueBox.SetActive(true);

        yield return new WaitForSeconds(5);
        GameWonTimeline.SetActive(false);
        GameWonTimeline.GetComponent<PlayableDirector>().initialTime = 0;
        Streamer.GetComponent<PlayableDirector>().initialTime = 0;
        Streamer.GetComponent<PlayableDirector>().enabled = false;
        DialogueBox.SetActive(false);

        ChooseTimeline();

        StartCoroutine("ShowDialogue");
        
    }

    //For the timeline emitter when you lose
    public void GameLost()
    {
        StartCoroutine("ReactionDialogueLose");
        
        
    }
    public IEnumerator ReactionDialogueLose()
    {
        StopCoroutine("ShowDialogue");
        Streamer.GetComponent<PlayableDirector>().enabled = true;
        //Use Scriptable Object here
        _textMessaage.text = "Oh shucks! looks like we lost buddies";


        DialogueBox.SetActive(true);

        yield return new WaitForSeconds(5);
        GameLoseTimeline.SetActive(false);
        GameLoseTimeline.GetComponent<PlayableDirector>().initialTime = 0;
        Streamer.GetComponent<PlayableDirector>().initialTime = 0;
        Streamer.GetComponent<PlayableDirector>().enabled = false;
        DialogueBox.SetActive(false);
        ChooseTimeline();
        StartCoroutine("ShowDialogue");
    }

    
    

    public void ChooseTimeline()
    {
        int rand = Random.Range(0,2);
        if (rand == 0)
        {
            GameLoseTimeline.SetActive(true);
        }
        if (rand == 1)
        {
            GameWonTimeline.SetActive(true);
        }
    }

}
