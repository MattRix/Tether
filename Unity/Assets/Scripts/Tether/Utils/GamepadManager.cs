using System;
using UnityEngine;
using System.Collections.Generic;


public class GamepadManager
{
    static public GamepadManager instance;

    public List<Gamepad> gamepads = new List<Gamepad>();

    public static void Init()
    {
        instance = new GamepadManager();
    }

    public GamepadManager()
    {
		GenerateInputManagerData ();


        Update();

    }

    public Gamepad GetGamepad(int index)
    {
		if (index < 0 || index >= gamepads.Count) return null;
        
        return gamepads [index];   
	}
    
    public void Update()
    {
        string[] joystickNames = Input.GetJoystickNames();

        int countDelta = joystickNames.Length - gamepads.Count;

        if (countDelta > 0)
        {
            Debug.Log("Just added " + countDelta + " gamepad" + (countDelta == 1 ? "" : "s"));
            while(countDelta > 0)
            {
                countDelta --;
                Gamepad gamepad = new Gamepad();
				Debug.Log("Adding gamepad " + joystickNames[gamepads.Count]);
                gamepads.Add(gamepad);
            }
        } 
        else if(countDelta < 0)
        {
            Debug.Log("Just removed " + (-countDelta) + " gamepad" + (countDelta == -1 ? "" : "s"));

            while(countDelta < 0)
            {
                countDelta++;
                gamepads.Pop();
            }
        } 
 
        for(int g = 0; g<gamepads.Count; g++)
        {
            gamepads[g].index = g;
            gamepads[g].Update();
        }
    }

	private static string TAB_CHAR = "  ";

	void GenerateInputManagerData ()
	{
		string fullOutput = "%YAML 1.1\n";
		fullOutput += "%TAG !u! tag:unity3d.com,2011:\n";
		fullOutput += "--- !u!13 &1\n";
		fullOutput += "InputManager:\n";
		fullOutput += TAB_CHAR+"m_ObjectHideFlags: 0\n";
		fullOutput += TAB_CHAR+"m_Axes:\n";

		for (int j = 0; j<11; j++) 
		{
			string output = "";
			for(int a = 0; a<10; a++)
			{
				output += GetAxisInputManagerData(j+1,0,"Axis "+(a+1),a);
			}

			fullOutput += output;
		}

		System.IO.File.WriteAllText("Assets/Notes/InputManagerData.txt", fullOutput);
		Debug.Log(fullOutput);
	}

	string GetAxisInputManagerData(int ordinal, int invert, string axisName, int axisIndex)
	{
		string output = TAB_CHAR+"- serializedVersion: 3\n";
		output += TAB_CHAR+TAB_CHAR+"m_Name: Joystick "+ordinal.ToString()+" "+axisName+"\n";
		output += TAB_CHAR+TAB_CHAR+"descriptiveName:\n";
		output += TAB_CHAR+TAB_CHAR+"descriptiveNegativeName:\n";
		output += TAB_CHAR+TAB_CHAR+"negativeButton:\n";
		output += TAB_CHAR+TAB_CHAR+"positiveButton:\n";
		output += TAB_CHAR+TAB_CHAR+"altNegativeButton:\n";
		output += TAB_CHAR+TAB_CHAR+"altPositiveButton:\n";
		output += TAB_CHAR+TAB_CHAR+"gravity: 3\n";
		output += TAB_CHAR+TAB_CHAR+"dead: .00100000005\n";
		output += TAB_CHAR+TAB_CHAR+"sensitivity: 1\n";
		output += TAB_CHAR+TAB_CHAR+"snap: 0\n";
		output += TAB_CHAR+TAB_CHAR+"invert: " + invert.ToString() + "\n";
		output += TAB_CHAR+TAB_CHAR+"type: 2" + "\n";
		output += TAB_CHAR+TAB_CHAR+"axis: " + axisIndex.ToString() + "\n";
		output += TAB_CHAR+TAB_CHAR+"joyNum: " +ordinal.ToString() + "\n";

		return output;
	}
}

