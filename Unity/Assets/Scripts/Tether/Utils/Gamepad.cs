using System;
using UnityEngine;
using System.Collections.Generic;

public class Gamepad
{
	static private bool SHOULD_LOG_ALL_BUTTON_PRESSES = false;

    public int index;
    
    public Vector2 direction;
    
	public string axisJoyName; 
	public string buttonJoyName; 

	public bool shouldInvertX;
	public bool shouldInvertY;

	public string axisXName = "";
	public string axisYName = "";

	public string buttonReady;
	public string buttonStart;
	public string buttonReset;
    
    public Gamepad(string joystickName)
    {
		bool isWindows = 
				Application.platform == RuntimePlatform.WindowsEditor ||
				Application.platform == RuntimePlatform.WindowsPlayer ||
				Application.platform == RuntimePlatform.WindowsWebPlayer;

		if(joystickName.Contains("360") || joystickName.ToLower().Contains("xbox")) //Xbox 360 controller or Microsoft 360 Wireless Controller
		{
			axisXName = "Axis 1";
			axisYName = "Axis 2";
			shouldInvertX = false;
			shouldInvertY = true;

			if(isWindows)
			{
				buttonStart = XboxButtonType.Start.numStringWin;
				buttonReady = XboxButtonType.A.numStringWin;
				buttonReset = XboxButtonType.Back.numStringWin;
			}
			else
			{
				buttonStart = XboxButtonType.Start.numStringOSX;
				buttonReady = XboxButtonType.A.numStringOSX;
				buttonReset = XboxButtonType.Back.numStringOSX;
			}
		}
		else if(joystickName.ToLower().Contains("sony")) //PS3 Controller
		{
			axisXName = "Axis 1";
			axisYName = "Axis 2";
			shouldInvertX = false;
			shouldInvertY = true;

			buttonStart = PS3ButtonType.Start.numStringOSX;
			buttonReady = PS3ButtonType.X.numStringOSX;
			buttonReset = PS3ButtonType.Select.numStringOSX;
		}
		else if(joystickName.ToLower().Contains("mad catz")) //for my crappy 10 year old Mad Catz gamepad
		{
			axisXName = "Axis 1";
			axisYName = "Axis 2";
			shouldInvertX = false;
			shouldInvertY = true;

			buttonStart = "8";
			buttonReady = "0";
			buttonReset = "6";
		}
		else
		{
			axisXName = "Axis 1";
			axisYName = "Axis 2";
			shouldInvertX = false;
			shouldInvertY = true;

			buttonStart = "0";
			buttonReady = "1";
			buttonReset = "2";
		}
    }

    public void Update()
    {
        string indexString = (index+1).ToString();
       
		axisJoyName = "Joystick " + indexString;
		buttonJoyName = "joystick " + indexString;

		direction = Deadzonize(0.1f, Input.GetAxisRaw(axisJoyName + " " + axisXName), Input.GetAxisRaw(axisJoyName + " " + axisYName));
		//rightStick = Deadzonize(0.1f, Input.GetAxisRaw(axisJoyName + " Axis 3"), Input.GetAxisRaw(axisJoyName + " Axis 4"));
        
		if(shouldInvertX) direction.x *= -1.0f;
		if(shouldInvertY) direction.y *= -1.0f;

		if(SHOULD_LOG_ALL_BUTTON_PRESSES)
		{
			for (int b = 0; b<20; b++)
			{
				if(Input.GetKeyDown(buttonJoyName + " button " + b.ToString()))
				{
					Debug.Log("Joystick " +index + " Button " + b.ToString() + " Pressed!");
				}
			}
		}
    }
    
    static public Vector2 Deadzonize(float deadzone, float axisX, float axisY)
    {
        Vector2 stickVector = new Vector2(axisX,axisY);
        if(stickVector.magnitude < deadzone) stickVector = Vector2.zero;
        else stickVector = stickVector.normalized * ((stickVector.magnitude - deadzone) / (1 - deadzone));
       
        return stickVector;
    }
    
	public bool GetButton(string buttonNumString)
    {
		return Input.GetKey(buttonJoyName + " button " + buttonNumString);
    }
    
	public bool GetButtonDown(string buttonNumString)
    {
		return Input.GetKeyDown(buttonJoyName + " button " + buttonNumString);
    }
    
	public bool GetButtonUp(string buttonNumString)
    {
		return Input.GetKeyUp(buttonJoyName + " button " + buttonNumString);
    }
}
              
//http://grapefruitgames.com/2011/05/30/ps3-controllers-osx-unity-3d-together-at-last/

public class PS3ButtonType
{
    public string name;
    public string numStringOSX;
    
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

	//leftStick: x: Axis 1, y: Axis 2
	//rightStick: x: Axis 3, y: Axis 4
    
	public PS3ButtonType(string name, string numStringOSX)
    {
        this.name = name;
		this.numStringOSX = numStringOSX;
        
        allButtons.Add(this);
    }
}

//http://wiki.unity3d.com/index.php?title=Xbox360Controller

public class XboxButtonType
{
	public string name;
	public string numStringOSX;
	public string numStringWin;

	static public List<XboxButtonType> allButtons = new List<XboxButtonType>();

	//on windows, the dpad ONLY comes through Axis 6 and Axis 7
	public static XboxButtonType Up = 		new XboxButtonType("Up", "5", "19");
	public static XboxButtonType Right = 	new XboxButtonType("Right", "8", "19");
	public static XboxButtonType Down = 	new XboxButtonType("Down", "6", "19");
	public static XboxButtonType Left = 	new XboxButtonType("Left", "7", "19");

	public static XboxButtonType Y = 		new XboxButtonType("Y", "19", "3");
	public static XboxButtonType B = 		new XboxButtonType("B", "17", "1");
	public static XboxButtonType A = 		new XboxButtonType("A", "16", "0");
	public static XboxButtonType X = 		new XboxButtonType("X", "18", "2");

	public static XboxButtonType LB = 		new XboxButtonType("LB", "13", "4");
	public static XboxButtonType LT = 		new XboxButtonType("LT", "0", "19"); //only via Axis 9 win Axis 5 osx
	public static XboxButtonType LS = 		new XboxButtonType("LS", "11", "8");

	public static XboxButtonType RB = 		new XboxButtonType("RB", "14", "5");
	public static XboxButtonType RT = 		new XboxButtonType("RT", "0", "19"); //only via Axis 10 win, Axis 6 osx
	public static XboxButtonType RS = 		new XboxButtonType("RS", "12", "9");

	public static XboxButtonType Start = 	new XboxButtonType("Start", "9", "7");
	public static XboxButtonType Back = 	new XboxButtonType("Back", "10", "6");

	//OSX:
	//leftStick: x: Axis 1, y: Axis 2
	//rightStick: x: Axis 3, y: Axis 4
	//LT: Axis 5, RT: Axis 6

	//360
	//leftStick: x: Axis 1, y: Axis 2
	//rightStick: x: Axis 4, y: Axis 5
	//triggers: Axis 3 (-1 is full left, 1 is full right?)
	//dpad: x: Axis 6, y: Axis 7
	//LT: Axis 9
	//RT: Axis 10

	public XboxButtonType(string name, string numStringOSX, string numStringWin)
	{
		this.name = name;
		this.numStringOSX = numStringOSX;
		this.numStringWin = numStringWin;

		allButtons.Add(this);
	}
}







                            
