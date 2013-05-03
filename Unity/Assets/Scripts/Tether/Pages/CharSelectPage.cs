using UnityEngine;
using System.Collections;
using System;

public class CharSelectPage : TPage
{
	private Tree _treeA;
	private Tree _treeB;

	public CharSelectPage()
	{
		ListenForUpdate (HandleUpdate);
	}
	
	override public void Start()
	{
		_treeA = new Tree(GamepadManager.instance.GetGamepad(0));
		_treeB = new Tree(GamepadManager.instance.GetGamepad(1));

		AddChild(_treeA);
		AddChild(_treeB);
	}

	
	protected void HandleUpdate ()
	{

	}

}

