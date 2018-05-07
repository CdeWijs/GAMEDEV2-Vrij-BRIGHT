using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoyController : BaseController
{
    public override void Start()
    {
        base.Start();

        // Check if Joystick exists
        if (Input.GetJoystickNames().Length > 0)
        {
            connectedController = new Joystick1();
        }
    }
}
