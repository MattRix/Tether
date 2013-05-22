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
	
	public float angle = 0;
	public float targetAngle = 0;

	public FSprite eyeSprite;
	public Vector2 eyeTarget = new Vector2(3,19);
	
	public FSprite goldSprite;

	public FSprite shadow;

	public void Init(Player player, Vector2 startPos)
	{
		this.player = player;

		world.beastHolder.AddChild(holder = new FContainer());

		gameObject.transform.position = new Vector3(startPos.x * FPhysics.POINTS_TO_METERS,startPos.y * FPhysics.POINTS_TO_METERS,0);
		gameObject.transform.parent = world.root.transform;

		bodyLink = gameObject.AddComponent<FPNodeLink>();
		bodyLink.Init(holder, false);

		bodySprite = new FSprite("Evil-Eye_"+player.numString+"_01");
		holder.AddChild(bodySprite);

		eyeSprite = new FSprite("Eye_" + player.numString);
		eyeSprite.scale = 0.33f;
		holder.AddChild(eyeSprite);
		//holder.alpha = 0.25f;

		goldSprite = new FSprite("Evil-Eye_crown_01");
		holder.AddChild(goldSprite);
		goldSprite.isVisible = false;
		//goldSprite.shader = FShader.Additive;

		InitPhysics();

		holder.ListenForUpdate(HandleUpdate);
		holder.ListenForLateUpdate(HandleLateUpdate);
		holder.ListenForFixedUpdate(HandleFixedUpdate);

		//AddTentacle(new Vector2(-20.0f, -20.0f), -90.0f);
		//AddTentacle(new Vector2(0.0f, -30.0f), 0.0f);
		//AddTentacle(new Vector2(20.0f, -20.0f), 90.0f);
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
		Orb orb = coll.collider.gameObject.GetComponent<Orb>();

		if (orb != null)
		{
			if(orb.player == this.player)
			{
				if(!world.isGameOver)
				{
					orb.player.AddScore();
					FSoundManager.PlaySound("pickUpOrb");
				}
				orb.Explode(true);
			}
			else //hit enemy orb
			{
				FSoundManager.PlaySound("attack", Mathf.Clamp01(coll.impactForceSum.magnitude/10.0f) * 0.5f);
			}
		}

		Beast beast = coll.collider.gameObject.GetComponent<Beast>();

		if (beast != null)
		{
			FSoundManager.PlaySound("attack", Mathf.Clamp01(coll.impactForceSum.magnitude/10.0f) * 0.2f);
		}
	}

	void HandleUpdate()
	{
		player.controller.Update();

		Vector2 eyeCenter = new Vector2(3.0f, 19.0f);

		if (RXRandom.Float() < 0.02f)
		{
			float angle = RXRandom.Range(0,RXMath.DOUBLE_PI);
			float dist = RXRandom.Range(0.0f,8.0f);
			eyeTarget = eyeCenter += new Vector2(Mathf.Cos(angle)*dist * 1.3f, Mathf.Sin(angle)*dist);
		}

		eyeSprite.x += (eyeTarget.x - eyeSprite.x) / 10.0f;
		eyeSprite.y += (eyeTarget.y - eyeSprite.y) / 10.0f;
	}

	void HandleLateUpdate()
	{
		Vector2 pos = GetPos();
		pos.y -= 3.0f;

		shadow.SetPosition(pos);
		shadow.rotation = holder.rotation;
	}

	void HandleFixedUpdate()
	{
		//if (world.isGameOver) return; //you can't play if you lose!

		rigidbody.drag = BeastConfig.DRAG;
		

		Vector2 movementVector = player.controller.movementVector;

		movementVector *= 1.5f;

		if (movementVector.magnitude > 1.0f)
		{
			movementVector.Normalize();
		}

		movementVector *= BeastConfig.MOVE_SPEED * Time.smoothDeltaTime * rigidbody.mass;

		if (movementVector.magnitude > 0.1f)
		{
			targetAngle = movementVector.GetAngle() + 90.0f;
		}
		
		angle += RXMath.GetDegreeDelta(angle,targetAngle) / 10.0f;
		
		holder.rotation = angle;

		movementVector *= 2.0f;


//			if (gamepad.GetButton(PS3ButtonType.X))
//			{
//				movementVector *= 2.0f;
//			}

		bodyVelocity += movementVector;
		
		rigidbody.AddForce(new Vector3(bodyVelocity.x, bodyVelocity.y, 0.0f), ForceMode.Impulse);
		
		bodyVelocity *= BeastConfig.MOVE_FRICTION;

		if (RXRandom.Float() < 0.99f)
		{
			Vector2 pos = this.transform.position.ToVector2InPoints();

			FParticleDefinition pd = new FParticleDefinition(RXRandom.Bool() ? "Particles/SplotchA" : "Particles/SplotchB");

			pd.x = pos.x + RXRandom.Range(-10.0f, 10.0f);
			pd.y = pos.y + RXRandom.Range(-10.0f, 10.0f);

			pd.startColor = player.color.CloneWithNewAlpha(0.35f);
			pd.endColor = player.color.CloneWithNewAlpha(-2.5f);

			pd.lifetime = 4.0f;
	
			world.backParticles.AddParticle(pd);
		}
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


