using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoyController : BaseController
{
    public override void Start()
    {
        base.Start();

        connectedController = new Joystick1();
    }
}
