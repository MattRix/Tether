using UnityEngine;
using System.Collections.Generic;
using System;

public class World : FContainer
{
	public static World instance;

	public FPWorld root;

	public FContainer backgroundHolder;
	public FContainer tentacleHolder;
	public FContainer entityHolder;
	public FContainer effectHolder;
	public FContainer additiveEffectHolder;
	public FContainer orbHolder;
	public FContainer uiHolder;

	public List<Beast> beasts = new List<Beast>();
	public List<Chain> chains = new List<Chain>();

	public List<Orb> orbs = new List<Orb>();
	public TRandomCollection orbColl;

	public float timeUntilNextOrb = 0;

	public FStage uiStage;

	public bool isGameOver;

	public Player winningPlayer;

	public FParticleSystem backParticles;
	public FParticleSystem glowParticles;

	public Walls walls;

	public FContainer pauseContainer;

	public List<BeastPanel> beastPanels = new List<BeastPanel>();

	public World()
	{
		instance = this;

		isGameOver = false;

		root = FPWorld.Create(64.0f);

		AddChild(tentacleHolder = new FContainer());
		AddChild(entityHolder = new FContainer());
		AddChild(effectHolder = new FContainer());
		AddChild(additiveEffectHolder = new FContainer());
		AddChild(orbHolder = new FContainer());

		tentacleHolder.AddChild(backParticles = new FParticleSystem(100));
		//backParticles.shader = FShader.Additive;

		additiveEffectHolder.AddChild(glowParticles = new FParticleSystem(100));
		glowParticles.shader = FShader.Additive;

		uiStage = new FStage("UIStage");
		Futile.AddStage(uiStage);

		uiStage.AddChild(uiHolder = new FContainer());

		InitBeasts();

		InitOrbs();

		InitUI();

		walls = new Walls(this);

		ListenForUpdate(HandleUpdate);

		Input.ResetInputAxes();
	}

	public void Destroy()
	{
		walls.Destroy();

		for(int b = 0; b<beasts.Count; b++)
		{
			beasts[b].Destroy();
		}
		
		for(int c = 0; c<chains.Count; c++)
		{
			chains[c].Destroy();
		}

		GameManager.instance.Reset();

		Futile.RemoveStage(uiStage);

		UnityEngine.Object.Destroy(root.gameObject);

		Input.ResetInputAxes();

		Time.timeScale = 1.0f;

		//GamepadManager.Init();
		//GameManager.Init();
	}


	void InitBeasts()
	{
		List<Player> players = GameManager.instance.activePlayers;
		float radiansPerPlayer = RXMath.DOUBLE_PI / (float)players.Count;
		float startRadius = 150.0f;

		for (int p = 0; p < players.Count; p++)
		{
			Vector2 startPos = new Vector2();
			startPos.x = Mathf.Cos(p * radiansPerPlayer) * startRadius;
			startPos.y = Mathf.Sin(p * radiansPerPlayer) * startRadius;
			Beast beast = Beast.Create(this);
			beast.Init(players[p], startPos);
			beasts.Add(beast);

			beast.player.SignalPlayerChange += HandleSignalPlayerChange;
		}

		if (beasts.Count == 2)
		{
			chains.Add(new Chain(this, beasts[0], beasts[1]));
		}
		else if (beasts.Count == 3)
		{
			chains.Add(new Chain(this, beasts[0], beasts[1]));
			chains.Add(new Chain(this, beasts[1], beasts[2]));
			chains.Add(new Chain(this, beasts[2], beasts[0]));
		}
		else if (beasts.Count == 4)
		{
			chains.Add(new Chain(this, beasts[0], beasts[1]));
			chains.Add(new Chain(this, beasts[1], beasts[2]));
			chains.Add(new Chain(this, beasts[2], beasts[3]));
			chains.Add(new Chain(this, beasts[3], beasts[0]));
		}


		for(int b = 0; b<beasts.Count; b++)
		{
			beasts[b].holder.MoveToFront();
		}
	}

	void HandleSignalPlayerChange ()
	{
		for(int b = 0; b<beasts.Count; b++)
		{
			if(beasts[b].player.score >= GameConfig.WIN_SCORE)
			{
				this.winningPlayer = beasts[b].player;
				DoGameOver();
				break;
			}
		}
	}

	void DoGameOver()
	{
		if (isGameOver) return; //it's already over

		FSoundManager.PlaySound("win");
		FSoundManager.StopMusic();

		isGameOver = true;

		FContainer gameOverHolder = new FContainer();

		uiStage.AddChild(gameOverHolder);

		FLabel titleLabel = new FLabel("CubanoBig", winningPlayer.name + " WON!");
		titleLabel.color = winningPlayer.color;
		titleLabel.y = 30.0f;
		gameOverHolder.AddChild(titleLabel);

		FLabel instructionLabel = new FLabel("CubanoBig", "PRESS START TO PLAY AGAIN!");
		instructionLabel.color = Color.white;
		instructionLabel.y = -40.0f;
		instructionLabel.scale = 0.75f;
		gameOverHolder.AddChild(instructionLabel);

		gameOverHolder.scale = 0.5f;

		Go.to(gameOverHolder, 0.5f, new TweenConfig().floatProp("scale", 1.0f).setEaseType(EaseType.BackOut));
	}

	void InitOrbs()
	{
		orbColl = new TRandomCollection();

		for (int b = 0; b<beasts.Count; b++)
		{
			orbColl.AddItem(beasts[b],100.0f);
		}

		timeUntilNextOrb = RXRandom.Range(3.0f,4.0f);
	}

