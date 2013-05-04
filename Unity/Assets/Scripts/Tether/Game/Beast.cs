using UnityEngine;
using System.Collections.Generic;
using System;

public class Beast : MonoBehaviour
{
	public static Beast Create(World world)
	{
		GameObject beastGO = new GameObject("Beast");
		Beast beast = beastGO.AddComponent<Beast>();
		beast.world = world;
		return beast;
	}

	public World world;
	public Player player;

	public FContainer holder;

	public FPNodeLink bodyLink;
	public FSprite bodySprite;
	public SphereCollider bodyCollider;

	public Vector2 bodyVelocity = new Vector2();
		
	public void Init(Player player, Vector2 startPos)
	{
		this.player = player;

		world.entityHolder.AddChild(holder = new FContainer());

		gameObject.transform.position = new Vector3(startPos.x * FPhysics.POINTS_TO_METERS,startPos.y * FPhysics.POINTS_TO_METERS,0);
		gameObject.transform.parent = world.root.transform;

		bodyLink = gameObject.AddComponent<FPNodeLink>();
		bodyLink.Init(holder, true);

		bodySprite = new FSprite("Player");
		holder.AddChild(bodySprite);
		bodySprite.color = player.color;

		InitPhysics();

		holder.ListenForUpdate(HandleUpdate);
		holder.ListenForFixedUpdate(HandleFixedUpdate);
	}

	public void Destroy()
	{
		UnityEngine.Object.Destroy(gameObject);
		
		holder.RemoveFromContainer();
	}
	
	void InitPhysics()
	{
		Rigidbody rb = gameObject.AddComponent<Rigidbody>();
		rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
		rb.angularDrag = 5.0f;
		rb.mass = 12.0f;
		//rb.mass = 0.0f;
		//rb.drag = BeastConfig.DRAG;
		
		bodyCollider = gameObject.AddComponent<SphereCollider>();
		bodyCollider.radius = 35.0f * FPhysics.POINTS_TO_METERS;
		
		PhysicMaterial mat = new PhysicMaterial();
		mat.bounciness = 0.3f;
		mat.dynamicFriction = 0.1f;
		mat.staticFriction = 0.1f;
		mat.frictionCombine = PhysicMaterialCombine.Maximum;
		collider.material = mat;
	}

	void OnCollisionEnter(Collision coll)
	{
		Beast beast = coll.collider.gameObject.GetComponent<Beast>();

		if (beast != null)
		{
			if(beast.player == this.player)
			{
				beast.player.AddScore();
				Destroy();
			}
		}
	}

	void HandleUpdate()
	{

	}

	void HandleFixedUpdate()
	{
		rigidbody.drag = BeastConfig.DRAG;
		
		Gamepad gamepad = player.gamepad;


		Vector2 movementVector = player.isSpecial ? gamepad.rightStick : gamepad.leftStick;

//		if (movementVector.magnitude > 0.1f)
//		{
//			rigidbody.drag = BeastConfig.DRAG;
//		}
//		else
//		{
//			rigidbody.drag = 5.0f;
//		}

		movementVector *= BeastConfig.MOVE_SPEED * Time.smoothDeltaTime * rigidbody.mass;

		movementVector *= 2.0f;

//		if (player.isSpecial)
//		{
//			if (gamepad.GetButton(PS3ButtonType.Square))
//			{
//				movementVector *= 2.0f;
//			}
//		}
//		else
//		{
//			if (gamepad.GetButton(PS3ButtonType.X))
//			{
//				movementVector *= 2.0f;
//			}
//		}

		bodyVelocity += movementVector;
		
		rigidbody.AddForce(new Vector3(bodyVelocity.x, bodyVelocity.y, 0.0f), ForceMode.Impulse);
		
		bodyVelocity *= BeastConfig.MOVE_FRICTION;
	}

	public Vector2 GetPos()
	{
		return new Vector2(transform.position.x * FPhysics.METERS_TO_POINTS, transform.position.y * FPhysics.METERS_TO_POINTS);
	}
}

public static class BeastConfig
{
	public static float DRAG = 15.0f * 0.5f;
	public static float MOVE_SPEED = 35.0f;
	public static float MOVE_FRICTION = 0.4f;

	static BeastConfig()
	{
		FWatcher.Watch(typeof(BeastConfig));
	}
}


