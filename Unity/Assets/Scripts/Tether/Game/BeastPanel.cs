using UnityEngine;
using System.Collections.Generic;
using System;

public class BeastPanel : FContainer
{
	public Beast beast;

	public FLabel scoreLabel;

	public BeastPanel(Beast beast)
	{
		this.beast = beast;

		beast.player.SignalPlayerChange += HandleSignalPlayerChange;

		AddChild(scoreLabel = new FLabel("CubanoBig", beast.player.score.ToString()));

		scoreLabel.color = beast.player.color;

	}

	void HandleSignalPlayerChange ()
	{
		scoreLabel.text = beast.player.score.ToString();
	}
}