	void InitUI()
	{
		float baseY = Futile.screen.halfHeight-20.0f;
		float spacing = 32.0f;

		CreateBeastPanel(0, new Vector2(0,baseY-spacing*0));
		CreateBeastPanel(1, new Vector2(0,baseY-spacing*1));
		CreateBeastPanel(2, new Vector2(0,baseY-spacing*2));
		CreateBeastPanel(3, new Vector2(0,baseY-spacing*3));

		pauseContainer = new FContainer();

		FSprite blackSprite = new FSprite("WhiteBox");
		blackSprite.color = Color.black;
		blackSprite.alpha = 0.5f;
		blackSprite.width = Futile.screen.width;
		blackSprite.height = Futile.screen.height;

		pauseContainer.AddChild(blackSprite);

		FLabel pauseLabel = new FLabel("Franchise", "PAUSED");
		pauseLabel.y = 16.0f;
		pauseContainer.AddChild(pauseLabel);

		FLabel changeLabel = new FLabel("Franchise", "(PRESS SELECT TO CHANGE PLAYERS)");
		changeLabel.alpha = 0.7f;
		changeLabel.scale = 0.5f;
		changeLabel.y = -36.0f;
		pauseContainer.AddChild(changeLabel);
	}

	void CreateBeastPanel(int index, Vector2 pos)
	{
		if (index > beasts.Count - 1) return;

		Beast beast = beasts [index];

		BeastPanel panel = new BeastPanel(beast);
		panel.SetPosition(pos);

		uiHolder.AddChild(panel);

		beastPanels.Add(panel);
	}

	void HandleUpdate()
	{
		timeUntilNextOrb -= Time.deltaTime;

		if (timeUntilNextOrb <= 0)
		{
			CreateOrb();
		}

		for (int b = 0; b<beasts.Count; b++)
		{
			if (isGameOver) //check for start being pressed to end the game
			{
				if (beasts [b].player.gamepad.GetButtonDown(PS3ButtonType.Start))
				{
					GameConfig.SHOULD_SKIP_CHAR_SELECT = true;
					TMain.instance.GoToPage(TPageType.PagePlayerSelect);
					break;
				}
			}
			else 
			{
				if (beasts [b].player.gamepad.GetButtonDown(PS3ButtonType.Start))
				{
					TogglePause();
				}
			}

			//if (beasts [b].player.gamepad.GetButton(PS3ButtonType.Start) && beasts [b].player.gamepad.GetButton(PS3ButtonType.Select))
			if (beasts [b].player.gamepad.GetButton(PS3ButtonType.Select))
			{
				GameConfig.SHOULD_SKIP_CHAR_SELECT = false;
				TMain.instance.GoToPage(TPageType.PagePlayerSelect);
				break;
			}
		
		}


		bool isOneCloseToWinning = false;

		int winningScore = 0;

		for (int b = 0; b<beasts.Count; b++)
		{
			if(beasts[b].player.score == GameConfig.WIN_SCORE-1)
			{
				isOneCloseToWinning = true;
			}

			winningScore = Mathf.Max(beasts[b].player.score, winningScore);
		}

		if (!isGameOver)
		{
			if (isOneCloseToWinning)
			{
				if (Time.frameCount % 75 == 0)
				{
					FSoundManager.PlaySound("alarm", 0.6f);
				}
			}
		}

		for (int b = 0; b<beasts.Count; b++)
		{
			if(beasts[b].player.score == winningScore && winningScore != 0)
			{
				beasts[b].goldSprite.isVisible = true;
				beastPanels[b].goldBG.isVisible = true;
			}
			else 
			{
				beasts[b].goldSprite.isVisible = false;
				beastPanels[b].goldBG.isVisible = false;
			}
		}

	}
	
	void CreateOrb()
	{
		if (isGameOver) return;

		Beast beast = orbColl.GetRandomItem() as Beast;

		Vector2 beastPos = beast.GetPos();

		Vector2 createPos = new Vector2();

		float closeDistance = 250.0f;
		closeDistance *= closeDistance; //for sqrMagnitude compare

		while (true)
		{
			float createRadius = RXRandom.Range(0.0f, 300.0f);
			float angle = RXRandom.Range(0,RXMath.DOUBLE_PI);
			createPos.x = Mathf.Cos(angle) * createRadius * 1.5f; //elliptical
			createPos.y = Mathf.Sin(angle) * createRadius;

			float sqrDist = (createPos - beastPos).sqrMagnitude;

			if(sqrDist >= closeDistance) //it's far enough away so we can create it
			{
				break;
			}
			else 
			{
				closeDistance -= 1; //this will help it eventually reach a manageable range and prevent an infite loop
			}
		}

		Orb orb = Orb.Create(this);
		orb.Init(beast.player, createPos);

		if (beasts.Count == 2)
		{
			timeUntilNextOrb = RXRandom.Range(0.5f,3.5f);
		}
		else if (beasts.Count == 3)
		{
			timeUntilNextOrb = RXRandom.Range(0.5f,3.0f);
		}
		else if (beasts.Count == 4)
		{
			timeUntilNextOrb = RXRandom.Range(0.5f,2.5f);
		}

	}


	void TogglePause()
	{
		if (Time.timeScale <= 0.1f)
		{
			Time.timeScale = 1.0f;
			pauseContainer.RemoveFromContainer();
		}
		else
		{
			Time.timeScale = 0.001f;
			AddChild(pauseContainer);
		}
	}
}


