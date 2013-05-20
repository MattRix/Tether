using System;
using UnityEngine;
using System.Collections.Generic;

public class GamepadPlayerController : PlayerController
{
	public int index;

	public GamepadPlayerController(int index)
	{
		this.index = index;
		title = "Gamepad " + (index+1);
	}

	override public void Update()
	{
		Gamepad gamepad = GamepadManager.instance.GetGamepad(index);

		if(gamepad == null)
		{
			movementVector.x = 0;
			movementVector.y = 0;
		}
		else
		{
			movementVector = gamepad.leftStick;
		}
	}

	override public bool GetButtonDown(PlayerControllerButtonType buttonType)
	{
		return false;
	}

	override public bool CanBeUsed()
	{
		Gamepad gamepad = GamepadManager.instance.GetGamepad(index);

		if(gamepad == null)
		{
			return false;
		}
		else
		{
			return true;
		}
	}
}