using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardianController : BaseController {

    public const float NormalSpeed = 8;
    public const float JumpForce = 5;

    private int flutterAmount = 5;

    public override void Start() {
        base.Start();

        // Check if Joystick exists
        if (Input.GetJoystickNames().Length > 0) {
            connectedController = new Joystick2();
        }
    }

    public override void Update() {
        base.Update();

        if (flutterAmount < 5 && grounded) {
            flutterAmount = 5;
        }
    }

    public override void FixedUpdate() {
        base.FixedUpdate();

        MoveHorizontally(NormalSpeed);
    }

    //override collision check so players can jump on eachothers head
    new public void OnCollisionEnter2D(Collision2D collision){
        if (collision.transform.tag == "Ground" || collision.transform.tag == "Player"){
            grounded = true;
        }
    }


    //override collision check so players can jump on eachothers head
    new public void OnCollisionExit2D(Collision2D collision){
        if (collision.transform.tag == "Ground" || collision.transform.tag == "Player"){
            grounded = false;
        }
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
