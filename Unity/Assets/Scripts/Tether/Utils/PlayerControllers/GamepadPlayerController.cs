using System;
using UnityEngine;
using System.Collections.Generic;

public class GamepadPlayerController : PlayerController
{
	public int index;
	public Gamepad gamepad = null;
	
	public GamepadPlayerController(int index)
	{
		this.index = index;
		title = "Gamepad " + (index+1);

		UpdateGamepadState();
	}

	private void UpdateGamepadState()
	{
		if (GamepadManager.instance.isStateCertain)
		{
			bool wasGamepadNull = (gamepad == null);
			
			gamepad = GamepadManager.instance.GetGamepad(index);
			
			if(wasGamepadNull && gamepad != null)
			{
				didJustConnect = true;
			}
			else if(!wasGamepadNull && gamepad == null)
			{
				didJustDisconnect = true;
			}
		}
	}

	override public void Update()
	{
		UpdateGamepadState();

		if(gamepad == null)
		{
			movementVector.x = 0;
			movementVector.y = 0;
		}
		else
		{
			movementVector = gamepad.direction;
		}

		CalcuateDirectionsBasedOnMovementVector();
	}

	override public bool GetButtonDown(PlayerControllerButtonType buttonType)
	{
		if(gamepad == null) return false;

		if(buttonType == PlayerControllerButtonType.Ready)
		{
			if(gamepad.GetButtonDown(gamepad.buttonReady))
			{
				return true;
			}
		}
		else if(buttonType == PlayerControllerButtonType.Start || buttonType == PlayerControllerButtonType.Pause)
		{
			if(gamepad.GetButtonDown(gamepad.buttonStart))
			{
				return true;
			}
		}
		else if(buttonType == PlayerControllerButtonType.Reset)
		{
			if(gamepad.GetButtonDown(gamepad.buttonReset))
			{
				return true;
			}
		}

		return false;
	}

	override public bool CanBeUsed()
	{
		UpdateGamepadState();

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