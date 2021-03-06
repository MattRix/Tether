using UnityEngine;
using System.Collections.Generic;
using System;

public class World : FContainer
{
	public static World instance;

	public FPWorld root;

	public FContainer backgroundHolder;
	public FContainer chainHolder;
	public FContainer beastShadowHolder;
	public FContainer beastHolder;
	public FContainer effectHolder;
	public FContainer orbHolder;
	public FContainer uiHolder;

	public List<Beast> beasts = new List<Beast>();
	public List<Chain> chains = new List<Chain>();

	public List<Orb> orbs = new List<Orb>();
	public TRandomCollection orbColl;

	public List<Team> teams;

	public float timeUntilNextOrb = 0;

	public FStage uiStage;

	public bool isGameOver;

	public Team winningTeam;

	public FParticleSystem backParticles;
	public FParticleSystem glowParticles;

	public Walls walls;

	public FContainer pauseContainer;

	public List<TeamPanel> teamPanels = new List<TeamPanel>();

	public double spawnRateMultiplier = 1.0;
	
	public World()
	{
		instance = this;

		isGameOver = false;

		root = FPWorld.Create(64.0f);

		AddChild(backParticles = new FParticleSystem(150));

		AddChild(chainHolder = new FContainer());
		AddChild(beastShadowHolder = new FContainer());
		AddChild(beastHolder = new FContainer());
		AddChild(effectHolder = new FContainer());

		AddChild(glowParticles = new FParticleSystem(150));
		glowParticles.shader = FShader.Additive;

		AddChild(orbHolder = new FContainer());

		uiStage = new FStage("UIStage");
		Futile.AddStage(uiStage);

		uiStage.scale = Futile.stage.scale;

		uiStage.AddChild(uiHolder = new FContainer());

		teams = GameManager.instance.activeTeams;

		InitBeasts();

		InitOrbs();

		InitUI();

		spawnRateMultiplier = 1.0f;

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

		for(int r = 0; r<orbs.Count; r++)
		{
			orbs[r].Destroy();
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

			FSprite beastShadow = new FSprite("Evil-Eye_Shadow_1");
			beastShadowHolder.AddChild(beastShadow);

			beastShadow.SetPosition(beast.GetPos());
			beastShadow.rotation = beast.holder.rotation;
			beastShadow.alpha = 0.7f;
			beastShadow.scale = 1.0f;

			beast.shadow = beastShadow;
		}

		for(int t = 0; t<teams.Count; t++)
		{
			Team team = teams[t];

			team.SignalTeamChange += HandleSignalTeamChange;
		}

		int linkCount = 11;

		if (beasts.Count == 2)
		{
			linkCount = 11;
			chains.Add(new Chain(this, 11, beasts[0], beasts[1]));
		}
		else if (beasts.Count == 3)
		{
			chains.Add(new Chain(this, 11, beasts[0], beasts[1]));
			chains.Add(new Chain(this, 11, beasts[1], beasts[2]));
			chains.Add(new Chain(this, 11, beasts[2], beasts[0]));
		}
		else if (beasts.Count == 4)
		{
			linkCount = 13;
			//chains.Add(new Chain(this, 11, beasts[0], beasts[1]));
			//chains.Add(new Chain(this, 11, beasts[1], beasts[2]));
			//chains.Add(new Chain(this, 11, beasts[2], beasts[3]));
			//chains.Add(new Chain(this, 11, beasts[3], beasts[0]));

			chains.Add(new Chain(this, linkCount, beasts[0], beasts[2]));
			chains.Add(new Chain(this, linkCount, beasts[1], beasts[3]));

			Chain ca = chains[0];
			Chain cb = chains[1];

			int middleLink = (linkCount-1)/2;
			ChainLink linkA = ca.links[middleLink];
			ChainLink linkB = cb.links[middleLink];

			linkA.gameObject.transform.position = new Vector2(0,0).ToVector3InMeters();
			linkB.gameObject.transform.position = new Vector2(0,0).ToVector3InMeters();

			HingeJoint hinge = linkA.gameObject.AddComponent<HingeJoint>();

			hinge.connectedBody = linkB.rigidbody;

			JointSpring jspring = hinge.spring;
			
			jspring.spring = 0.1f;
			
			hinge.spring = jspring;
			
			hinge.axis = new Vector3(0.0f, 0.0f, 1.0f);
		}


		for(int b = 0; b<beasts.Count; b++)
		{
			beasts[b].holder.MoveToFront();
		}
	}

