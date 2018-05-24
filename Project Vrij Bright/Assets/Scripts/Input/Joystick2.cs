using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joystick2 : ControllerInput
{
    public override bool Trig_CheckInput()
    {
        //Debug.Log(Input.GetAxis("LeftTriggerJ2"));
        if (Input.GetAxis("LeftTriggerJ2") >= .85f)
        {
            return true;
        }
        else
            return false;
    }
    public override bool A_CheckInput()
    {
        if (Input.GetButtonDown("A_ButtonJ2") == true)
        {
            return true;
        }
        else
            return false;
    }
    public override bool B_CheckInput()
    {
        if (Input.GetButtonDown("B_ButtonJ2") == true)
        {
            return true;
        }
        else
            return false;
    }
    public override bool X_CheckInput()
    {
        if (Input.GetButtonDown("X_ButtonJ2") == true)
        {
            return true;
        }
        else
            return false;
    }
    public override bool Y_CheckInput()
    {
        if (Input.GetButtonDown("Y_ButtonJ2") == true)
        {
            return true;
        }
        else
            return false;
    }

    public override string GetControllerName()
    {
        return "Joystick 2 ";
    }
    public override string GetVertical()
    {
        return "VerticalJ2";
    }
    public override string GetHorizontal()
    {
        return "HorizontalJ2";
    }
}
