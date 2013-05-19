using UnityEngine;
using System.Collections;
using System;

public class Player
{
	public int index;
	public PlayerController controller;
	public string name;

	public string numString;

	public Color color;

	public int score;

	public event Action SignalPlayerChange;

	public Player(int index, String name, Color color, PlayerController controller)
	{
		this.index = index;
		this.name = name;
		this.numString = (index + 1).ToString();

		this.color = color;
		this.controller = controller;
		this.controller.SetPlayer(this);

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

