using UnityEngine;
using System.Collections.Generic;
using System;


public class PlayerSelectPanel : FContainer
{
	public FLabel nameLabel;
	public FLabel readyLabel;
	
	public Player player;
	
	public FSliceSprite background;
	
	public Action signalStateChange;
	
	public PlayerSelectPanel(Player player)
	{
		this.player = player;
		
		AddChild(background = new FSliceSprite("Popup_Bg",520,200,16,16,16,16));
		background.color = player.color;
		background.alpha = 0.4f;
		
		AddChild(nameLabel = new FLabel("CubanoBig", player.name));
		nameLabel.scale = 1.0f;
		nameLabel.y = 30.0f;
		
		AddChild(readyLabel = new FLabel("CubanoBig", ""));
		readyLabel.y = -30.0f;
		readyLabel.scale = 0.75f;
		
		ListenForUpdate(HandleUpdate);
		
		UpdateState();
	}
	
	void HandleUpdate()
	{
		if (player.isConnected)
		{
			if(player.gamepad.GetButtonDown(PS3ButtonType.X))
			{
				player.isReady = !player.isReady;
				UpdateState();
				background.scale = 1.1f;
				
				if(player.isReady)
				{
					FSoundManager.PlaySound("tone");
				}
				else 
				{
					FSoundManager.PlaySound("click4");
				}
			}
		}
		
		background.scale += (1.0f - background.scale) / 10.0f;
	}
	
	void UpdateState()
	{
		if (player.isConnected)
		{
			if(player.isReady)
			{
				readyLabel.text = "READY!";
				readyLabel.color = RXColor.GetColorFromHex(0xFFFFFF);
			}
			else 
			{
				readyLabel.text = "PRESS X!";
				readyLabel.color = RXColor.GetColorFromHex(0xFF9966);
			}
		}
		else
		{
			readyLabel.text = "UNPLUGGED";
			readyLabel.color = RXColor.GetColorFromHex(0xFF0000);
		}
		
		if(signalStateChange != null) signalStateChange();
	}
}
