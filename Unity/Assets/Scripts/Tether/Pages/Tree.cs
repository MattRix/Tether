using System;
using UnityEngine;


public class Tree : FContainer
{
	private Gamepad _gamepad;

	public Tree(Gamepad gamepad)
	{
		_gamepad = gamepad;

		FSprite treeSprite = new FSprite("GameTree");
		AddChild(treeSprite);

		ListenForUpdate (HandleUpdate);
	}

	protected void HandleUpdate ()
	{
		Vector2 movement = _gamepad.leftStick * 5 * 60.0f * Time.deltaTime;

		this.x += movement.x;
		this.y += movement.y;
		
		this.rotation -= _gamepad.rightStick.x * 1.0f * 60.0f * Time.deltaTime;
		this.scale += _gamepad.rightStick.y * 0.01f * 60.0f * Time.deltaTime;
	}
}


