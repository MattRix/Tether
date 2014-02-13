using UnityEngine;
using System;
using System.Collections.Generic;

public class GameManager
{
	static public GameManager instance;

	public static void Init() {instance = new GameManager();}

	public List<Team> teams = new List<Team>();
	public List<Team> activeTeams = null;

	public List<Player> players = new List<Player>();
	public List<Player> activePlayers = null;

	public List<PlayerController> availablePlayerControllers = new List<PlayerController>();

	public UnusedPlayerController unusedPlayerController;
	public AISymbolicPlayerController aiPlayerController;
	
	public GameManager()
	{
		unusedPlayerController = new UnusedPlayerController();
		aiPlayerController = new AISymbolicPlayerController();
		
		availablePlayerControllers.Add(unusedPlayerController);
		availablePlayerControllers.Add(new KeyboardPlayerController(false));
		availablePlayerControllers.Add(new KeyboardPlayerController(true));
		availablePlayerControllers.Add(new GamepadPlayerController(0));
		availablePlayerControllers.Add(new GamepadPlayerController(1));
		availablePlayerControllers.Add(new GamepadPlayerController(2));
		availablePlayerControllers.Add(new GamepadPlayerController(3));
		availablePlayerControllers.Add(aiPlayerController);

		teams.Add(new Team(0,"PURPLE",RXColor.GetColorFromHex(0xFF00EE)));
		teams.Add(new Team(1,"GREEN",RXColor.GetColorFromHex(0x00FF00)));
		teams.Add(new Team(2,"BLUE",RXColor.GetColorFromHex(0x0011EE)));
		teams.Add(new Team(3,"RED",RXColor.GetColorFromHex(0xFF0011)));

		players.Add(new Player(0, "PURPLE", teams[2], RXColor.GetColorFromHex(0xFF00EE), unusedPlayerController));
		players.Add(new Player(1, "GREEN", teams[3],RXColor.GetColorFromHex(0x00FF00), unusedPlayerController));
		players.Add(new Player(2, "BLUE", teams[2],RXColor.GetColorFromHex(0x0011EE), unusedPlayerController));
		players.Add(new Player(3, "RED", teams[3],RXColor.GetColorFromHex(0xFF0011), unusedPlayerController));
	}

	public void SetRoundData(List<Player> activePlayers)
	{
		this.activePlayers = activePlayers;

		activeTeams.Clear();

		for(int p = 0; p<activePlayers.Count; p++)
		{
			Team team = activePlayers[p].team;
			if(!activeTeams.Contains(team))
			{
				activeTeams.Add(team);
			}
		}

	}

	public void Reset()
	{
		for(int t = 0; t<activeTeams.Count; t++)
		{
			activeTeams[t].Reset();
		}
	}

	public PlayerController GetNextAvailablePlayerController(PlayerController currentPC)
	{
		int pcCount = availablePlayerControllers.Count;

		int index = (1 + availablePlayerControllers.IndexOf(currentPC)) % pcCount;

		for(int c = 0; c<pcCount; c++)
		{
			PlayerController pcToCheck = availablePlayerControllers[index];

			if(pcToCheck.CanBeUsed() && pcToCheck.GetPlayer() == null)
			{
				return pcToCheck;
			}

			index = (index+1) % pcCount; //increment the index
		}

		return unusedPlayerController;
	}
}


