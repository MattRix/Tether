using UnityEngine;
using System.Collections.Generic;
using System;

public class PlayerSelectPage : TPage
{
	FLabel allReadyLabel;

	public FContainer panelHolder;

	public bool areAllPlayersReady = false;

	public PlayerSelectPage()
	{

	}
	
	override public void Start()
	{
		FLabel titleLabel = new FLabel("CubanoBig", "TETHER");
		AddChild(titleLabel);
		titleLabel.y = 270;

		allReadyLabel = new FLabel("CubanoBig", "");
		AddChild(allReadyLabel);
		allReadyLabel.y = 215;
		allReadyLabel.scale = 0.75f;

		float spreadX = 200;
		float spreadY = 120;

		AddChild(panelHolder = new FContainer());
		panelHolder.y = -60.0f;

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

			if(player.isConnected && player.isReady)
			{
				readyPlayersCount ++;
			}
		}

		if (readyPlayersCount >= 2)
		{
			allReadyLabel.text = "PRESS START TO BEGIN!";
			allReadyLabel.color = RXColor.GetColorFromHex(0x66FF66);
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
		if (areAllPlayersReady)
		{
			List<Player> players = GameManager.instance.players;
        
			for (int p = 0; p<players.Count; p++)
			{
				Player player = players [p];
			
				if (player.isConnected && player.isReady)
				{
					if (player.gamepad.GetButtonDown(PS3ButtonType.Start))
					{
						StartGame();
						return;
					}
				}
			}
		}
    }

	void StartGame()
	{
		List<Player> activePlayers = new List<Player>();

		List<Player> players = GameManager.instance.players;
		
		for (int p = 0; p<players.Count; p++)
		{
			Player player = players [p];
			
			if (player.isConnected && player.isReady)
			{
				activePlayers.Add(player);
			}
		}

		GameManager.instance.SetRoundData(activePlayers);
		TMain.instance.GoToPage(TPageType.PageInGame);
	}
}

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

		AddChild(background = new FSliceSprite("Popup_Bg",350,200,16,16,16,16));
		background.color = player.color;

		AddChild(nameLabel = new FLabel("CubanoBig", "PLAYER " + player.numString));
		nameLabel.y = 40.0f;

		AddChild(readyLabel = new FLabel("CubanoBig", ""));
		readyLabel.y = -40.0f;
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
				readyLabel.color = RXColor.GetColorFromHex(0x66FF66);
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
