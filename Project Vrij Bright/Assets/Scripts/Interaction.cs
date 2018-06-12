using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour {

    public GameObject buttonImage;
    public GameObject other;
    //public string _tag;

    // FMOD
    [FMODUnity.EventRef]
    public string eventRef;
    private FMOD.Studio.EventInstance instance;

    private void Start() {
        instance = FMODUnity.RuntimeManager.CreateInstance(eventRef);
    }

    public void SetButtonActive(bool _bool) {
        buttonImage.SetActive(_bool);

    }

    public void Teleport(GameObject _object) {
        FMODUnity.RuntimeManager.PlayOneShotAttached(eventRef, this.gameObject);
        instance.start();
        _object.transform.position = new Vector3(
             other.transform.position.x,
            _object.transform.position.y,
            _object.transform.position.z);
    }
}

