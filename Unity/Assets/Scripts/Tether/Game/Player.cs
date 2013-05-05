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

		if (!GameConfig.SHOULD_SIMULATE_FOUR_PLAYER) isSpecial = false;

		this.isSpecial = isSpecial;

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
			isReady = true;

			if (index >= 2)
			{
				isReady = false;
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