	void HandleSignalTeamChange ()
	{
		for(int t = 0; t<teams.Count; t++)
		{
			if(teams[t].score >= GameConfig.WINNING_SCORE)
			{
				this.winningTeam = teams[t];
				DoGameOver();
				break;
			}
		}
	}

	void DoGameOver()
	{
		if (isGameOver) return; //it's already over

		FSoundManager.PlaySound("win", 0.4f);
		FSoundManager.StopMusic();

		isGameOver = true;

		FContainer gameOverHolder = new FContainer();

		uiStage.AddChild(gameOverHolder);

		FSprite blackSprite = new FSprite("WhiteBox");
		blackSprite.color = Color.black;
		blackSprite.alpha = 0.5f;
		blackSprite.width = Futile.screen.width;
		blackSprite.height = Futile.screen.height;
		
		gameOverHolder.AddChild(blackSprite);

		FLabel titleLabel = new FLabel("CubanoBig", winningTeam.name + " WON!");
		titleLabel.color = winningTeam.color;
		titleLabel.y = 30.0f;
		gameOverHolder.AddChild(titleLabel);

		FLabel instructionLabel = new FLabel("CubanoBig", "PRESS START/ENTER TO PLAY AGAIN!");
		instructionLabel.color = Color.white;
		instructionLabel.y = -40.0f;
		instructionLabel.scale = 0.75f;
		gameOverHolder.AddChild(instructionLabel);

		instructionLabel = new FLabel("CubanoBig", "PRESS SELECT/BACK/R TO CHANGE PLAYERS");
		instructionLabel.color = Color.white;
		instructionLabel.alpha = 0.7f;
		instructionLabel.y = -150.0f;
		instructionLabel.scale = 0.5f;
		gameOverHolder.AddChild(instructionLabel);

		gameOverHolder.scale = 0.5f;

		Go.to(gameOverHolder, 0.5f, new TweenConfig().floatProp("scale", 1.0f).setEaseType(EaseType.BackOut));
	}

	void InitOrbs()
	{
		orbColl = new TRandomCollection();

		for (int t = 0; t<teams.Count; t++)
		{
			orbColl.AddItem(teams[t],100.0f);
		}

		timeUntilNextOrb = RXRandom.Range(3.0f,4.0f);
	}

	void InitUI()
	{
		float baseY = 340.0f;
		//float spacing = 32.0f;

		float barWidth = GameConfig.WINNING_SCORE * TeamPanel.WIDTH_PER_NIB;
		float paddingX = 8.0f;
		float fullWidth  = barWidth * teams.Count + paddingX * (teams.Count-1);

		for(int t = 0; t<teams.Count; t++)
		{
			//CreateTeamPanel(teams[t], new Vector2(0,baseY-spacing*t));
			float barX = -fullWidth/2 + t*(barWidth+paddingX) + barWidth/2;
			CreateTeamPanel(teams[t],new Vector2(barX,baseY));
		}

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

		FLabel changeLabel = new FLabel("Franchise", "(PRESS BACK/SELECT/R TO RESET)");
		changeLabel.alpha = 0.7f;
		changeLabel.scale = 0.5f;
		changeLabel.y = -36.0f;
		pauseContainer.AddChild(changeLabel);
	}

	void CreateTeamPanel(Team team, Vector2 pos)
	{
		TeamPanel panel = new TeamPanel(team);
		panel.SetPosition(pos);

		uiHolder.AddChild(panel);

		teamPanels.Add(panel);
	}

