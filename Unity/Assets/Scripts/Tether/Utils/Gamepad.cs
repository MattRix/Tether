using System;
using UnityEngine;
using System.Collections.Generic;

public class Gamepad
{
    public int index;
    
    public Vector2 leftStick;
    public Vector2 rightStick;
    
	public string axisJoyName; 
	public string buttonJoyName; 
    
    public Gamepad()
    {
    }

    public void Update()
    {
        string indexString = (index+1).ToString();
       
		axisJoyName = "Joystick " + indexString;
		buttonJoyName = "joystick " + indexString;

		leftStick = Deadzonize(0.1f, Input.GetAxisRaw(axisJoyName + " Axis 1"), Input.GetAxisRaw(axisJoyName + " Axis 2"));
		rightStick = Deadzonize(0.1f, Input.GetAxisRaw(axisJoyName + " Axis 3"), Input.GetAxisRaw(axisJoyName + " Axis 4"));
        
//      for (int b = 0; b<PS3ButtonType.allButtons.Count; b++)
//		{
//        	if(GetButtonDown(PS3ButtonType.allButtons[b]))
//            {
//                Debug.Log("Joystick " +index + " Button " + PS3ButtonType.allButtons[b].name + " Pressed!");
//            }
//		}
    }
    
    static public Vector2 Deadzonize(float deadzone, float axisX, float axisY)
    {
        Vector2 stickVector = new Vector2(axisX,axisY);
        if(stickVector.magnitude < deadzone) stickVector = Vector2.zero;
        else stickVector = stickVector.normalized * ((stickVector.magnitude - deadzone) / (1 - deadzone));
       
        return stickVector;
    }
    
    public bool GetButton(PS3ButtonType buttonType)
    {
		return Input.GetKey(buttonJoyName + " button " + buttonType.numString);
    }
    
    public bool GetButtonDown(PS3ButtonType buttonType)
    {
		return Input.GetKeyDown(buttonJoyName + " button " + buttonType.numString);
    }
    
    public bool GetButtonUp(PS3ButtonType buttonType)
    {
		return Input.GetKeyUp(buttonJoyName + " button " + buttonType.numString);
    }
}
                            
public class PS3ButtonType
{
    public string name;
    public string numString;
    
    static public List<PS3ButtonType> allButtons = new List<PS3ButtonType>();
    
    public static PS3ButtonType Up = 		new PS3ButtonType("Up", "4");
    public static PS3ButtonType Right = 	new PS3ButtonType("Right", "5");
    public static PS3ButtonType Down = 		new PS3ButtonType("Down", "6");
    public static PS3ButtonType Left = 		new PS3ButtonType("Left", "7");
    
    public static PS3ButtonType Triangle = 	new PS3ButtonType("Triangle", "12");
    public static PS3ButtonType Circle = 	new PS3ButtonType("Circle", "13");
    public static PS3ButtonType X = 		new PS3ButtonType("X", "14");
    public static PS3ButtonType Square = 	new PS3ButtonType("Square", "15");
    
    public static PS3ButtonType L1 = 		new PS3ButtonType("L1", "10");
    public static PS3ButtonType L2 = 		new PS3ButtonType("L2", "8");
    public static PS3ButtonType L3 = 		new PS3ButtonType("L3", "1");
    
    public static PS3ButtonType R1 = 		new PS3ButtonType("R1", "11");
    public static PS3ButtonType R2 = 		new PS3ButtonType("R2", "9");
    public static PS3ButtonType R3 = 		new PS3ButtonType("R3", "2");
    
    public static PS3ButtonType Start = 	new PS3ButtonType("Start", "3");
    public static PS3ButtonType Select = 	new PS3ButtonType("Select", "0");
    
    public PS3ButtonType(string name, string numString)
    {
        this.name = name;
        this.numString = numString;
        
        allButtons.Add(this);
    }
}







                            
