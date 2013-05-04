using UnityEngine;
using System.Collections.Generic;
using System;

public class InGamePage : TPage
{
	private FLabel titleLabel;
	public InGamePage()
	{
		
	}
	
	override public void Start()
	{
		titleLabel = new FLabel("CubanoBig", "TETHER");
		AddChild(titleLabel);
		titleLabel.y = 0;

		ListenForUpdate (HandleUpdate);
	}
	

	protected void HandleUpdate ()
	{
		Vector2 movement = GameManager.instance.players [0].gamepad.leftStick;
		movement *= 5;

		titleLabel.x += movement.x;
		titleLabel.y += movement.y;

		titleLabel.rotation -= GameManager.instance.players [0].gamepad.rightStick.x * 5.0f;

		titleLabel.scale += GameManager.instance.players [0].gamepad.rightStick.y * 0.01f;
	}
}