	void HandleUpdate()
	{
		spawnRateMultiplier += Time.deltaTime * 0.005;
		timeUntilNextOrb -= Time.deltaTime * (float)spawnRateMultiplier;

//		if(Time.frameCount % 120 == 0)
//		{
//			Debug.Log(spawnRateMultiplier);
//		}

		if (timeUntilNextOrb <= 0)
		{
			CreateOrb();
		}

		for (int b = 0; b<beasts.Count; b++)
		{
			if (isGameOver) //check for start being pressed to end the game
			{
				if (beasts [b].player.controller.GetButtonDown(PlayerControllerButtonType.Start))
				{
					RestartGame();
					break;
				}
			}
			else 
			{
				if (beasts [b].player.controller.GetButtonDown(PlayerControllerButtonType.Pause))
				{
					TogglePause();
				}
			}

			if (beasts [b].player.controller.GetButtonDown(PlayerControllerButtonType.Reset))
			{
				RestartAtCharSelect();
				break;
			}
		
		}

		if(Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
		{	
			if(isGameOver)
			{
				RestartGame();
			}
			else 
			{
				TogglePause();
			}
		}
		else if(Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.Backspace))
		{
			if(pauseContainer.container != null) //can only restart when paused
			{
				RestartAtCharSelect();
			}
		}

		bool isOneCloseToWinning = false;

		int winningScore = 0;

		for (int t = 0; t<teams.Count; t++)
		{
			if(teams[t].score == GameConfig.WINNING_SCORE-1)
			{
				isOneCloseToWinning = true;
			}

			winningScore = Mathf.Max(teams[t].score, winningScore);
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
			if(beasts[b].player.team.score == winningScore && winningScore != 0)
			{
				beasts[b].goldSprite.isVisible = true;
			}
			else 
			{
				beasts[b].goldSprite.isVisible = false;
			}
		}

		for (int t = 0; t<teamPanels.Count; t++)
		{
			if(teamPanels[t].team.score == winningScore && winningScore != 0)
			{
				teamPanels[t].goldBG.isVisible = true;
			}
			else 
			{
				teamPanels[t].goldBG.isVisible = false;
			}
		}
	}

	static void RestartGame()
	{
		GameConfig.SHOULD_SKIP_CHAR_SELECT = true;
		TMain.instance.GoToPage(TPageType.PagePlayerSelect);
	}

	static void RestartAtCharSelect()
	{
		GameConfig.SHOULD_SKIP_CHAR_SELECT = false;
		TMain.instance.GoToPage(TPageType.PagePlayerSelect);
	}
	
	void CreateOrb()
	{
		if (isGameOver) return;

		Team team = orbColl.GetRandomItem() as Team;

		List<Vector2> beastPoses = new List<Vector2>();

		for(int b = 0; b<beasts.Count; b++)
		{
			if(beasts[b].player.team == team)
			{
				beastPoses.Add(beasts[b].GetPos());
			}
		}

		int posCount = beastPoses.Count;

		Vector2 createPos = new Vector2();

		float closeDistance = 250.0f;
		closeDistance *= closeDistance; //for sqrMagnitude compare

		int failsafe = 0;
		while (true && failsafe++ < 100)
		{
			float createRadius = RXRandom.Range(0.0f, 300.0f);
			float angle = RXRandom.Range(0,RXMath.DOUBLE_PI);
			createPos.x = Mathf.Cos(angle) * createRadius * 1.5f; //elliptical
			createPos.y = Mathf.Sin(angle) * createRadius;

			bool wasTooClose = false;

			for(int b = 0; b<posCount; b++)
			{
				float sqrDist = (createPos - beastPoses[b]).sqrMagnitude;

				if(sqrDist <= closeDistance) //it's too close
				{
					wasTooClose = true;
					break;
				}
			}

			if(wasTooClose) 
			{
				closeDistance -= 1; //this will help it eventually reach a manageable range and prevent an infite loop
			}
			else //it's far enough away so we can create it
			{
				break;  
			}
		}

		Orb orb = Orb.Create(this);
		orb.Init(team, createPos);
		orbs.Add(orb);


		int thingCount = GameManager.instance.shouldUseTeams ? teams.Count : beasts.Count;


		if (thingCount == 2)
		{
			timeUntilNextOrb = RXRandom.Range(2.5f,4.5f);
		}
		else if (thingCount == 3)
		{
			timeUntilNextOrb = RXRandom.Range(2f,4f);
		}
		else if (thingCount == 4)
		{
			timeUntilNextOrb = RXRandom.Range(1.5f,3.5f);
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

	public Beast GetBeastForPlayer(Player player)
	{
		for(int b = 0; b<beasts.Count; b++)
		{
			if(beasts[b].player == player)
			{
				return beasts[b];
			}
		}

		return null;
	}

	public Beast GetBeastForTeam(Team team) //there could be multiple but we'll just get one
	{
		for(int b = 0; b<beasts.Count; b++)
		{
			if(beasts[b].player.team == team)
			{
				return beasts[b];
			}
		}
		
		return null;
	}
}


