using System;

public class UnusedPlayerController : PlayerController
{
	public UnusedPlayerController()
	{
		title = "[UNUSED]";
	}

	override public bool CanBeUsed()
	{
		return true;
	}

	override public void SetPlayer(Player player)
	{
		//DO NOTHING, _player is always null
	}

	override public Player GetPlayer()
	{
		return null;
	}
}