using UnityEngine;
using System.Collections.Generic;
using System;

public class Tentacle
{	
	public World world;
	public Beast beast;

	public GameObject goA;
	public GameObject goB;
	public GameObject goC;

	public FSprite spriteA;
	public FSprite spriteB;
	public FSprite spriteC;

	public Vector2 startPos;
	
	public Tentacle(World world, Beast beast, Vector2 startPos, float angle)
	{
		this.world = world;
		this.beast = beast;
		this.startPos = startPos;


		Vector2 nextPos = beast.transform.position.ToVector2InPoints();
		nextPos += startPos;

		nextPos.x += 20.0f;

		goA = new GameObject("TentacleA");
		goA.transform.parent = world.root.transform;
		goA.transform.position = nextPos.ToVector3InMeters();

		FSprite debugSpriteA = new FSprite("WhiteBox");
		world.effectHolder.AddChild(debugSpriteA);
		FPNodeLink nodeLinkA = goA.AddComponent<FPNodeLink>();
		nodeLinkA.Init(debugSpriteA, true);
		debugSpriteA.scale = 0.25f;

		Rigidbody rb;
		JointSpring jspring;
		JointLimits jlimits;

		float minAngle = 180.0f + -20.0f;
		float maxAngle = 180.0f + 20.0f;

		rb = goA.AddComponent<Rigidbody>();
		rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
		rb.angularDrag = 5.0f;
		rb.mass = 5.0f;
		rb.drag = 1.0f;

		goA.AddComponent<HingeJoint>();
		goA.hingeJoint.connectedBody = beast.rigidbody;
		//jspring = goA.hingeJoint.spring;
		//jspring.spring = 0.5f;
		//goA.hingeJoint.spring = jspring;
		goA.hingeJoint.axis = new Vector3(0.0f, 0.0f, 1.0f);
		goA.hingeJoint.anchor = startPos.ToVector3InMeters();
		goA.hingeJoint.useLimits = true;
		jlimits = goA.hingeJoint.limits;
		jlimits.min = angle+minAngle;
		jlimits.max = angle+maxAngle;
		goA.hingeJoint.limits = jlimits;

		nextPos.x += 20.0f;
		goB = new GameObject("TentacleB");
		goB.transform.parent = world.root.transform;
		goB.transform.position = nextPos.ToVector3InMeters();

		FSprite debugSpriteB = new FSprite("WhiteBox");
		world.effectHolder.AddChild(debugSpriteB);
		FPNodeLink nodeLinkB = goB.AddComponent<FPNodeLink>();
		nodeLinkB.Init(debugSpriteB, true);
		debugSpriteB.scale = 0.25f;
		
		rb = goB.AddComponent<Rigidbody>();
		rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
		rb.angularDrag = 5.0f;
		rb.mass = 5.0f;
		rb.drag = 1.0f;
		
		goB.AddComponent<HingeJoint>();
		goB.hingeJoint.connectedBody = goA.rigidbody;

		//jspring = goB.hingeJoint.spring;
		//jspring.spring = 0.5f;
		//goB.hingeJoint.spring = jspring;
		goB.hingeJoint.axis = new Vector3(0.0f, 0.0f, 1.0f);
		goB.hingeJoint.anchor = new Vector2(20.0f,0.0f).ToVector3InMeters();
		goB.hingeJoint.useLimits = true;
		jlimits = goB.hingeJoint.limits;
		jlimits.min = minAngle;
		jlimits.max = maxAngle;
		goB.hingeJoint.limits = jlimits;



		nextPos.x += 5.0f;

		goC = new GameObject("TentacleC");
		goC.transform.parent = world.root.transform;
		goC.transform.position = nextPos.ToVector3InMeters();

		FSprite debugSpriteC = new FSprite("WhiteBox");
		world.effectHolder.AddChild(debugSpriteC);
		FPNodeLink nodeLinkC = goC.AddComponent<FPNodeLink>();
		nodeLinkC.Init(debugSpriteC, true);
		debugSpriteC.scale = 0.25f;

		rb = goC.AddComponent<Rigidbody>();
		rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
		rb.angularDrag = 5.0f;
		rb.mass = 5.0f;
		rb.drag = 1.0f;
		
		goC.AddComponent<HingeJoint>();
		goC.hingeJoint.connectedBody = goB.rigidbody;
		//jspring = goC.hingeJoint.spring;
		//jspring.spring = 0.0f;
		//goC.hingeJoint.spring = jspring;
		goC.hingeJoint.axis = new Vector3(0.0f, 0.0f, 1.0f);
		goC.hingeJoint.anchor = new Vector2(0.0f,5.0f).ToVector3InMeters();
		goC.hingeJoint.useLimits = true;
		jlimits = goC.hingeJoint.limits;
		jlimits.min = minAngle;
		jlimits.max = maxAngle;
		goC.hingeJoint.limits = jlimits;

		spriteA = new FSprite("Eye/Evil-Eye_"+beast.player.numString+"_02");
		world.tentacleHolder.AddChild(spriteA);
		spriteA.ListenForUpdate(HandleUpdate);

		spriteB = new FSprite("Eye/Evil-Eye_"+beast.player.numString+"_04");
		world.tentacleHolder.AddChild(spriteB);

		spriteC = new FSprite("Eye/Evil-Eye_"+beast.player.numString+"_05");
		world.tentacleHolder.AddChild(spriteC);
	}
	
	public void Destroy()
	{
		UnityEngine.Object.Destroy(goA);
		UnityEngine.Object.Destroy(goB);
		UnityEngine.Object.Destroy(goC);

		spriteA.RemoveFromContainer();
		spriteB.RemoveFromContainer();
		spriteC.RemoveFromContainer();
		
		//holder.RemoveFromContainer();
	}
	

	void HandleUpdate()
	{
		spriteA.SetPosition(goA.transform.position.ToVector2InPoints());
		spriteA.rotation = -goA.transform.rotation.eulerAngles.z;
		
		spriteB.SetPosition(goB.transform.position.ToVector2InPoints());
		spriteB.rotation = -goB.transform.rotation.eulerAngles.z;

		spriteC.SetPosition(goC.transform.position.ToVector2InPoints());
		spriteC.rotation = -goC.transform.rotation.eulerAngles.z;
	}
	
	void HandleFixedUpdate()
	{
		
	}
}


