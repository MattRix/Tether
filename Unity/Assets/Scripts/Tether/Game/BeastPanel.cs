using UnityEngine;
using System.Collections.Generic;
using System;

public class BeastPanel : FContainer
{
	public Beast beast;

	public FSliceSprite bg;

	public int currentPoints = 0;

	public float widthPerNib = 30.0f;
	public float fullWidth;

	public List<FContainer>nibs = new List<FContainer>();

	public BeastPanel(Beast beast)
	{
		this.beast = beast;

		beast.player.SignalPlayerChange += HandleSignalPlayerChange;

		fullWidth = GameConfig.WIN_SCORE * widthPerNib;

		bg = new FSliceSprite("PointBarBG", fullWidth+6.0f, 30.0f, 0, 10, 0, 10);
		AddChild(bg);
		bg.color = beast.player.color;

		for(int p = 0; p<GameConfig.WIN_SCORE; p++)
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


