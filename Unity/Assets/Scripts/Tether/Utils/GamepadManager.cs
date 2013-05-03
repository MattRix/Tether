using System;
using UnityEngine;
using System.Collections.Generic;


public class GamepadManager
{
    static public GamepadManager instance;

    private List<Gamepad> _gamepads = new List<Gamepad>();

    public static void Init()
    {
        instance = new GamepadManager();
    }

    public GamepadManager()
    {
        Update();
    }

    public int GetGamepadCount()
    {
        return _gamepads.Count;
	}
    
    public Gamepad GetGamepad(int index)
    {
        if (index < 0 || index >= _gamepads.Count) throw new Exception("Attempt to access gamepad " + index + " failed");
        
        return _gamepads [index];   
	}
    
    public void Update()
    {
        string[] joystickNames = Input.GetJoystickNames();

        int countDelta = joystickNames.Length - _gamepads.Count;

        if (countDelta > 0)
        {
            Debug.Log("Just added " + countDelta + " gamepad" + (countDelta == 1 ? "" : "s"));
            while(countDelta > 0)
            {
                countDelta --;
                Gamepad gamepad = new Gamepad();
                _gamepads.Add(gamepad);
            }
        } 
        else if(countDelta < 0)
        {
            Debug.Log("Just removed " + (-countDelta) + " gamepad" + (countDelta == -1 ? "" : "s"));

            while(countDelta < 0)
            {
                countDelta++;
                _gamepads.Pop();
            }
        } 
 
        for(int g = 0; g<_gamepads.Count; g++)
        {
            _gamepads[g].index = g;
            _gamepads[g].Update();
        }
    }
}

