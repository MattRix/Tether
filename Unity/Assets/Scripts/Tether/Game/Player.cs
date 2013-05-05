using UnityEngine;
using System.Collections;
using System;

public class Player
{
	public int index;
	public Gamepad gamepad;
	public string name;

	public bool isConnected = false;

	public bool isReady = false;

	public string numString;

	public Color color;

	public bool isSpecial;

	public int score;

	public event Action SignalPlayerChange;

	public Player(int index, String name, Color color, bool isSpecial)
	{
		this.index = index;
		this.name = name;
		this.isSpecial = isSpecial;

		if (!GameConfig.SHOULD_SIMULATE_FOUR_PLAYER) isSpecial = false;

		if (isSpecial)
		{
			this.gamepad = GamepadManager.instance.GetGamepad(index-2);
		}
		else
		{
			this.gamepad = GamepadManager.instance.GetGamepad(index);
		}

		this.numString = (index + 1).ToString();

		this.color = color;

		if (this.gamepad != null)
		{
			isConnected = true;
			
			if(GameConfig.IS_DEBUG)
			{
				isReady = true;
			}
		}

		Reset();
	}

	public void Reset()
	{
		score = 0;
	}

	public void AddScore()
	{
		score++;

		if (SignalPlayerChange != null) SignalPlayerChange();
	}
}

