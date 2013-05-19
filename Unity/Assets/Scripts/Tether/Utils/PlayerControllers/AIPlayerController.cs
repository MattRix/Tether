using System;
using UnityEngine;
using System.Collections.Generic;

public class AIPlayerController : PlayerController
{
	public AISymbolicPlayerController symbolic;

	public AIPlayerController(AISymbolicPlayerController symbolic)
	{
		this.symbolic = symbolic;

		title = "AI";
	}

	override public void Update()
	{
		
	}

	override public bool CanBeUsed()
	{
		return true;
	}
}

public class AISymbolicPlayerController : PlayerController
{
	public AISymbolicPlayerController()
	{
		title = "AI";
	}

	override public bool CanBeUsed()
	{
		return true;
	}

	override public void SetPlayer(Player player)
	{
		//DO NOTHING, _player is always null
	}

	override public Player GetPlayer()
	{
		return null;
	}

	public PlayerController CreateActualController()
	{
		return new AIPlayerController(this);
	}
}