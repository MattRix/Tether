using System;

public class AIPlayerController : PlayerController
{
	public AIPlayerController()
	{
		title = "AI";
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