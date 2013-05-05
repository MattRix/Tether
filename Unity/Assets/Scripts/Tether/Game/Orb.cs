using UnityEngine;
using System.Collections.Generic;
using System;

public class Orb : MonoBehaviour
{
	public static Orb Create(World world)
	{
		GameObject orbGO = new GameObject("Orb");
		Orb orb = orbGO.AddComponent<Orb>();
		orb.world = world;
		return orb;
	}
	
	public World world;
	public Player player;
	
	public FContainer holder;
	
	public FPNodeLink link;
	public FSprite sprite;
	public SphereCollider sphereCollider;

	public float lifetime = 6.0f;

	public int frameIndex = 0;
	public FAtlasElement[] frames;

	public float scale = 0.333f;

	public FParticleDefinition pd;
	
	public void Init(Player player, Vector2 startPos)
	{
		this.player = player;
		
		world.orbHolder.AddChild(holder = new FContainer());
		
		gameObject.transform.position = new Vector3(startPos.x * FPhysics.POINTS_TO_METERS,startPos.y * FPhysics.POINTS_TO_METERS,0);
		gameObject.transform.parent = world.root.transform;
		
		link = gameObject.AddComponent<FPNodeLink>();
		link.Init(holder, true);

		frames = new FAtlasElement[11];

		for (int f = 0; f<frames.Length; f++)
		{
			frames[f] = Futile.atlasManager.GetElementWithName("orb"+f.ToString("00"));
		}

		sprite = new FSprite(frames[0].name);
		holder.AddChild(sprite);
		sprite.color = player.color;
		sprite.scale = scale;
		
		InitPhysics();
		
		holder.ListenForUpdate(HandleUpdate);
		holder.ListenForFixedUpdate(HandleFixedUpdate);

		FSoundManager.PlaySound("orbAppears");

		pd = new FParticleDefinition("Particles/Flame");
		
		pd.startColor = player.color.CloneWithNewAlpha(0.2f);
		pd.endColor = Color.clear;
	}
	
	public void Destroy()
	{
		world.orbs.Remove(this);

		UnityEngine.Object.Destroy(gameObject);
		
		holder.RemoveFromContainer();
	}
	
	void InitPhysics()
	{
		Rigidbody rb = gameObject.AddComponent<Rigidbody>();
		rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
		rb.angularDrag = 5.0f;
		rb.mass = 10.0f;
		rb.drag = 0.8f;
		//rb.mass = 0.0f;
		//rb.drag = OrbConfig.DRAG;
		
		sphereCollider = gameObject.AddComponent<SphereCollider>();
		sphereCollider.radius = 50.0f * scale * FPhysics.POINTS_TO_METERS;
		
		PhysicMaterial mat = new PhysicMaterial();
		mat.bounciness = 0.3f;
		mat.dynamicFriction = 0.5f;
		mat.staticFriction = 0.5f;
		mat.frictionCombine = PhysicMaterialCombine.Maximum;
		collider.material = mat;

		float speed = 30.0f;
		float angle = RXRandom.Range(0, RXMath.DOUBLE_PI);
		Vector2 startVector = new Vector2(Mathf.Cos(angle)*speed, Mathf.Sin(angle)*speed);
		rb.velocity = startVector.ToVector3InMeters();
	}

	void OnCollisionEnter(Collision coll)
	{
		FPPolygonalCollider wall = coll.gameObject.GetComponent<FPPolygonalCollider>();

		//Debug.Log(collider.gameObject.name + " hit orb ");

		if (wall != null)
		{
			Explode(false);
		}
	}

	public void Explode(bool isGood)
	{
		FSoundManager.PlaySound("BombSmall");
		OrbExplosion explosion = new OrbExplosion(isGood, player);
		explosion.SetPosition(this.GetPos());
		world.effectHolder.AddChild(explosion);
		Destroy();
	}
	
	void HandleUpdate()
	{
		int frameRate = 2;
		
		if (Time.frameCount % frameRate == 0)
		{
			frameIndex++;
			sprite.element = frames[frameIndex%frames.Length];
		}

		lifetime -= Time.deltaTime;

		if (lifetime <= 2.0f)
		{
			sprite.isVisible = (Time.frameCount % 14 < 7);
		}

		if(lifetime <= 0)
		{
			Explode(false);
		}

		if (Time.frameCount % 5 == 0)
		{

			Vector2 pos = GetPos();

			pd.x = pos.x + RXRandom.Range(-5.0f, 5.0f);
			pd.y = pos.y + RXRandom.Range(-5.0f, 5.0f);
		
			Vector2 speed = RXRandom.Vector2Normalized() * RXRandom.Range(20.0f,80.0f);

			pd.speedX = speed.x;
			pd.speedY = speed.y;
		
			pd.lifetime = RXRandom.Range(0.5f, 1.0f);
		
			world.glowParticles.AddParticle(pd);
		}
		
	}
	
	void HandleFixedUpdate()
	{

	}
	
	public Vector2 GetPos()
	{
		return new Vector2(transform.position.x * FPhysics.METERS_TO_POINTS, transform.position.y * FPhysics.METERS_TO_POINTS);
	}
}


