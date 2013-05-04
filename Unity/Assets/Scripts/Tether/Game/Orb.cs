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

	public float lifetime = 7.0f;
	
	public void Init(Player player, Vector2 startPos)
	{
		this.player = player;
		
		world.entityHolder.AddChild(holder = new FContainer());
		
		gameObject.transform.position = new Vector3(startPos.x * FPhysics.POINTS_TO_METERS,startPos.y * FPhysics.POINTS_TO_METERS,0);
		gameObject.transform.parent = world.root.transform;

		gameObject.layer = 11;
		
		link = gameObject.AddComponent<FPNodeLink>();
		link.Init(holder, true);
		
		sprite = new FSprite("Orb");
		holder.AddChild(sprite);
		sprite.color = player.color;
		
		InitPhysics();
		
		holder.ListenForUpdate(HandleUpdate);
		holder.ListenForFixedUpdate(HandleFixedUpdate);
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
		rb.mass = 20.0f;
		rb.drag = 2.0f;
	
		//rb.mass = 0.0f;
		//rb.drag = OrbConfig.DRAG;
		
		sphereCollider = gameObject.AddComponent<SphereCollider>();
		sphereCollider.radius = 35.0f * FPhysics.POINTS_TO_METERS;
		
		PhysicMaterial mat = new PhysicMaterial();
		mat.bounciness = 0.3f;
		mat.dynamicFriction = 0.5f;
		mat.staticFriction = 0.5f;
		mat.frictionCombine = PhysicMaterialCombine.Maximum;
		collider.material = mat;
	}
	
	void HandleUpdate()
	{
		lifetime -= Time.deltaTime;

		if (lifetime < 3.0f)
		{
			if(Time.frameCount % 10 < 5)
			{
				holder.isVisible = false;
			}
			else 
			{
				holder.isVisible = true;
			}
		}

		if(lifetime <= 0)
		{
			Destroy();
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


