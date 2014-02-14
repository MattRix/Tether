using UnityEngine;
using System.Collections;
using System;

public class Team
{
	public int index;
	public string name;
	public Color color;
	public string numString;

	public int score;

	public event Action SignalTeamChange;
	
	public Team(int index, String name, Color color)
	{
		this.index = index;
		this.name = name;
		this.color = color;
		this.numString = (index + 1).ToString();
		
		Reset();
	}
	
	public void Reset()
	{
		score = 0;
	}
	
	public void AddScore()
	{
		score++;
		
		if (SignalTeamChange != null) SignalTeamChange();
	}
}

