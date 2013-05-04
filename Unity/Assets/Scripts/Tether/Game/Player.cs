using UnityEngine;
using System.Collections;
using System;

public class Player
{
	public int index;
	public Gamepad gamepad;

	public bool isConnected = false;

	public bool isReady = false;

	public string numString;

	public Color color;

	public Player(int index, Color color)
	{
		this.index = index;
		this.gamepad = GamepadManager.instance.GetGamepad(index);

		if (this.gamepad != null)
		{
			isConnected = true;

			if(GameConfig.IS_DEBUG)
			{
				isReady = true;
			}
		}

		this.numString = (index + 1).ToString();

		this.color = color;


	}
}

