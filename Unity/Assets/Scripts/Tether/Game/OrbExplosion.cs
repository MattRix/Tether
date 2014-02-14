using System;
using System.Collections.Generic;
using UnityEngine;

public class OrbExplosion : FContainer
{
	public bool isGood;
	public Team team;

	public int frameIndex = 0;
	public FAtlasElement[] frames;

	public FSprite sprite;

	public OrbExplosion(bool isGood, Team team)
	{
		this.isGood = isGood;
		this.team = team;

		if (isGood)
		{
			frames = new FAtlasElement[7];
			
			for (int f = 0; f<frames.Length; f++)
			{
				frames[f] = Futile.atlasManager.GetElementWithName("orb grab"+f.ToString("00"));
			}
		}
		else
		{
			frames = new FAtlasElement[4];
			
			for (int f = 0; f<frames.Length; f++)
			{
				frames[f] = Futile.atlasManager.GetElementWithName("orb_2_Crash"+f.ToString("00"));
			}
		}

		AddChild(sprite = new FSprite(frames [0].name));

		sprite.color = team.color;

		if (isGood)
		{
			sprite.y += 20.0f;
			sprite.scale = 0.75f;
		}

		ListenForUpdate(HandleUpdate);
	}

	void HandleUpdate()
	{
		int frameRate = 2;
		
		if (Time.frameCount % frameRate == 0)
		{
			frameIndex++;
			if(frameIndex == frames.Length)
			{
				this.RemoveFromContainer();
			}
			else 
			{
				sprite.element = frames[frameIndex];
			}
		}
	}
}


