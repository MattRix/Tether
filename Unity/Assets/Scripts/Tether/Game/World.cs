using UnityEngine;
using System.Collections.Generic;
using System;

public class World : FContainer
{
	public static World instance;

	public FPWorld root;

	public FContainer backgroundHolder;
	public FContainer entityHolder;
	public FContainer effectHolder;

	public List<Beast> beasts = new List<Beast>();
	public List<Chain> chains = new List<Chain>();

	public List<Orb> orbs = new List<Orb>();

	public World()
	{
		instance = this;

		root = FPWorld.Create(64.0f);

		AddChild(backgroundHolder = new FContainer());
		AddChild(entityHolder = new FContainer());
		AddChild(effectHolder = new FContainer());

		CreateBeasts();
	}

	void CreateBeasts()
	{
		List<Player> players = GameManager.instance.activePlayers;
		float radiansPerPlayer = RXMath.DOUBLE_PI / (float)players.Count;
		float startRadius = 150.0f;

		for (int p = 0; p < players.Count; p++)
		{
			Vector2 startPos = new Vector2();
			startPos.x = Mathf.Cos(p * radiansPerPlayer) * startRadius;
			startPos.y = Mathf.Sin(p * radiansPerPlayer) * startRadius;
			Beast beast = Beast.Create(this);
			beast.Init(players[p], startPos);
			beasts.Add(beast);
		}

		for(int b = 0; b<beasts.Count; b++)
		{
			int firstIndex = b;
			int secondIndex = (b+1)%beasts.Count;

			Chain chain = new Chain(this, beasts[firstIndex], beasts[secondIndex]);
			chains.Add(chain);

			if(beasts.Count == 2)
			{
				return;
			}
		}

		for(int b = 0; b<beasts.Count; b++)
		{
			beasts[b].holder.MoveToFront();
		}
	}

	public void Destroy()
	{
		for(int b = 0; b<beasts.Count; b++)
		{
			beasts[b].Destroy();
		}

		for(int c = 0; c<chains.Count; c++)
		{
			chains[c].Destroy();
		}
	}
}


