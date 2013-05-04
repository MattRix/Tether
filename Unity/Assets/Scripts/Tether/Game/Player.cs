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

	public bool isSpecial;

	public int score;

	public Player(int index, Color color, bool isSpecial)
	{
		this.index = index;
		this.isSpecial = isSpecial;

		if (isSpecial)
		{
			this.gamepad = GamepadManager.instance.GetGamepad(index-2);
		}
		else
		{
			this.gamepad = GamepadManager.instance.GetGamepad(index);
		}

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

		Reset();
	}

	public void Reset()
	{
		score = 0;
		isReady = false;
	}

	public void AddScore()
	{
		score++;
	}
}

