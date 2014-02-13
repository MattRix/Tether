using UnityEngine;
using System.Collections;
using System;

public class Player
{
	public int index;
	public PlayerController controller;
	public string name;

	public string numString;

	public Team team;

	public int score;

	public Color color;

	public Player(int index, String name, Team team, Color color, PlayerController controller)
	{
		this.index = index;
		this.name = name;
		this.numString = (index + 1).ToString();

		this.color = color;
		this.team = team;
		this.controller = controller;
		this.controller.SetPlayer(this);
	}
}

