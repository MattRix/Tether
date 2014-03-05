using UnityEngine;
using System.Collections.Generic;
using System;

public class PlayerSelectPage : TPage
{
	public FLabel allReadyLabel;

	public FLabel infoLabel;

	public FContainer panelHolder;

	public bool areAllPlayersReady = false;

	public FSliceButton startButton;

	public List<PlayerSelectPanel> panels = new List<PlayerSelectPanel>();

	public int selectedPanelIndex = 0;
	public FContainer selectedPanelBorder;

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

		if(GameManager.instance.shouldUseTeams)
		{
			FLabel teamLabel = new FLabel("Franchise", "TEAM MODE");
			AddChild(teamLabel);
			teamLabel.y = -332;
			teamLabel.scale = 0.5f;
			teamLabel.alpha = 0.0f;

			Go.to(teamLabel, 0.5f, new TweenConfig().floatProp("alpha", 0.7f).setDelay(0.7f));
		}

//		infoLabel = new FLabel("Franchise", "CLICK THE BOXES OR PRESS BACK/SELECT TO TOGGLE PLAYERS");
//		AddChild(infoLabel);
//		infoLabel.y = -332;
//		infoLabel.scale = 0.5f;
//		infoLabel.alpha = 0.0f;
//
//		Go.to(infoLabel, 0.5f, new TweenConfig().floatProp("alpha", 0.7f).setDelay(0.7f));

		FLabel smallLabel;
//
		smallLabel = new FLabel("Franchise", "F: FULLSCREEN");
		AddChild(smallLabel);
		smallLabel.x = -634;
		smallLabel.y = 336.0f;
		smallLabel.alignment = FLabelAlignment.Left;
		smallLabel.scale = 0.4f;
		smallLabel.alpha = 0.7f;

		smallLabel = new FLabel("Franchise", "T: TOGGLE TEAMS");
		AddChild(smallLabel);
		smallLabel.x = -634;
		smallLabel.y = 336.0f - 28.0f * 1;
		smallLabel.alignment = FLabelAlignment.Left;
		smallLabel.scale = 0.4f;
		smallLabel.alpha = 0.7f;
//
		smallLabel = new FLabel("Franchise", "M: MUTE");
		AddChild(smallLabel);
		smallLabel.x = -634;
		smallLabel.y = 336.0f - 28.0f * 2;
		smallLabel.alignment = FLabelAlignment.Left;
		smallLabel.scale = 0.4f;
		smallLabel.alpha = 0.7f;

//		smallLabel = new FLabel("Franchise", "R: RESET");
//		AddChild(smallLabel);
//		smallLabel.x = -634;
//		smallLabel.y = 336.0f - 28.0f * 3;
//		smallLabel.alignment = FLabelAlignment.Left;
//		smallLabel.scale = 0.4f;
//		smallLabel.alpha = 0.7f;

		FSliceButton teamButton = new FSliceButton(140, 36, "Popup_BG", "Popup_BG", Color.white, Color.white, "click1");
		AddChild(teamButton);
		teamButton.x = -Futile.screen.width/2 + 70;
		teamButton.y = Futile.screen.height/2 - 56;
		teamButton.sprite.alpha = 0.0f;
		//teamButton.AddLabelA("CubanoBig", "START!", 0.75f, 2f, Color.white);
		teamButton.SignalRelease += (b) =>
		{
			TMain.instance.SwapTeams();
		};

		FSliceButton fullButton = new FSliceButton(140, 36, "Popup_BG", "Popup_BG", Color.white, Color.white, "click1");
		AddChild(fullButton);
		fullButton.x = -Futile.screen.width/2 + 70;
		fullButton.y = Futile.screen.height/2 - 56 + 28.0f;
		fullButton.sprite.alpha = 0.0f;
		//teamButton.AddLabelA("CubanoBig", "START!", 0.75f, 2f, Color.white);
		fullButton.SignalRelease += (b) =>
		{
			TMain.instance.SwapFullScreen();
		};



		startButton = new FSliceButton(220, 80, "Popup_BG", "Popup_BG", Color.blue, Color.white, "click1");
		AddChild(startButton);
		startButton.x = 425.0f;
		startButton.y = 270;
		startButton.sprite.alpha = 0.3f;
		startButton.AddLabelA("CubanoBig", "START!", 0.75f, 2f, Color.white);
		startButton.SignalRelease += HandleStartButtonClick;

		startButton.alpha = 0.0f;


		float spreadX = 275;
		float spreadY = 115;

		AddChild(panelHolder = new FContainer());
		panelHolder.y = -80.0f;

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

		selectedPanelBorder = new FContainer(); 

		FSliceSprite selectBG = new FSliceSprite("Selection_BG", 540, 220, 16, 16, 16, 16);
 
