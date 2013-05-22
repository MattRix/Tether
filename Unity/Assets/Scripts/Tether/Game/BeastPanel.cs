using UnityEngine;
using System.Collections.Generic;
using System;

public class BeastPanel : FContainer
{
	public Beast beast;

	public FSliceSprite bg;
	public FSliceSprite goldBG;

	public int currentPoints = 0;

	public float widthPerNib = 30.0f;
	public float fullWidth;

	public List<FContainer>nibs = new List<FContainer>();


	public BeastPanel(Beast beast)
	{
		this.beast = beast;

		beast.player.SignalPlayerChange += HandleSignalPlayerChange;

		fullWidth = GameConfig.WINNING_SCORE * widthPerNib;

		bg = new FSliceSprite("PointBarBG", fullWidth+6.0f, 30.0f, 0, 10, 0, 10);
		AddChild(bg);
		bg.color = beast.player.color;

		goldBG = new FSliceSprite("PointBarBGGold", fullWidth+6.0f, 30.0f, 0, 10, 0, 10);
		AddChild(goldBG);
		goldBG.alpha = 0.66f;
		goldBG.isVisible = false;

		for(int p = 0; p<GameConfig.WINNING_SCORE; p++)
		{
			FContainer nib = new FContainer();

			nib.x = -fullWidth*0.5f + 15.0f + p * widthPerNib;
			nib.y = 0;

			FSprite bgSprite = new FSprite("PointBarNibBG");
			bgSprite.color = beast.player.color;
			nib.AddChild(bgSprite);

			AddChild(nib);
			nibs.Add(nib);
		}

		ListenForUpdate(HandleUpdate);
	}

	void HandleUpdate()
	{
		scale += (1.0f - scale) / 12.0f;

		//blink lights when about to win

//		if (beast.player.score >= GameConfig.WINNING_SCORE - 1)
//		{
//			if(beast.player.score == GameConfig.WINNING_SCORE - 1 && World.instance.isGameOver) return; 
//
//			int nibCount = nibs.Count;
//
//			if(Time.frameCount % 1 == 0)
//			{
//				int randNib = RXRandom.Range(0,nibCount);
//				for(int n = 0; n<nibCount; n++)
//				{
//					bool isVis = n == randNib;
//					if(nibs[n].GetChildCount() == 3)
//					{
//						nibs[n].GetChildAt(1).alpha = 
//							nibs[n].GetChildAt(2).alpha = isVis ? 1.0f : 0.57f; 
//					}
//				}
//			}
//		}

		if (beast.player.score >= GameConfig.WINNING_SCORE - 1)
		{
			if(beast.player.score == GameConfig.WINNING_SCORE - 1 && World.instance.isGameOver) return; 
			
			int nibCount = nibs.Count;


			for(int n = 0; n<nibCount; n++)
			{
				float pulseAlpha = 0.8f + Mathf.Sin(n * 11.34f + Time.time * 22.0f) * 0.2f;
				if(nibs[n].GetChildCount() == 3)
				{
					nibs[n].GetChildAt(1).alpha = 
					nibs[n].GetChildAt(2).alpha = pulseAlpha;
				}
			}

		}
	}

	void HandleSignalPlayerChange ()
	{
		while (currentPoints < beast.player.score)
		{
			FSprite glowSprite = new FSprite("PointBarNib");
			glowSprite.color = beast.player.color;
			glowSprite.alpha = 0.8f;
			glowSprite.scale = 0.0f;
			nibs[currentPoints].AddChild(glowSprite);

			FSprite tipSprite = new FSprite("PointBarNib");
			tipSprite.scale = 0.0f;
			nibs[currentPoints].AddChild(tipSprite);

			Go.to(glowSprite, 0.3f, new TweenConfig().floatProp("scale",1.0f).setEaseType(EaseType.BackOut).setDelay(0.2f));
			Go.to(tipSprite, 0.3f, new TweenConfig().floatProp("scale",0.5f).setEaseType(EaseType.BackOut).setDelay(0.2f));

			currentPoints++;
		}

		scale = 1.1f;

	}
}


