using System;
using UnityEngine;
using System.Collections.Generic;

public class KeyboardPlayerController : PlayerController
{
	public bool isWASD;

	public KeyCode keyUp;
	public KeyCode keyDown;
	public KeyCode keyLeft;
	public KeyCode keyRight;

	public Vector2 targetVector = new Vector2();

	public KeyboardPlayerController(bool isWASD)
	{
		this.isWASD = isWASD;

		if(isWASD)
		{
			title = "WASD";
			keyUp = KeyCode.W;
			keyDown = KeyCode.S;
			keyLeft = KeyCode.A;
			keyRight = KeyCode.D;
		}
		else
		{
			title = "ARROW KEYS";
			keyUp = KeyCode.UpArrow;
			keyDown = KeyCode.DownArrow;
			keyLeft = KeyCode.LeftArrow;
			keyRight = KeyCode.RightArrow;
		}
	}

	override public void Update()
	{
		movementVector *= 0.90f;

		targetVector.x = 0;
		targetVector.y = 0;

		if(Input.GetKey(keyUp))
		{
			targetVector.y += 1.0f;
		}

		if(Input.GetKey(keyDown))
		{
			targetVector.y -= 1.0f;
		}

		if(Input.GetKey(keyLeft))
		{
			targetVector.x -= 1.0f;
		}

		if(Input.GetKey(keyRight))
		{
			targetVector.x += 1.0f;
		}

		movementVector += (targetVector - movementVector) * 0.7f;
	}

	override public bool GetButtonDown(PlayerControllerButtonType buttonType)
	{
		if(buttonType == PlayerControllerButtonType.Ready)
		{
			if(Input.GetKeyDown(keyUp) || Input.GetKeyDown(keyDown) || Input.GetKeyDown(keyLeft) || Input.GetKeyDown(keyRight))
			{
				return true;
			}
		}
		else if(buttonType == PlayerControllerButtonType.Start || buttonType == PlayerControllerButtonType.Pause)
		{
			if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
			{
				return true;
			}
		}
		else if(buttonType == PlayerControllerButtonType.Reset)
		{
			if(Input.GetKeyDown(KeyCode.Backspace))
			{
				return true;
			}
		}

		return false;
	}

	override public bool CanBeUsed()
	{
		return (SystemInfo.deviceType == DeviceType.Desktop);
	}
}