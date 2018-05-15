using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardianController : BaseController {

    public const float NormalSpeed = 8;
    public const float JumpForce = 4;

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
