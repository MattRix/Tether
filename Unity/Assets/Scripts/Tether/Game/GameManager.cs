using UnityEngine;
using System;
using System.Collections.Generic;

public class GameManager
{
	static public GameManager instance;

	public static void Init() {instance = new GameManager();}

	public List<Player> players = new List<Player>();
	public List<Player> activePlayers = null;
	
	public GameManager()
	{
		players.Add(new Player(0, "PURPLE PLAYER", RXColor.GetColorFromHex(0xFF00EE), false));
		players.Add(new Player(1, "GREEN PLAYER", RXColor.GetColorFromHex(0x00FF00), false));
		players.Add(new Player(2, "BLUE PLAYER", RXColor.GetColorFromHex(0x0000FF), true));
		players.Add(new Player(3, "RED PLAYER", RXColor.GetColorFromHex(0xFF0000), true));
	}

	public void SetRoundData(List<Player> activePlayers)
	{
		this.activePlayers = activePlayers;
	}

	public void Reset()
	{
		foreach (Player player in players)
		{
			player.Reset();
		}
	}
}


