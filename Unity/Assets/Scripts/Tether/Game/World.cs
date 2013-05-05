using UnityEngine;
using System.Collections.Generic;
using System;

public class World : FContainer
{
	public static World instance;

	public FPWorld root;

	public FContainer backgroundHolder;
	public FContainer entityHolder;
	public FContainer effectHolder;
	public FContainer uiHolder;

	public List<Beast> beasts = new List<Beast>();
	public List<Chain> chains = new List<Chain>();

	public List<Orb> orbs = new List<Orb>();
	public TRandomCollection orbColl;

	public float timeUntilNextOrb = 0;

	public FStage uiStage;

	public World()
	{
		instance = this;

		root = FPWorld.Create(64.0f);

		AddChild(backgroundHolder = new FContainer());
		AddChild(entityHolder = new FContainer());
		AddChild(effectHolder = new FContainer());

		uiStage = new FStage("UIStage");
		Futile.AddStage(uiStage);

		uiStage.AddChild(uiHolder = new FContainer());

		InitBeasts();

		InitOrbs();

		InitUI();

		ListenForUpdate(HandleUpdate);
	}

	public void Destroy()
	{
		for(int b = 0; b<beasts.Count; b++)
		{
			beasts[b].Destroy();
		}
		
		for(int c = 0; c<chains.Count; c++)
		{
			chains[c].Destroy();
		}

		Futile.RemoveStage(uiStage);
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
		}

		for(int b = 0; b<beasts.Count; b++)
		{
			int firstIndex = b;
			int secondIndex = (b+1)%beasts.Count;

			Chain chain = new Chain(this, beasts[firstIndex], beasts[secondIndex]);
			chains.Add(chain);

			if(beasts.Count == 2)
			{
				return;
			}
		}

		for(int b = 0; b<beasts.Count; b++)
		{
			beasts[b].holder.MoveToFront();
		}
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
		float spreadX = 100.0f;
		float spreadY = 100.0f;

		CreateBeastPanel(0, new Vector2(-spreadX,spreadY));
		CreateBeastPanel(1, new Vector2(spreadX,spreadY));
		CreateBeastPanel(2, new Vector2(-spreadX,-spreadY));
		CreateBeastPanel(3, new Vector2(spreadX,-spreadY));
	}

	void CreateBeastPanel(int index, Vector2 pos)
	{
		if (index > beasts.Count - 1) return;

		Beast beast = beasts [index];

		BeastPanel panel = new BeastPanel(beast);
		panel.SetPosition(pos);

		uiHolder.AddChild(panel);
	}

	void HandleUpdate()
	{
		timeUntilNextOrb -= Time.deltaTime;

		if (timeUntilNextOrb <= 0)
		{
			CreateOrb();
		}
	}
	
	void CreateOrb()
	{
		Beast beast = orbColl.GetRandomItem() as Beast;

		Vector2 beastPos = beast.GetPos();

		Vector2 createPos = new Vector2();

		float closeDistance = 200.0f;
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
				closeDistance -= 2; //this will help it eventually reach a manageable range and prevent an infite loop
			}
		}

		Orb orb = Orb.Create(this);
		orb.Init(beast.player, createPos);

		timeUntilNextOrb = RXRandom.Range(0.5f,4.0f);
	}


}


