using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour {

    public DoorsController doorScript;

    private GameObject buttonUnpressed;
    private GameObject buttonPressed;

    // FMOD
    [FMODUnity.EventRef]
    public string buttonPress;
    private FMOD.Studio.EventInstance instancePress;
    [FMODUnity.EventRef]
    public string buttonUnpress;
    private FMOD.Studio.EventInstance instanceUnpress;

    private void Start() {
        buttonUnpressed = transform.GetChild(0).gameObject;
        buttonUnpressed.SetActive(true);
        buttonPressed = transform.GetChild(1).gameObject;
        buttonPressed.SetActive(false);

        instancePress = FMODUnity.RuntimeManager.CreateInstance(buttonPress);
        instanceUnpress = FMODUnity.RuntimeManager.CreateInstance(buttonUnpress);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag != "Hint" && collision.tag != "Bait" && collision.tag != "Guardian") {
            buttonUnpressed.SetActive(false);
            buttonPressed.SetActive(true);
            doorScript.HandleDoors(true);
            FMODUnity.RuntimeManager.PlayOneShotAttached(buttonPress, this.gameObject);
            instancePress.start();
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        buttonUnpressed.SetActive(true);
        buttonPressed.SetActive(false);
        doorScript.HandleDoors(false);
        FMODUnity.RuntimeManager.PlayOneShotAttached(buttonUnpress, this.gameObject);
        instanceUnpress.start();
    }
}
