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

	public FButton button;
	
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

		AddChild(button = new FButton("Popup_Bg"));
		button.sprite.width = 520;
		button.sprite.height = 200;
		button.alpha = 0.0f; //hidden button
		button.SignalPress += HandleSignalPress;
		
		ListenForUpdate(HandleUpdate);
		
		UpdateState();
	}

	void HandleSignalPress (FButton button)
	{
		background.scaleX = 1.05f;
		background.scaleY = 1.1f;

		PlayerController oldPC = player.controller;

		player.controller = GameManager.instance.GetNextAvailablePlayerController(oldPC);

		oldPC.SetPlayer(null);
		player.controller.SetPlayer(player);

		UpdateState();
	}
	
	void HandleUpdate()
	{
//		if (player != null && player.controller != null & player.controller.CanBeUsed())
//		{
//			if(player.controller.GetButtonDown(PlayerControllerButtonType.Ready))
//			{
//				UpdateState();
//				background.scale = 1.1f;
//			}
//		}
//		
		background.scaleX += (1.0f - background.scaleX) / 10.0f;
		background.scaleY += (1.0f - background.scaleY) / 10.0f;
	}
	
	void UpdateState()
	{
		if (player.controller == GameManager.instance.unusedPlayerController)
		{
			readyLabel.text = "[UNUSED]";
			readyLabel.color = RXColor.GetColorFromHex(0xFF0000);
		}
		else
		{
			readyLabel.text = player.controller.title;
			readyLabel.color = RXColor.GetColorFromHex(0xFFFFFF);
		}
		
		if(signalStateChange != null) signalStateChange();
	}
}