		selectedPanelBorder.AddChild(selectBG);
		selectBG.alpha = 0.9f;
		selectBG.color = RXColor.GetColorFromHex(0xFF9966);

		AddChild(selectedPanelBorder);

		CreatePlayerPanel(0, -spreadX, spreadY);
		CreatePlayerPanel(1, spreadX, spreadY);
		CreatePlayerPanel(2, -spreadX, -spreadY);
		CreatePlayerPanel(3, spreadX, -spreadY);

		HandlePanelStateChange();

		UpdateSelectedPanel();

		ListenForUpdate (HandleUpdate);
	}

	void UpdateSelectedPanel()
	{
		PlayerSelectPanel panel = panels[selectedPanelIndex];
		selectedPanelBorder.x = panel.x;
		selectedPanelBorder.y = panel.y - 80;
	}

	void CreatePlayerPanel(int index, float createX, float createY)
	{
		PlayerSelectPanel panel = new PlayerSelectPanel(GameManager.instance.players [index]);
		panelHolder.AddChild(panel);
		panel.SetPosition(createX, createY);
		panel.signalStateChange = HandlePanelStateChange;

		panel.scale = 0.5f;
		panel.alpha = 0.0f;

		Go.to(panel, 0.5f, new TweenConfig().floatProp("scale", 1.0f).floatProp("alpha", 1.0f).setEaseType(EaseType.BackOut).setDelay((float)index*0.2f));

		panels.Add(panel);
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
			allReadyLabel.text = "PRESS START/ENTER TO BEGIN!";
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
		if(startButton.isEnabled)
		{
			startButton.scale = 1.0f + Mathf.Sin(Time.time * 10.0f) * 0.025f;
		}
		else
		{
			startButton.scale = 1.0f;
		}

		int oldSelectedIndex = selectedPanelIndex;

		List<Player> players = GameManager.instance.players;

		List<PlayerController> pcs = GameManager.instance.availablePlayerControllers;
		
		for (int c = 0; c<pcs.Count; c++)
		{
			PlayerController pc = pcs[c];

			pc.Update();

			if(pc.didJustPressUp)
			{
				if(selectedPanelIndex == 2) selectedPanelIndex = 0;
				if(selectedPanelIndex == 3) selectedPanelIndex = 1;
			}

			if(pc.didJustPressDown)
			{
				if(selectedPanelIndex == 0) selectedPanelIndex = 2;
				if(selectedPanelIndex == 1) selectedPanelIndex = 3;
			}

			if(pc.didJustPressLeft)
			{
				if(selectedPanelIndex == 1) selectedPanelIndex = 0;
				if(selectedPanelIndex == 3) selectedPanelIndex = 2;
			}

			if(pc.didJustPressRight)
			{
				if(selectedPanelIndex == 0) selectedPanelIndex = 1;
				if(selectedPanelIndex == 2) selectedPanelIndex = 3;
			}

			if(pc.GetButtonDown(PlayerControllerButtonType.Ready))
			{
				//DO THE CYCLE ON THE SELECTED PLAYER
				selectedPanelBorder.scaleX = 1.03f;
				selectedPanelBorder.scaleY = 1.05f;
				FSoundManager.PlaySound("click1",0.4f);
				panels[selectedPanelIndex].CycleController();
			}

			if(pc.didJustConnect == true)
			{
				FSoundManager.PlaySound("pickUpOrb",0.2f);
				pc.didJustConnect = false;

				for(int p = 0;p<players.Count;p++)
				{
					Player player = players[p];
					
					if(pc.CanBeUsed() && player.controller == GameManager.instance.unusedPlayerController)
					{
						panels[p].SetController(pc);
						break;
					}
				}
			}

			if(pc.didJustDisconnect == true)
			{
				FSoundManager.PlaySound("orbAppears", 0.2f);
				pc.didJustDisconnect = false;
			}
		}


		if(oldSelectedIndex != selectedPanelIndex)
		{
			FSoundManager.PlaySound("click4",0.2f);
			UpdateSelectedPanel();
		}

		if(areAllPlayersReady)
		{
			if(GameConfig.SHOULD_SKIP_CHAR_SELECT)
			{
				StartGame();
				return;
			}

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

			if(Input.GetKeyDown(KeyCode.Return))
			{
				StartGame();
				return;
			}

			startButton.alpha = 1.0f;
			startButton.isEnabled = true;
		}
		else
		{
			startButton.alpha = 0.0f; //hidden when disabled
			startButton.isEnabled = false;
		}

		selectedPanelBorder.scaleX += (1.0f - selectedPanelBorder.scaleX) / 10.0f;
		selectedPanelBorder.scaleY += (1.0f - selectedPanelBorder.scaleY) / 10.0f;
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
