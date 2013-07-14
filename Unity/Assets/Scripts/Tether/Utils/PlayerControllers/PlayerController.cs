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

	public bool didJustPressUp = false;
	public bool didJustPressDown = false;
	public bool didJustPressRight = false;
	public bool didJustPressLeft = false;

	public bool isPressingUp = false;
	public bool isPressingDown = false;
	public bool isPressingLeft = false;
	public bool isPressingRight = false;

	public PlayerController()
	{

	}

	virtual public void Update()
	{

	}

	virtual public void CalcuateDirectionsBasedOnMovementVector()
	{
		float threshold = 0.4f;

		didJustPressUp = false;
		didJustPressDown = false;
		didJustPressRight = false;
		didJustPressLeft = false;

		if(movementVector.x < -threshold)
		{
			if(!isPressingLeft)
			{
				isPressingLeft = true;
				didJustPressLeft = true;
			}
		}
		else
		{
			isPressingLeft = false;
		}

		if(movementVector.x > threshold)
		{
			if(!isPressingRight)
			{
				isPressingRight = true;
				didJustPressRight = true;
			}
		}
		else
		{
			isPressingRight = false;
		}

		if(movementVector.y < -threshold)
		{
			if(!isPressingDown)
			{
				isPressingDown = true;
				didJustPressDown = true;
			}
		}
		else
		{
			isPressingDown = false;
		}

		if(movementVector.y > threshold)
		{
			if(!isPressingUp)
			{
				isPressingUp = true;
				didJustPressUp = true;
			}
		}
		else
		{
			isPressingUp = false;
		}
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


