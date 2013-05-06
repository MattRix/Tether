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

	public float timeUntilNextOrb = 0;

	public FStage uiStage;

	public bool isGameOver;

	public Player winningPlayer;

	public FParticleSystem backParticles;
	public FParticleSystem glowParticles;

	public Walls walls;

	public FContainer pauseContainer;

	public List<BeastPanel> beastPanels = new List<BeastPanel>();

	public List<Vector2> lavaPositions = new List<Vector2>();

	public List<FSprite> beastShadows = new List<FSprite>();

	public FParticleDefinition lavaPD;

	public World()
	{
		instance = this;

		isGameOver = false;

		root = FPWorld.Create(64.0f);

		AddChild(backParticles = new FParticleSystem(100));

		AddChild(chainHolder = new FContainer());
		AddChild(beastShadowHolder = new FContainer());
		AddChild(beastHolder = new FContainer());
		AddChild(effectHolder = new FContainer());

		AddChild(glowParticles = new FParticleSystem(100));
		glowParticles.shader = FShader.Additive;

		AddChild(orbHolder = new FContainer());



		uiStage = new FStage("UIStage");
		Futile.AddStage(uiStage);

		uiStage.AddChild(uiHolder = new FContainer());

		InitBeasts();

		InitOrbs();

		InitUI();

		InitLava();

		walls = new Walls(this);

		ListenForUpdate(HandleUpdate);

		Input.ResetInputAxes();
	}

	void InitLava()
	{
		lavaPD = new FParticleDefinition("Particles/Flame");
		
		lavaPD.startColor = new Color(1.0f,RXRandom.Range(0.0f,1.0f),0,0.6f);
		lavaPD.endColor = lavaPD.startColor.CloneWithNewAlpha(-0.5f);

		lavaPositions.Add(new Vector2(-623.9141f,278.9141f));
		lavaPositions.Add(new Vector2(-604.9141f,299.9141f));
		lavaPositions.Add(new Vector2(-574.9141f,321.9141f));
		lavaPositions.Add(new Vector2(-535.9141f,335.9141f));
		lavaPositions.Add(new Vector2(-540.9141f,342.9141f));
		lavaPositions.Add(new Vector2(-599.9141f,339.9141f));
		lavaPositions.Add(new Vector2(-608.9141f,339.9141f));
		lavaPositions.Add(new Vector2(-623.9141f,311.9141f));
		lavaPositions.Add(new Vector2(-630.9141f,342.9141f));
		lavaPositions.Add(new Vector2(-639.9141f,303.9141f));
		lavaPositions.Add(new Vector2(-450.9141f,342.9141f));
		lavaPositions.Add(new Vector2(-439.9141f,313.9141f));
		lavaPositions.Add(new Vector2(-422.9141f,300.9141f));
		lavaPositions.Add(new Vector2(-380.9141f,290.9141f));
		lavaPositions.Add(new Vector2(-346.9141f,292.9141f));
		lavaPositions.Add(new Vector2(-339.9141f,301.9141f));
		lavaPositions.Add(new Vector2(-353.9141f,339.9141f));
		lavaPositions.Add(new Vector2(-368.9141f,349.9141f));
		lavaPositions.Add(new Vector2(-394.9141f,354.9141f));
		lavaPositions.Add(new Vector2(-291.9141f,316.9141f));
		lavaPositions.Add(new Vector2(-266.7813f,309.8672f));
		lavaPositions.Add(new Vector2(-321.7813f,291.8672f));
		lavaPositions.Add(new Vector2(167.2188f,272.8672f));
		lavaPositions.Add(new Vector2(167.2188f,298.8672f));
		lavaPositions.Add(new Vector2(205.2188f,291.8672f));
		lavaPositions.Add(new Vector2(166.2188f,317.8672f));
		lavaPositions.Add(new Vector2(171.2188f,343.8672f));
		lavaPositions.Add(new Vector2(56.21875f,292.8672f));
		lavaPositions.Add(new Vector2(11.21875f,291.8672f));
		lavaPositions.Add(new Vector2(250.2188f,351.8672f));
		lavaPositions.Add(new Vector2(317.2188f,351.8672f));
		lavaPositions.Add(new Vector2(372.2188f,347.8672f));
		lavaPositions.Add(new Vector2(432.2188f,346.8672f));
		lavaPositions.Add(new Vector2(480.2188f,345.8672f));
		lavaPositions.Add(new Vector2(516.2188f,343.8672f));
		lavaPositions.Add(new Vector2(530.2188f,329.8672f));
		lavaPositions.Add(new Vector2(545.2188f,314.8672f));
		lavaPositions.Add(new Vector2(563.2188f,299.8672f));
		lavaPositions.Add(new Vector2(580.2188f,287.8672f));
		lavaPositions.Add(new Vector2(611.2188f,271.8672f));
		lavaPositions.Add(new Vector2(616.2188f,264.8672f));
		lavaPositions.Add(new Vector2(617.2188f,292.8672f));
		lavaPositions.Add(new Vector2(612.2188f,311.8672f));
		lavaPositions.Add(new Vector2(587.2188f,332.8672f));
		lavaPositions.Add(new Vector2(577.2188f,334.8672f));
		lavaPositions.Add(new Vector2(606.2188f,338.8672f));
		lavaPositions.Add(new Vector2(512.2188f,336.8672f));
		lavaPositions.Add(new Vector2(627.2188f,39.86719f));
		lavaPositions.Add(new Vector2(610.2188f,20.86719f));
		lavaPositions.Add(new Vector2(585.2188f,-1.132813f));
		lavaPositions.Add(new Vector2(568.2188f,-5.132813f));
		lavaPositions.Add(new Vector2(542.2188f,-13.13281f));
		lavaPositions.Add(new Vector2(542.2188f,-17.13281f));
		lavaPositions.Add(new Vector2(-301.8711f,308.8242f));
		lavaPositions.Add(new Vector2(-344.8711f,297.8242f));
		lavaPositions.Add(new Vector2(-367.8711f,288.8242f));
		lavaPositions.Add(new Vector2(-382.8711f,290.8242f));
		lavaPositions.Add(new Vector2(-403.8711f,294.8242f));
		lavaPositions.Add(new Vector2(-424.8711f,303.8242f));
		lavaPositions.Add(new Vector2(572.1289f,-7.175781f));
		lavaPositions.Add(new Vector2(548.1289f,-17.17578f));
		lavaPositions.Add(new Vector2(573.1289f,-28.17578f));
		lavaPositions.Add(new Vector2(601.1289f,-32.17578f));
		lavaPositions.Add(new Vector2(600.1289f,-28.17578f));
		lavaPositions.Add(new Vector2(581.1289f,-55.17578f));
		lavaPositions.Add(new Vector2(584.1289f,-44.17578f));
		lavaPositions.Add(new Vector2(557.1289f,-21.17578f));
		lavaPositions.Add(new Vector2(537.1289f,-19.17578f));
		lavaPositions.Add(new Vector2(605.1289f,-56.17578f));
		lavaPositions.Add(new Vector2(627.1289f,-45.17578f));
		lavaPositions.Add(new Vector2(627.1289f,-56.17578f));
		lavaPositions.Add(new Vector2(621.1289f,-127.1758f));
		lavaPositions.Add(new Vector2(614.1289f,-130.1758f));
		lavaPositions.Add(new Vector2(612.1289f,-75.17578f));
		lavaPositions.Add(new Vector2(606.1289f,-83.17578f));
		lavaPositions.Add(new Vector2(598.1289f,-113.1758f));
		lavaPositions.Add(new Vector2(577.1289f,-120.1758f));
		lavaPositions.Add(new Vector2(567.1289f,-125.1758f));
		lavaPositions.Add(new Vector2(608.1289f,-147.1758f));
		lavaPositions.Add(new Vector2(615.1289f,-159.1758f));
		lavaPositions.Add(new Vector2(617.1289f,-164.1758f));
		lavaPositions.Add(new Vector2(629.1289f,-148.1758f));
		lavaPositions.Add(new Vector2(630.1289f,-145.1758f));
		lavaPositions.Add(new Vector2(630.1289f,-184.1758f));
		lavaPositions.Add(new Vector2(630.1289f,-193.1758f));
		lavaPositions.Add(new Vector2(630.1289f,-205.1758f));
		lavaPositions.Add(new Vector2(629.1289f,-250.1758f));
		lavaPositions.Add(new Vector2(629.1289f,-270.1758f));
		lavaPositions.Add(new Vector2(625.1289f,-305.1758f));
		lavaPositions.Add(new Vector2(622.1289f,-317.1758f));
		lavaPositions.Add(new Vector2(612.1289f,-288.1758f));
		lavaPositions.Add(new Vector2(605.1289f,-276.1758f));
		lavaPositions.Add(new Vector2(598.1289f,-300.1758f));
		lavaPositions.Add(new Vector2(593.1289f,-308.1758f));
		lavaPositions.Add(new Vector2(565.1289f,-338.1758f));
		lavaPositions.Add(new Vector2(560.1289f,-323.1758f));
		lavaPositions.Add(new Vector2(560.1289f,-320.1758f));
		lavaPositions.Add(new Vector2(555.1289f,-338.1758f));
		lavaPositions.Add(new Vector2(540.1289f,-356.1758f));
		lavaPositions.Add(new Vector2(529.1289f,-359.1758f));
		lavaPositions.Add(new Vector2(520.1289f,-354.1758f));
		lavaPositions.Add(new Vector2(488.1289f,-352.1758f));
		lavaPositions.Add(new Vector2(488.1289f,-352.1758f));
		lavaPositions.Add(new Vector2(-335.8711f,-344.1758f));
		lavaPositions.Add(new Vector2(-373.9141f,-328.1328f));
		lavaPositions.Add(new Vector2(-412.9141f,-318.1328f));
		lavaPositions.Add(new Vector2(-418.9141f,-321.1328f));
		lavaPositions.Add(new Vector2(-456.9141f,-331.1328f));
		lavaPositions.Add(new Vector2(-471.9141f,-328.1328f));
		lavaPositions.Add(new Vector2(-495.9141f,-321.1328f));
		lavaPositions.Add(new Vector2(-515.9141f,-311.1328f));
		lavaPositions.Add(new Vector2(-540.9141f,-287.1328f));
		lavaPositions.Add(new Vector2(-555.9141f,-282.1328f));
		lavaPositions.Add(new Vector2(-568.9141f,-265.1328f));
		lavaPositions.Add(new Vector2(-577.9141f,-261.1328f));
		lavaPositions.Add(new Vector2(-628.9141f,-255.1328f));
		lavaPositions.Add(new Vector2(-616.9141f,-278.1328f));
		lavaPositions.Add(new Vector2(-600.9141f,-298.1328f));
		lavaPositions.Add(new Vector2(-580.9141f,-315.1328f));
		lavaPositions.Add(new Vector2(-561.9141f,-327.1328f));
		lavaPositions.Add(new Vector2(-549.9141f,-341.1328f));
		lavaPositions.Add(new Vector2(-543.9141f,-350.1328f));
		lavaPositions.Add(new Vector2(-561.9141f,-345.1328f));
		lavaPositions.Add(new Vector2(-582.9141f,-343.1328f));
		lavaPositions.Add(new Vector2(-594.9141f,-323.1328f));
		lavaPositions.Add(new Vector2(-615.9141f,-306.1328f));
		lavaPositions.Add(new Vector2(-625.9141f,-297.1328f));
		lavaPositions.Add(new Vector2(-624.0938f,-318.1758f));
		lavaPositions.Add(new Vector2(-609.0938f,-334.1758f));
		lavaPositions.Add(new Vector2(-601.0938f,-350.1758f));
		lavaPositions.Add(new Vector2(-601.0938f,-352.1758f));
		lavaPositions.Add(new Vector2(-615.0938f,-352.1758f));
		lavaPositions.Add(new Vector2(-625.0938f,-347.1758f));
		lavaPositions.Add(new Vector2(-626.0938f,-347.1758f));
		lavaPositions.Add(new Vector2(-628.0938f,-121.1758f));
		lavaPositions.Add(new Vector2(-624.0938f,-112.1758f));
		lavaPositions.Add(new Vector2(-625.0938f,-96.17578f));
		lavaPositions.Add(new Vector2(-626.0938f,-87.17578f));
		lavaPositions.Add(new Vector2(-588.0938f,12.82422f));
		lavaPositions.Add(new Vector2(-601.0938f,20.82422f));
		lavaPositions.Add(new Vector2(-614.0938f,31.82422f));
		lavaPositions.Add(new Vector2(-618.0938f,38.82422f));
		lavaPositions.Add(new Vector2(-625.0938f,47.82422f));
		lavaPositions.Add(new Vector2(-627.0938f,52.82422f));
		lavaPositions.Add(new Vector2(-626.0938f,74.82422f));
		lavaPositions.Add(new Vector2(-615.0938f,84.82422f));
		lavaPositions.Add(new Vector2(-608.0938f,89.82422f));
		lavaPositions.Add(new Vector2(-589.0938f,97.82422f));
		lavaPositions.Add(new Vector2(-593.0938f,107.8242f));
		lavaPositions.Add(new Vector2(-624.0938f,121.8242f));
		lavaPositions.Add(new Vector2(-627.0938f,134.8242f));
		lavaPositions.Add(new Vector2(-627.0938f,138.8242f));
		lavaPositions.Add(new Vector2(-605.0938f,301.8242f));
		lavaPositions.Add(new Vector2(-588.0938f,317.8242f));
		lavaPositions.Add(new Vector2(-364.0938f,295.8242f));
		lavaPositions.Add(new Vector2(176.9063f,282.8242f));
		lavaPositions.Add(new Vector2(574.9063f,-25.17578f));
		lavaPositions.Add(new Vector2(-319.0938f,-345.1758f));
		lavaPositions.Add(new Vector2(-411.0938f,-321.1758f));

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

			FSprite beastShadow = new FSprite("Evil-Eye_Shadow_1");
			beastShadowHolder.AddChild(beastShadow);

			beastShadow.SetPosition(beast.GetPos());
			beastShadow.rotation = beast.holder.rotation;
			beastShadow.alpha = 0.7f;
			beastShadow.scale = 1.0f;

			beastShadows.Add(beastShadow);
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

			Vector2 pos = beasts[b].GetPos();
			pos.y -= 3.0f;


			beastShadows[b].SetPosition(pos);
			beastShadows[b].rotation = beasts[b].holder.rotation;

		}

		Vector2 lavaPos = lavaPositions[RXRandom.Range(0,lavaPositions.Count-1)];
		
		lavaPD.x = lavaPos.x + RXRandom.Range(-3.0f, 3.0f);
		lavaPD.y = lavaPos.y + RXRandom.Range(-3.0f, 3.0f);

		lavaPD.speedX = RXRandom.Range(-10.0f,10.0f);
		lavaPD.speedY = RXRandom.Range(-5.0f,20.0f);

		lavaPD.startScale = RXRandom.Range(0.2f,0.3f);
		lavaPD.endScale = RXRandom.Range(0.4f,0.9f);
		
		lavaPD.lifetime = RXRandom.Range(2.0f, 3.0f);
		
		glowParticles.AddParticle(lavaPD);
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


