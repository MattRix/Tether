using System;
using UnityEngine;
using System.Collections.Generic;

public class AIPlayerController : PlayerController
{
	public AISymbolicPlayerController symbolic;

	public Vector2 targetIdlePos = new Vector2();

	public float timeUntilUpdateIdle = 0.0f;

	public AIPlayerController(AISymbolicPlayerController symbolic)
	{
		this.symbolic = symbolic;

		title = "AI";
	}

	override public void Update()
	{
		timeUntilUpdateIdle -= Time.deltaTime;

		if(timeUntilUpdateIdle <= 0)
		{
			GetNewIdlePos();
		}


		World world = World.instance;
		Beast beast = world.beasts[_player.index];

		Vector2 beastPos = beast.GetPos();

		Orb closestOrb = null;

		float closestDist = float.MaxValue;

		for(int r = 0; r<world.orbs.Count; r++)
		{
			Orb checkOrb = world.orbs[r];

			Vector2 delta = checkOrb.GetPos() - beastPos;

			float dist = delta.magnitude;

			if(dist < closestDist)
			{
				closestDist = dist;
				closestOrb = checkOrb;
			}

		}

		if(closestOrb != null)
		{
			Vector2 deltaToClosest = closestOrb.GetPos() - beastPos;

			if(closestOrb.player == _player) //closest is good!
			{
				movementVector = deltaToClosest.normalized;
			}
			else //closest is bad!
			{
				movementVector = -deltaToClosest.normalized;
			}
		}
		else 
		{
			Vector2 deltaToTarget = targetIdlePos - beastPos;

			while(deltaToTarget.magnitude < 60.0f)
			{
				GetNewIdlePos();
				deltaToTarget = targetIdlePos - beastPos;
			}

			movementVector = deltaToTarget.normalized;
		}

	}

	void GetNewIdlePos()
	{
		float aWidth = TMain.instance.background.sprite.width * 0.33f;
		float aHeight = TMain.instance.background.sprite.height * 0.33f;
		targetIdlePos.x = RXRandom.Range(-aWidth,aWidth);
		targetIdlePos.y = RXRandom.Range(-aHeight,aHeight);
		timeUntilUpdateIdle = RXRandom.Range(2.0f,10.0f);
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