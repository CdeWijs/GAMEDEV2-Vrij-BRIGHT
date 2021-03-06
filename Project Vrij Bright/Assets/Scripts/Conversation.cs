﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// class for displaying text on screen
/// coroutine sets [conversation UI text element] to textarray[counter] and takes in text from hints 
/// </summary>
public class Conversation : MonoBehaviour
{
    public static Conversation _Instance;
    public Text text;
    public string[] lines;
    public float conversationTime;
    public int counter = 0;

    public string currentString;
    public bool playAudio;
    public bool playing = false;
    
    [FMODUnity.EventRef]
    public string[] audioEvents;

    #region singleton
    private void Awake()
    {
        if (_Instance == null)
        {
            _Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    #endregion

    private void Start()
    {
        //play start text
        playing = true;
        DisplayHint(lines);
    }

    //set playing to true to prevent multiple plays and then display hint
    public void DisplayHint(string[] _hints)
    {
        counter = 0;
        playing = true;
        StartCoroutine(UpdateConversation(_hints));
    }

    //keeps calling itself till all text has been displayed
    private IEnumerator UpdateConversation(string[] _lines)
    {
        text.text = _lines[counter];
        Debug.Log(_lines[counter]);
        if (playAudio)
        {
            FMODUnity.RuntimeManager.PlayOneShotAttached(audioEvents[counter], this.gameObject);
        }
        counter++;

        yield return new WaitForSeconds(conversationTime);

        if (counter < _lines.Length)
        {
            StartCoroutine(UpdateConversation(_lines));
        }
        else
        {
            playing = false;
            text.text = "";
            playAudio = false;
        }
    }

    public IEnumerator ThankYou()
    {
        playing = true;
        text.text = "Thank you";
        Debug.Log("Thank You");
        yield return new WaitForSeconds(conversationTime);
        playing = false;
        text.text = "";
    }
}