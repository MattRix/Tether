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
}

