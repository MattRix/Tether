using System;
using UnityEngine;
using System.Collections.Generic;

public class AIPlayerController : PlayerController
{
	public AISymbolicPlayerController symbolic;

	public Vector2 targetIdlePos = new Vector2();


	public float timeUntilRetarget = 0.0f;
	public float timeUntilUpdateIdle = 0.0f;

	public Vector2 targetMovementVector = new Vector2();

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

		timeUntilRetarget -= Time.deltaTime;

		if(timeUntilRetarget <= 0)
		{
			UpdateMovementVector();
		}

		movementVector += (targetMovementVector - movementVector) * 0.7f;
	}

	void UpdateMovementVector()
	{
		timeUntilRetarget = RXRandom.Range(0.2f,0.5f);

		World world = World.instance;
		Beast beast = world.beasts[_player.index];

		Vector2 beastPos = beast.GetPos();

		Orb closestOwnOrb = null;
		Orb closestEnemyOrb = null;

		float closestOwnDist = float.MaxValue;
		float closestEnemyDist = float.MaxValue;

		for(int r = 0; r<world.orbs.Count; r++)
		{
			Orb checkOrb = world.orbs[r];

			Vector2 delta = checkOrb.GetPos() - beastPos;

			float dist = delta.magnitude;

			if(checkOrb.player == _player)
			{
				if(dist < closestOwnDist)
				{
					closestOwnDist = dist;
					closestOwnOrb = checkOrb;
				}
			}
			else
			{
				if(dist < closestEnemyDist)
				{
					closestEnemyDist = dist;
					closestEnemyOrb = checkOrb;
				}
			}


		}

		Orb orbToTarget = null;
		bool shouldGoTowardsOrb = true;

		//
		//dist *= 0.8f; //prioritize own orbs slightly

		if(closestOwnDist < closestEnemyDist)
		{
			orbToTarget = closestOwnOrb;
			shouldGoTowardsOrb = true;
		}
		else 
		{
			if(closestEnemyOrb != null)
			{
				bool isPlayerAboutToWin = closestEnemyOrb.player.score == GameConfig.WIN_SCORE-1;

				Beast enemyBeast = world.beasts[closestEnemyOrb.player.index];

				Vector2 enemyDelta = closestEnemyOrb.GetPos() - enemyBeast.GetPos();

				float enemyDist = enemyDelta.magnitude;

				if(enemyDist > closestEnemyDist + 20.0f) //if we're at least 20 px closer, go knock it away!
				{
					orbToTarget = closestEnemyOrb;
					shouldGoTowardsOrb = true;
				}
				else
				{
					//prioritize our own orbs slightly normally, except when someone is about to win
					float fearModifier = isPlayerAboutToWin ? 1.2f : 0.8f; //lower means it'll avoid its own orbs to prevent a win

					if(enemyDist < closestOwnDist * fearModifier) 
					{
						orbToTarget = closestEnemyOrb;
						shouldGoTowardsOrb = false;
					}
					else 
					{
						orbToTarget = closestOwnOrb;
						shouldGoTowardsOrb = true;
					}
				}
			}
		}

		//beast.

		if(orbToTarget != null)
		{
			Vector2 deltaToClosest = orbToTarget.GetPos() - beastPos;

			if(shouldGoTowardsOrb)
			{
				targetMovementVector = deltaToClosest.normalized;
			}
			else
			{
				targetMovementVector = -deltaToClosest.normalized;
			}
		}
		else //do idle motion
		{
			Vector2 deltaToTarget = targetIdlePos - beastPos;

			while(deltaToTarget.magnitude < 60.0f)
			{
				GetNewIdlePos();
				deltaToTarget = targetIdlePos - beastPos;
			}

			targetMovementVector = deltaToTarget.normalized;
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