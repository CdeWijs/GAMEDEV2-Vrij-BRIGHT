using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// destroy cage when colliding with ground
/// </summary>
public class CageDestroy : MonoBehaviour {

    // FMOD
    [FMODUnity.EventRef]
    public string eventRef;
    private FMOD.Studio.EventInstance instance;

    private void Start() {
        instance = FMODUnity.RuntimeManager.CreateInstance(eventRef);
    }

    public GuardianController guardianController;
    public GameObject brokenCage;
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.transform.tag == "Ground") {
            guardianController.captured = false;
            brokenCage.SetActive(true);
            this.gameObject.SetActive(false);
            FMODUnity.RuntimeManager.PlayOneShotAttached(eventRef, this.gameObject);
            instance.start();
        }
        }
    }
