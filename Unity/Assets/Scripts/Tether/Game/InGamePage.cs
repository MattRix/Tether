using UnityEngine;
using System.Collections.Generic;
using System;

public class InGamePage : TPage
{
	public World world;

	public InGamePage()
	{

	}
	
	override public void Start()
	{
		//FSoundManager.PlaySound("turbo");
		FSoundManager.PlayMusic("NO_DROP_VERSION", 0.8f);

		this.world = new World();
		AddChild(world);
	}

	override public void Destroy()
	{
		world.Destroy();
	}
}
