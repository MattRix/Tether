using UnityEngine;
using System.Collections.Generic;
using System;

public class Chain
{
	public static int LINK_COUNT = 11;

	public World world;

	public Beast beastA;
	public Beast beastB;

	public List<ChainLink>links = new List<ChainLink>();
	
	public Chain(World world, Beast beastA, Beast beastB)
	{
		this.world = world;

		this.beastA = beastA;
		this.beastB = beastB;

		Vector2 startPos = beastA.GetPos();
		Vector2 endPos = beastB.GetPos();

		//Vector2 delta = (endPos - startPos) / (float)LINK_COUNT;

		ChainLink previousLink = null;

		for (int c = 0; c<LINK_COUNT; c++)
		{
			ChainLink link;

			bool isLastLink = (c == LINK_COUNT-1);

			if(isLastLink)
			{
				link = ChainLink.Create(world, beastB);
			}
			else 
			{
				link = ChainLink.Create(world, null);
			}

			Vector2 linkPos = new Vector2(startPos.x + ChainLink.LENGTH * (c), startPos.y);

			link.Init(this, previousLink, linkPos, isLastLink);
			links.Add(link);
			previousLink = link;
		}

		beastA.transform.position = new Vector3(startPos.x * FPhysics.POINTS_TO_METERS, startPos.y * FPhysics.POINTS_TO_METERS);
		beastB.transform.position = new Vector3(endPos.x * FPhysics.POINTS_TO_METERS, endPos.y * FPhysics.POINTS_TO_METERS);

	}

	public void Destroy()
	{
		foreach (ChainLink link in links)
		{
			link.Destroy();
		}
	}
}

public class ChainLink : MonoBehaviour
{
	public static float LENGTH = 25.0f;

	public static ChainLink Create(World world, Beast endBeast)
	{
		GameObject linkGO;

		if(endBeast == null)
		{
			linkGO = new GameObject("ChainLink");
		}
		else 
		{
			linkGO = endBeast.gameObject;
		}

		ChainLink link = linkGO.AddComponent<ChainLink>();
		link.world = world;

		return link;
	}
	
	public World world;

	public Chain chain;
	public ChainLink previousLink;
	
	public FSprite sprite;
	public FPNodeLink nodeLink;

	public HingeJoint hinge;

	public bool isLastLink;
	
	public void Init(Chain chain, ChainLink previousLink, Vector2 startPos, bool isLastLink)
	{
		this.chain = chain;
		this.previousLink = previousLink;
		this.isLastLink = isLastLink;

		gameObject.transform.position = new Vector3(startPos.x * FPhysics.POINTS_TO_METERS, startPos.y * FPhysics.POINTS_TO_METERS, 0);
		gameObject.transform.parent = world.root.transform;
	
		if (this.previousLink != null)
		{
			sprite = new FSprite("ChainLink");
			world.entityHolder.AddChild(sprite);

			sprite.ListenForUpdate(HandleUpdate);
		}

		InitPhysics();
	}
	
	public void Destroy()
	{
		UnityEngine.Object.Destroy(gameObject);
		
		if(sprite != null) sprite.RemoveFromContainer();
	}
	
	void InitPhysics()
	{
		if (!isLastLink)
		{
			Rigidbody rb = gameObject.AddComponent<Rigidbody>();
			rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
			rb.angularDrag = 1.0f;
			rb.drag = 0.8f;
			rb.mass = 1.0f;
		}

		hinge = gameObject.AddComponent<HingeJoint>();

		if (previousLink != null)
		{
			hinge.connectedBody = previousLink.rigidbody;
		}
		else
		{
			hinge.connectedBody = chain.beastA.rigidbody;
		}

		JointSpring jspring = hinge.spring;

		jspring.spring = 0.1f;

		hinge.spring = jspring;

		hinge.axis = new Vector3(0.0f, 0.0f, 1.0f);
	}
	
	
	void HandleUpdate()
	{
		if (previousLink == null) return;

		Vector2 previousPos = previousLink.transform.position.ToVector2InPoints();
		Vector2 currentPos = transform.position.ToVector2InPoints();

		Vector2 delta = currentPos - previousPos;

		Vector2 center = (currentPos+previousPos)*0.5f;

		sprite.SetPosition(center);
		sprite.rotation = Mathf.Atan2(-delta.y, delta.x) * RXMath.RTOD;
	}
}


