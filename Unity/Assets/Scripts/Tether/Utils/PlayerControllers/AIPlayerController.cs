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

		CalcuateDirectionsBasedOnMovementVector();
	}

	void UpdateMovementVector()
	{
		timeUntilRetarget = RXRandom.Range(0.2f,0.5f);

		World world = World.instance;
		Beast beast = world.GetBeastForPlayer(_player);

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
				bool isPlayerAboutToWin = closestEnemyOrb.player.team.score == GameConfig.WINNING_SCORE-1;

				Beast enemyBeast = world.GetBeastForPlayer(closestEnemyOrb.player);

				Vector2 enemyDelta = closestEnemyOrb.GetPos() - enemyBeast.GetPos();

				float enemyDist = enemyDelta.magnitude;

				if(enemyDist > closestEnemyDist + 30.0f) //if we're at least 30 px closer, go knock it away!
				{
					orbToTarget = closestEnemyOrb;
					shouldGoTowardsOrb = true;
				}
				else
				{
					//prioritize our own orbs slightly normally, except when someone is about to win
					float cautionModifier = 0.3f; //higher means it'll avoid its own orbs to prevent a win

					if(isPlayerAboutToWin) //if someone is about to win, fear them!
					{
						cautionModifier = 1.2f; 
					}
					else if(enemyBeast.player.team.score <= beast.player.team.score - 2)//if we're beating them by 2, have less fear of them
					{
						cautionModifier = 0.05f;
					}

					if(enemyDist < closestOwnDist * cautionModifier) 
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
				float deltaAngle = deltaToClosest.GetRadians();
				deltaAngle += RXRandom.Range(-1.0f,1.0f);

				Vector2 moveDelta = new Vector2(Mathf.Cos(deltaAngle), Mathf.Sin(deltaAngle));

				targetMovementVector = -moveDelta.normalized;
			}
		}
		else //do idle motion
		{
			Vector2 deltaToTarget = targetIdlePos - beastPos;

			while(deltaToTarget.magnitude < 30.0f)
			{
				GetNewIdlePos();
				deltaToTarget = targetIdlePos - beastPos;
			}

			targetMovementVector = deltaToTarget.normalized;
		}
	}

	void GetNewIdlePos()
	{
		if(RXRandom.Float() < 0.25f) //stand still sometimes
		{
			Vector2 beastPos = World.instance.GetBeastForPlayer(_player).GetPos();
			targetIdlePos.x = beastPos.x;
			targetIdlePos.y = beastPos.y;
		}
		else 
		{
			float w = 1280.0f * 0.33f;
			float h = 720.0f * 0.33f;
			targetIdlePos.x = RXRandom.Range(-w,w);
			targetIdlePos.y = RXRandom.Range(-h,h);
		}

		timeUntilUpdateIdle = RXRandom.Range(2.0f,6.0f);
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