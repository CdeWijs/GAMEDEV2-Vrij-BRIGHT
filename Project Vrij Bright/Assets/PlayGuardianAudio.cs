using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayGuardianAudio : MonoBehaviour {

    public Conversation conversation;

    // FMOD
    [FMODUnity.EventRef]
    public string eventRef;
    
    void Start () {
        StartCoroutine(PlayAudio());
	}

    private IEnumerator PlayAudio()
    {
        yield return new WaitForSeconds(2);
        FMODUnity.RuntimeManager.PlayOneShotAttached(eventRef, this.gameObject);
        StartCoroutine(conversation.ThankYou());
    }
}
