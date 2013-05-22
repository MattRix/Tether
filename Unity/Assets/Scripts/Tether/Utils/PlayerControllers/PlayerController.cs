using System;
using UnityEngine;
using System.Collections.Generic;

public class PlayerController
{
	public Vector2 movementVector = new Vector2();

	public string title = "TITLE NOT SET";

	protected Player _player = null;

	public bool didJustConnect = false; //can only be true for game controllers
	public bool didJustDisconnect = false; //can only be true for game controllers

	public PlayerController()
	{

	}

	virtual public void Update()
	{

	}

	virtual public bool GetButtonDown(PlayerControllerButtonType buttonType)
	{
		return false;
	}

	virtual public bool CanBeUsed()
	{
		return false;
	}

	virtual public void SetPlayer(Player player)
	{
		_player = player;
	}

	virtual public Player GetPlayer()
	{
		return _player;
	}
}

public enum PlayerControllerButtonType
{
	Start,
	Pause,
	Reset,
	Ready
}


