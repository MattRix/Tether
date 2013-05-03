using System;
using UnityEngine;
using System.Collections.Generic;

public class Gamepad
{
    public int index;
    
    public Vector2 leftStick;
    public Vector2 rightStick;
    
    public Gamepad()
    {
    }

    public void Update()
    {
        string indexString = (index+1).ToString();
        string joyName = "Joystick " + indexString;
        
        leftStick = Deadzonize(0.25f, Input.GetAxisRaw(joyName + " X"), Input.GetAxisRaw(joyName + " Y"));
        rightStick = Deadzonize(0.25f, Input.GetAxisRaw(joyName + " Axis 3"), Input.GetAxisRaw(joyName + " Axis 4"));
    }
    
    static public Vector2 Deadzonize(float deadzone, float axisX, float axisY)
    {
        Vector2 stickVector = new Vector2(axisX,axisY);
        if(stickVector.magnitude < deadzone) stickVector = Vector2.zero;
        else stickVector = stickVector.normalized * ((stickVector.magnitude - deadzone) / (1 - deadzone));
        
        return stickVector;
    }
}
