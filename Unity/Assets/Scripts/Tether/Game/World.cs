using UnityEngine;
using System.Collections.Generic;
using System;

public class World : FContainer
{
	public static World instance;


	public FContainer backgroundHolder;
	public FContainer entityHolder;
	public FContainer effectHolder;

	public List<Beast> beasts = new List<Beast>();

	public World()
	{
		instance = this;

		AddChild(backgroundHolder = new FContainer());
		AddChild(entityHolder = new FContainer());
		AddChild(effectHolder = new FContainer());

		CreateBeasts();
	}

	void CreateBeasts()
	{
		List<Player> players = GameManager.instance.activePlayers;
		float radiansPerPlayer = RXMath.DOUBLE_PI / (float)players.Count;
		float startRadius = 100.0f;
		for (int p = 0; p < players.Count; p++)
		{
			Vector2 startPos = new Vector2();
			startPos.x = Mathf.Cos(p * radiansPerPlayer) * startRadius;
			startPos.y = Mathf.Sin(p * radiansPerPlayer) * startRadius;
			Beast beast = Beast.Create(this);
			beast.Init(players[p], startPos);
			beasts.Add(beast);
		}
	}
}


