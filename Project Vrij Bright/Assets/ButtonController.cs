using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour {

    public DoorsController doorScript;

    private GameObject buttonUnpressed;
    private GameObject buttonPressed;

    private void Start() {
        buttonUnpressed = transform.GetChild(0).gameObject;
        buttonUnpressed.SetActive(true);
        buttonPressed = transform.GetChild(1).gameObject;
        buttonPressed.SetActive(false);
    }
    
    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.tag != "Hint" && collision.tag != "Bait" && collision.tag != "Guardian") {
            buttonUnpressed.SetActive(false);
            buttonPressed.SetActive(true);
            doorScript.HandleDoors(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        buttonUnpressed.SetActive(true);
        buttonPressed.SetActive(false);
        doorScript.HandleDoors(false);
    }
}
