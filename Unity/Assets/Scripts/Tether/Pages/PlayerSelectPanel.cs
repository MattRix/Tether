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
		DoPulseEffect();

		PlayerController oldPC = player.controller;

		player.controller = GameManager.instance.GetNextAvailablePlayerController(oldPC);

		oldPC.SetPlayer(null);
		player.controller.SetPlayer(player);

		UpdateState();
	}

	void DoPulseEffect()
	{
		background.scaleX = 1.05f;
		background.scaleY = 1.1f;
		FSoundManager.PlaySound("click4", 0.2f);
	}
	
	void HandleUpdate()
	{
		//first, check if we should add a controller that is pressing its back/select button

		bool didJustAddController = false;

		if(player.controller == GameManager.instance.unusedPlayerController)
		{
			List<PlayerController> pcs = GameManager.instance.availablePlayerControllers;

			for(int p = 0; p<pcs.Count; p++)
			{
				PlayerController pc = pcs[p];

				if(pc.GetPlayer() == null && pc.CanBeUsed() && pc.GetButtonDown(PlayerControllerButtonType.Reset))
				{
					player.controller.SetPlayer(null);
					player.controller = pc;
					pc.SetPlayer(player);
					UpdateState();
					DoPulseEffect();
					didJustAddController = true;
					break;
				}
			}
		}

		//add AI players if someone presses I

		if(Input.GetKeyDown(KeyCode.I))
		{
			if(player.controller == GameManager.instance.unusedPlayerController)
			{
				player.controller = GameManager.instance.aiPlayerController;
				UpdateState();
				DoPulseEffect();
				didJustAddController = true;
			}
			return;
		}

		if (!didJustAddController && player.controller.CanBeUsed())
		{
			if(player.controller.GetButtonDown(PlayerControllerButtonType.Ready))
			{
				DoPulseEffect();
			}
			else if(player.controller.GetButtonDown(PlayerControllerButtonType.Reset)) //toggle it off
			{
				DoPulseEffect();

				player.controller.SetPlayer(null);
				player.controller = GameManager.instance.unusedPlayerController;

				//reset input so it can't be re-added during this update
				//(more of a hack than an ideal way to do this, but I'm lazy)
				Input.ResetInputAxes(); 

				UpdateState();
			}
		}

		background.scaleX += (1.0f - background.scaleX) / 10.0f;
		background.scaleY += (1.0f - background.scaleY) / 10.0f;
	}
	
	void UpdateState()
	{
		if (player.controller == GameManager.instance.unusedPlayerController)
		{
			readyLabel.text = "UNUSED";
			readyLabel.color = RXColor.GetColorFromHex(0xAAAAAA);
		}
		else
		{
			readyLabel.text = player.controller.title;
			readyLabel.color = RXColor.GetColorFromHex(0xFFFFFF);
		}
		
		if(signalStateChange != null) signalStateChange();
	}
}
