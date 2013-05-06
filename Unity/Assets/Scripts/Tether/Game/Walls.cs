using UnityEngine;
using System.Collections.Generic;
using System;

public class Walls
{	
	public World world;
	
	public GameObject topGO;
	public GameObject bottomGO;
	
	public Walls(World world)
	{
		this.world = world;

		FPPolygonalCollider polyCol;
		List<Vector2>verts;

		topGO = new GameObject("TopWall");
		topGO.transform.parent = world.root.transform;

//		verts = new List<Vector2>();
//		verts.Add(new Vector2(0, 10));
//		verts.Add(new Vector2(10, 10));
//		verts.Add(new Vector2(10, 0));
//		verts.Add(new Vector2(0, 0));

		//Futile.stage.scale = 0.9f;

		verts = new List<Vector2>();
		verts.Add(new Vector2(-652.9905f,-33.31163f));
		verts.Add(new Vector2(-651.8794f,373.355f));
		verts.Add(new Vector2(664.7874f,374.4662f));
		verts.Add(new Vector2(654.7874f,-21.08941f));
		verts.Add(new Vector2(638.1207f,-23.31163f));
		verts.Add(new Vector2(625.8984f,221.1328f));
		verts.Add(new Vector2(531.454f,298.9106f));
		verts.Add(new Vector2(382.5651f,338.9106f));
		verts.Add(new Vector2(-290.7682f,342.2439f));
		verts.Add(new Vector2(-455.2127f,338.9106f));
		verts.Add(new Vector2(-525.2127f,303.355f));
		verts.Add(new Vector2(-598.546f,240.0217f));
		verts.Add(new Vector2(-614.1016f,173.355f));
		verts.Add(new Vector2(-602.9905f,-42.20052f));


		polyCol = topGO.AddComponent<FPPolygonalCollider>();
		polyCol.Init(new FPPolygonalData(verts.ToArray(),false));

		//FPDebugRenderer.Create(topGO, world.effectHolder, 0x00FF00, false);

		bottomGO = new GameObject("BottomWall");
		bottomGO.transform.parent = world.root.transform;
		
		//		verts = new List<Vector2>();
		//		verts.Add(new Vector2(0, 10));
		//		verts.Add(new Vector2(10, 10));
		//		verts.Add(new Vector2(10, 0));
		//		verts.Add(new Vector2(0, 0));
		
		verts = new List<Vector2>();
		verts.Add(new Vector2(-682.9905f,-28.88889f));
		verts.Add(new Vector2(-605.2127f,-36.66667f));
		verts.Add(new Vector2(-610.7682f,-132.2222f));
		verts.Add(new Vector2(-615.2127f,-203.3333f));
		verts.Add(new Vector2(-598.546f,-257.7778f));
		verts.Add(new Vector2(-536.3238f,-322.2222f));
		verts.Add(new Vector2(-356.3238f,-345.5556f));
		verts.Add(new Vector2(522.5651f,-346.6667f));
		verts.Add(new Vector2(590.3429f,-266.6667f));
		verts.Add(new Vector2(615.8984f,-183.3333f));
		verts.Add(new Vector2(638.1207f,-25.55556f));
		verts.Add(new Vector2(702.5651f,-24.44445f));
		verts.Add(new Vector2(702.5651f,-396.6667f));
		verts.Add(new Vector2(-690.7682f,-393.3333f));
		verts.Add(new Vector2(-681.8794f,-55.55556f));




		
		polyCol = bottomGO.AddComponent<FPPolygonalCollider>();
		polyCol.Init(new FPPolygonalData(verts.ToArray(),false));

		//FPDebugRenderer.Create(bottomGO, world.effectHolder, 0x0000FF, false);

		world.effectHolder.ListenForUpdate(HandleUpdate);
	}
	
	public void Destroy()
	{
		UnityEngine.Object.Destroy(topGO);
		UnityEngine.Object.Destroy(bottomGO);
	}
	
	public string result = "";

	void HandleUpdate()
	{
		if (Input.GetMouseButtonDown(1))
		{
			System.IO.File.WriteAllText("Assets/Notes/Polygon.txt", result);
			Debug.Log(result);
			result = "";
		}
		else if (Input.GetMouseButtonDown(0))
		{
			Vector2 mousePos = world.effectHolder.GetLocalMousePosition();
			result += "lavaPositions.Add(new Vector2("+mousePos.x+"f,"+mousePos.y+"f));\n";
		}
	}
	
	void HandleFixedUpdate()
	{
		
	}
}


