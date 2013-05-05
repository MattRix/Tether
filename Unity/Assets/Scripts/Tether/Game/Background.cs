using System;
using System.Collections.Generic;
using UnityEngine;

public class Background : FContainer
{
	public FSprite sprite;

	public Background()
	{
		AddChild(sprite = new FSprite("background"));
	}
}


