using UnityEngine;
using System.Collections.Generic;
using System;

public class PlayerSelectPage : TPage
{
	FLabel allReadyLabel;

	public FContainer panelHolder;

	public bool areAllPlayersReady = false;

	public FSliceButton startButton;

	public PlayerSelectPage()
	{

	}
	
	override public void Start()
	{
		FSoundManager.StopMusic();

		FSprite logo = new FSprite("TetherLogo");
		logo.y = 270.0f;
		AddChild(logo);

//		FLabel titleLabel = new FLabel("CubanoBig", "TETHER");
//		AddChild(titleLabel);
//		titleLabel.y = 300;

		allReadyLabel = new FLabel("CubanoBig", "");
		AddChild(allReadyLabel);
		allReadyLabel.y = 190;
		allReadyLabel.scale = 0.75f;

		startButton = new FSliceButton(230, 110, "Popup_Bg", "Popup_Bg", Color.blue, Color.white, "click1");
		AddChild(startButton);
		startButton.x = TMain.instance.background.sprite.width/2.0f - 215.0f;
		startButton.y = 240;
		startButton.sprite.alpha = 0.6f;
		startButton.AddLabelA("CubanoBig", "START!", 0.75f, 2f, Color.white);
		startButton.SignalRelease += HandleStartButtonClick;

		float spreadX = 280;
		float spreadY = 120;

		AddChild(panelHolder = new FContainer());
		panelHolder.y = -90.0f;

		List<Player> players = GameManager.instance.players;

		for(int p = 0; p<players.Count; p++)
		{
			Player player = players[p];

			if(player.controller != GameManager.instance.unusedPlayerController && player.controller.CanBeUsed())
			{
				AIPlayerController aiController = player.controller as AIPlayerController;

				if(aiController != null)
				{
					player.controller.SetPlayer(null);
					player.controller = aiController.symbolic;
				}
			}
		}

		CreatePlayerPanel(0, -spreadX, spreadY);
		CreatePlayerPanel(1, spreadX, spreadY);
		CreatePlayerPanel(2, -spreadX, -spreadY);
		CreatePlayerPanel(3, spreadX, -spreadY);

		HandlePanelStateChange();

		ListenForUpdate (HandleUpdate);
	}

	void CreatePlayerPanel(int index, float createX, float createY)
	{
		PlayerSelectPanel panel = new PlayerSelectPanel(GameManager.instance.players [index]);
		panelHolder.AddChild(panel);
		panel.SetPosition(createX, createY);
		panel.signalStateChange = HandlePanelStateChange;

		panel.scale = 0.0f;

		Go.to(panel, 0.5f, new TweenConfig().floatProp("scale", 1.0f).setEaseType(EaseType.BackOut).setDelay((float)index*0.2f));
	}

	void HandlePanelStateChange()
	{
		List<Player> players = GameManager.instance.players;

		int readyPlayersCount = 0;

		for(int p = 0; p<players.Count; p++)
		{
			Player player = players[p];

			if(player.controller != GameManager.instance.unusedPlayerController && player.controller.CanBeUsed())
			{
				readyPlayersCount++;
			}

		}

		if (readyPlayersCount >= 2)
		{
			allReadyLabel.text = "PRESS START TO BEGIN!";
			allReadyLabel.color = RXColor.GetColorFromHex(0xEECCFF);
			areAllPlayersReady = true;
        }
		else
		{
			allReadyLabel.text = "WAITING FOR PLAYERS...";
			allReadyLabel.color = RXColor.GetColorFromHex(0xFF9933);
			areAllPlayersReady = false;
		}
	}
	
	protected void HandleUpdate ()
	{
		if(areAllPlayersReady)
		{
			if(GameConfig.SHOULD_SKIP_CHAR_SELECT)
			{
				StartGame();
				return;
			}
//
			List<Player> players = GameManager.instance.players;
        
			for(int p = 0;p<players.Count;p++)
			{
				Player player = players[p];
			
				if(player.controller.CanBeUsed())
				{
					if(player.controller.GetButtonDown(PlayerControllerButtonType.Start))
					{
						StartGame();
						return;
					}
				}
			}

			if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
			{
				StartGame();
				return;
			}

			startButton.alpha = 1.0f;
			startButton.isEnabled = true;
		}
		else
		{
			startButton.alpha = 0.5f;
			startButton.isEnabled = false;
		}
    }

	void HandleStartButtonClick (FSliceButton button)
	{
		if(areAllPlayersReady)
		{
			StartGame();
		}
		else
		{

		}
	}

	void StartGame()
	{
		List<Player> activePlayers = new List<Player>();

		List<Player> players = GameManager.instance.players;
		
		for (int p = 0; p<players.Count; p++)
		{
			Player player = players [p];
			
			if(player.controller != GameManager.instance.unusedPlayerController && player.controller.CanBeUsed())
			{
				activePlayers.Add(player);

				AISymbolicPlayerController aiController = player.controller as AISymbolicPlayerController;

				if(aiController != null)
				{
					player.controller = aiController.CreateActualController();
					player.controller.SetPlayer(player);
				}
			}
		}

		GameManager.instance.SetRoundData(activePlayers);
		TMain.instance.GoToPage(TPageType.PageInGame);
	}
}