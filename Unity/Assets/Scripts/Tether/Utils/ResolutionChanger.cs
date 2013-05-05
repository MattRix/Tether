using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;

public class ResolutionChanger
{
	[MenuItem("ResolutionChanger/720p")]
	public static void SetSevenTwenty()
	{
		SetResolution(1280,720);		
	}
	
	[MenuItem("ResolutionChanger/1440p")]
	public static void SetFourteen()
	{
		SetResolution(2560,1440);		
	}
	
	private static void SetResolution(int width, int height)
	{
		PlayerSettings.defaultScreenHeight = height;
		PlayerSettings.defaultScreenWidth = width;
	}
}

#else 

public class ResolutionChanger
{
	
}

#endif