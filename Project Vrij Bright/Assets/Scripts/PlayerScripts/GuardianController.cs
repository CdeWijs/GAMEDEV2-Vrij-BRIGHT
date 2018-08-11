using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardianController : BaseController {

    public const float NormalSpeed = 4;
    public const float JumpForce = 5;

    private int flutterAmount = 5;

    public GameObject lightSource;
    private float LightPower = 100f;
    public Light lt;
    public float decrease;

    public bool captured;

    // FMOD
    [FMODUnity.EventRef]
    public string doorOpen;
    private FMOD.Studio.EventInstance instanceOpen;
    [FMODUnity.EventRef]
    public string doorClose;
    private FMOD.Studio.EventInstance instanceClose;
    
    public override void Start() {
        base.Start();

        // Check if Joystick exists
        if (Input.GetJoystickNames().Length > 0) {
            connectedController = new Joystick2();
        }

        instanceOpen = FMODUnity.RuntimeManager.CreateInstance(doorOpen);
        instanceClose = FMODUnity.RuntimeManager.CreateInstance(doorClose);
    }

    public override void Update() {
        base.Update();

        if (flutterAmount < 5 && grounded) {
            flutterAmount = 5;
        }

        //keeps guardian trapped in cage while imprisoned 
        if (captured) {
            this.gameObject.layer = 20;
        } else { this.gameObject.layer = 9; }
    }

    public override void FixedUpdate() {
        base.FixedUpdate();

        MoveHorizontally(NormalSpeed);
    }

    //override collision check so players can jump on eachothers head
    new public void OnCollisionEnter2D(Collision2D collision) {
        if (collision.transform.tag == "Ground" || collision.transform.tag == "Player") {
            grounded = true;
        }
    }


    //override collision check so players can jump on eachothers head
    new public void OnCollisionExit2D(Collision2D collision) {
        if (collision.transform.tag == "Ground" || collision.transform.tag == "Player") {
            grounded = false;
        }
    }

    public void OnTriggerStay2D(Collider2D col) {
        if (col.tag == "Hint") {
            lightSource.SetActive(true);
            if (x_active) {
                if (!Conversation._Instance.playing) {
                    col.gameObject.GetComponent<Hint>().SetHintActive();
                }
            }
        }

        if (col.tag == "Bait") {
            Interaction interaction = col.GetComponent<Interaction>();
            interaction.SetButtonActive(true);
            if (connectedController != null && x_active) {
                interaction.Teleport(col.gameObject);
            }
            else if (Input.GetKeyDown(KeyCode.E)) {
                interaction.Teleport(col.gameObject);
            }
        }
    }


    public void OnTriggerExit2D(Collider2D col) {
        if (col.tag == "Hint") {
            lightSource.SetActive(false);
            if (col.gameObject.GetComponent<Hint>() != null) {
                col.gameObject.GetComponent<Hint>().SetHintActive();
            }
        }

        if (col.tag == "Bait") {
            Interaction interaction = col.GetComponent<Interaction>();
            interaction.SetButtonActive(false);
        }
    }

    public void UseLight() {
        lt.intensity -= decrease;
    }

    public override void GetInput() {
        base.GetInput();

        if (connectedController != null) {
            if (a_active && flutterAmount > 0) {
                Jump(JumpForce);
                flutterAmount--;
            }
        } else {
            if (Input.GetKeyDown(KeyCode.Space) && flutterAmount > 0) {
                Jump(JumpForce);
                flutterAmount--;
            }
        }
    }
}
