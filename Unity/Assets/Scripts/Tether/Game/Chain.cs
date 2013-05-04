using UnityEngine;
using System.Collections.Generic;
using System;

public class Chain
{
	public static int LINK_COUNT = 12;

	public World world;

	public Beast beastA;
	public Beast beastB;

	public FSprite sprite;

	public Chain(World world, Beast beastA, Beast beastB)
	{
		this.world = world;

		this.beastA = beastA;
		this.beastB = beastB;

		Vector2 startPos = beastA.GetPos();
		Vector2 endPos = beastB.GetPos();

		//Vector2 delta = (endPos - startPos) / (float)LINK_COUNT;

		beastB.transform.position = new Vector2(startPos.x + 50.0f, startPos.y).ToVector3InMeters();

		SpringJoint spring = beastA.gameObject.AddComponent<SpringJoint>();
		spring.connectedBody = beastB.rigidbody;

		spring.spring = 9.0f;


		beastA.transform.position = new Vector3(startPos.x * FPhysics.POINTS_TO_METERS, startPos.y * FPhysics.POINTS_TO_METERS);
		beastB.transform.position = new Vector3(endPos.x * FPhysics.POINTS_TO_METERS, endPos.y * FPhysics.POINTS_TO_METERS);

		sprite = new FSprite("ChainLink");
		world.entityHolder.AddChild(sprite);
		sprite.ListenForUpdate(HandleUpdate);
	}

	void HandleUpdate()
	{
		Vector2 startPos = beastA.GetPos();
		Vector2 endPos = beastB.GetPos();
		Vector2 delta = endPos - startPos;
		Vector2 center = (startPos + endPos) * 0.5f;

		sprite.SetPosition(center);

		sprite.rotation = Mathf.Atan2(-delta.y, delta.x) * RXMath.RTOD;

		sprite.width = delta.magnitude;
	}

	public void Destroy()
	{
		sprite.RemoveFromContainer();
	}
}
