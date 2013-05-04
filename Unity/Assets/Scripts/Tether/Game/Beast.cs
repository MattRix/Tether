using UnityEngine;
using System.Collections.Generic;
using System;

public class Beast : MonoBehaviour
{
	public static Beast Create(World world)
	{
		GameObject beastGO = new GameObject("Beast");
		Beast beast = beastGO.AddComponent<Beast>();
		beast.world = world;
		return beast;
	}

	public World world;
	public Player player;

	public void Init(Player player, Vector2 startPos)
	{
		this.player = player;
	}

	public void Destroy()
	{
		UnityEngine.Object.Destroy(gameObject);
	}
}


