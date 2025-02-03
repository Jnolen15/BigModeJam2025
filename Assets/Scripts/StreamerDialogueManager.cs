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
    [SerializeField]
    public AudioClip[] StreamerAudio;


    private TextMeshProUGUI _textMessaage;
    private bool _startDialogue = false;
    private bool _endDialogue = false;
    private Sprite _image;


    [SerializeField] private List<StreamerTalkTopicSO> _streamerTalkTopicList = new List<StreamerTalkTopicSO>();
    // Start is called before the first frame update
    void Start()
    {

        GameManager.OnGameStarted += StartDialogue;
        GameManager.OnGameEnded += EndDialogue;
        _textMessaage = DialogueTextBox.GetComponent<TextMeshProUGUI>();
        _image = Streamer.GetComponent<UnityEngine.UI.Image>().overrideSprite;
        ChooseTimeline();


        StartCoroutine("ShowDialogue");

    }

    public void OnDestroy()
    {
        GameManager.OnGameStarted -= StartDialogue;
        GameManager.OnGameEnded -= EndDialogue;
    }

    public void StartDialogue()
    {
        _startDialogue = true;
    }
    public void EndDialogue()
    {
        _endDialogue = true;
        GameWonTimeline.SetActive(false);
        GameLoseTimeline.SetActive(false);
        gameObject.GetComponent<AudioSource>().Stop();
}

    //Take a random amount of seconds for streamer to talk
    //Take a random SO from the list and get the dialogue
    //Activate the dialogue box that is off and put the SO inside of it
    //wait for certain amount of seconds
    //set the dialogue active false again and reset the while loop
    public IEnumerator ShowDialogue()
    {
        
        while (!_endDialogue)
        {
            if (_startDialogue)
            {
                Streamer.GetComponent<UnityEngine.UI.Image>().sprite = _image;
                int _randNumber = Random.Range(3, 10);
                int randTopic = Random.Range(0, _streamerTalkTopicList.Count);
                yield return new WaitForSeconds(_randNumber);
                //Use Scriptable Object here
                _textMessaage.text = _streamerTalkTopicList[randTopic].StreamerDialogue;

                gameObject.GetComponent<AudioSource>().PlayOneShot(StreamerAudio[Random.Range(0, StreamerAudio.Length)]);
                Streamer.GetComponent<PlayableDirector>().enabled = true;
                DialogueBox.SetActive(true);

                yield return new WaitForSeconds(6);
                Streamer.GetComponent<PlayableDirector>().initialTime = 0;
                Streamer.GetComponent<PlayableDirector>().enabled = false;
                DialogueBox.SetActive(false);

            }
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
        if (!_endDialogue)
        {
            StopCoroutine("ShowDialogue");
            gameObject.GetComponent<AudioSource>().Stop();
            Streamer.GetComponent<PlayableDirector>().enabled = true;
            //Use Scriptable Object here
            _textMessaage.text = "OH SHELL YEAH! WE WON BUDDIES";
            gameObject.GetComponent<AudioSource>().PlayOneShot(StreamerAudio[1]);


            DialogueBox.SetActive(true);

            yield return new WaitForSeconds(6);
            GameWonTimeline.SetActive(false);
            GameWonTimeline.GetComponent<PlayableDirector>().initialTime = 0;
            Streamer.GetComponent<PlayableDirector>().initialTime = 0;
            Streamer.GetComponent<PlayableDirector>().enabled = false;
            DialogueBox.SetActive(false);

            ChooseTimeline();

            StartCoroutine("ShowDialogue");
        }
    }

    //For the timeline emitter when you lose
    public void GameLost()
    {
        StartCoroutine("ReactionDialogueLose");
        
        
    }
    public IEnumerator ReactionDialogueLose()
    {
        if (!_endDialogue)
        {
            StopCoroutine("ShowDialogue");
            gameObject.GetComponent<AudioSource>().Stop();
            Streamer.GetComponent<PlayableDirector>().enabled = true;
            //Use Scriptable Object here
            _textMessaage.text = "Oh shucks! looks like we lost buddies";
            gameObject.GetComponent<AudioSource>().PlayOneShot(StreamerAudio[2]);

            DialogueBox.SetActive(true);

            yield return new WaitForSeconds(6);
            GameLoseTimeline.SetActive(false);
            GameLoseTimeline.GetComponent<PlayableDirector>().initialTime = 0;
            Streamer.GetComponent<PlayableDirector>().initialTime = 0;
            Streamer.GetComponent<PlayableDirector>().enabled = false;
            DialogueBox.SetActive(false);
            ChooseTimeline();
            StartCoroutine("ShowDialogue");
        }
    }

    IEnumerator StartStream()
    {
        while (!_startDialogue)
        {
            yield return null;
        }
        ChooseTimeline();
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
