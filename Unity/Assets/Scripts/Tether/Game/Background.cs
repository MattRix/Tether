using System;
using System.Collections.Generic;
using UnityEngine;

public class Background : FContainer
{
	public FSprite sprite;

	public List<Vector2> lavaPositions = new List<Vector2>();
	public FParticleSystem lavaParticles;
	public FParticleDefinition lavaPD;


	public Background()
	{
		AddChild(sprite = new FSprite("background"));

		sprite.scale = 1.3333f;

		AddChild(lavaParticles = new FParticleSystem(100));
		lavaParticles.shader = FShader.Additive;

		InitLava ();

		ListenForUpdate (HandleUpdate);
	}

	void InitLava()
	{
		lavaPD = new FParticleDefinition("Particles/Flame");
		
		lavaPD.startColor = new Color(1.0f,RXRandom.Range(0.0f,1.0f),0,0.3f);
		lavaPD.endColor = lavaPD.startColor.CloneWithNewAlpha(-0.6f);
		
		lavaPositions.Add(new Vector2(-623.9141f,278.9141f));
		lavaPositions.Add(new Vector2(-604.9141f,299.9141f));
		lavaPositions.Add(new Vector2(-574.9141f,321.9141f));
		lavaPositions.Add(new Vector2(-535.9141f,335.9141f));
		lavaPositions.Add(new Vector2(-540.9141f,342.9141f));
		lavaPositions.Add(new Vector2(-599.9141f,339.9141f));
		lavaPositions.Add(new Vector2(-608.9141f,339.9141f));
		lavaPositions.Add(new Vector2(-623.9141f,311.9141f));
		lavaPositions.Add(new Vector2(-630.9141f,342.9141f));
		lavaPositions.Add(new Vector2(-639.9141f,303.9141f));
		lavaPositions.Add(new Vector2(-450.9141f,342.9141f));
		lavaPositions.Add(new Vector2(-439.9141f,313.9141f));
		lavaPositions.Add(new Vector2(-422.9141f,300.9141f));
		lavaPositions.Add(new Vector2(-380.9141f,290.9141f));
		lavaPositions.Add(new Vector2(-346.9141f,292.9141f));
		lavaPositions.Add(new Vector2(-339.9141f,301.9141f));
		lavaPositions.Add(new Vector2(-353.9141f,339.9141f));
		lavaPositions.Add(new Vector2(-368.9141f,349.9141f));
		lavaPositions.Add(new Vector2(-394.9141f,354.9141f));
		lavaPositions.Add(new Vector2(-291.9141f,316.9141f));
		lavaPositions.Add(new Vector2(-266.7813f,309.8672f));
		lavaPositions.Add(new Vector2(-321.7813f,291.8672f));
		lavaPositions.Add(new Vector2(167.2188f,272.8672f));
		lavaPositions.Add(new Vector2(167.2188f,298.8672f));
		lavaPositions.Add(new Vector2(205.2188f,291.8672f));
		lavaPositions.Add(new Vector2(166.2188f,317.8672f));
		lavaPositions.Add(new Vector2(171.2188f,343.8672f));
		lavaPositions.Add(new Vector2(56.21875f,292.8672f));
		lavaPositions.Add(new Vector2(11.21875f,291.8672f));
		lavaPositions.Add(new Vector2(250.2188f,351.8672f));
		lavaPositions.Add(new Vector2(317.2188f,351.8672f));
		lavaPositions.Add(new Vector2(372.2188f,347.8672f));
		lavaPositions.Add(new Vector2(432.2188f,346.8672f));
		lavaPositions.Add(new Vector2(480.2188f,345.8672f));
		lavaPositions.Add(new Vector2(516.2188f,343.8672f));
		lavaPositions.Add(new Vector2(530.2188f,329.8672f));
		lavaPositions.Add(new Vector2(545.2188f,314.8672f));
		lavaPositions.Add(new Vector2(563.2188f,299.8672f));
		lavaPositions.Add(new Vector2(580.2188f,287.8672f));
		lavaPositions.Add(new Vector2(611.2188f,271.8672f));
		lavaPositions.Add(new Vector2(616.2188f,264.8672f));
		lavaPositions.Add(new Vector2(617.2188f,292.8672f));
		lavaPositions.Add(new Vector2(612.2188f,311.8672f));
		lavaPositions.Add(new Vector2(587.2188f,332.8672f));
		lavaPositions.Add(new Vector2(577.2188f,334.8672f));
		lavaPositions.Add(new Vector2(606.2188f,338.8672f));
		lavaPositions.Add(new Vector2(512.2188f,336.8672f));
		lavaPositions.Add(new Vector2(627.2188f,39.86719f));
		lavaPositions.Add(new Vector2(610.2188f,20.86719f));
		lavaPositions.Add(new Vector2(585.2188f,-1.132813f));
		lavaPositions.Add(new Vector2(568.2188f,-5.132813f));
		lavaPositions.Add(new Vector2(542.2188f,-13.13281f));
		lavaPositions.Add(new Vector2(542.2188f,-17.13281f));
		lavaPositions.Add(new Vector2(-301.8711f,308.8242f));
		lavaPositions.Add(new Vector2(-344.8711f,297.8242f));
		lavaPositions.Add(new Vector2(-367.8711f,288.8242f));
		lavaPositions.Add(new Vector2(-382.8711f,290.8242f));
		lavaPositions.Add(new Vector2(-403.8711f,294.8242f));
		lavaPositions.Add(new Vector2(-424.8711f,303.8242f));
		lavaPositions.Add(new Vector2(572.1289f,-7.175781f));
		lavaPositions.Add(new Vector2(548.1289f,-17.17578f));
		lavaPositions.Add(new Vector2(573.1289f,-28.17578f));
		lavaPositions.Add(new Vector2(601.1289f,-32.17578f));
		lavaPositions.Add(new Vector2(600.1289f,-28.17578f));
		lavaPositions.Add(new Vector2(581.1289f,-55.17578f));
		lavaPositions.Add(new Vector2(584.1289f,-44.17578f));
		lavaPositions.Add(new Vector2(557.1289f,-21.17578f));
		lavaPositions.Add(new Vector2(537.1289f,-19.17578f));
		lavaPositions.Add(new Vector2(605.1289f,-56.17578f));
		lavaPositions.Add(new Vector2(627.1289f,-45.17578f));
		lavaPositions.Add(new Vector2(627.1289f,-56.17578f));
		lavaPositions.Add(new Vector2(621.1289f,-127.1758f));
		lavaPositions.Add(new Vector2(614.1289f,-130.1758f));
		lavaPositions.Add(new Vector2(612.1289f,-75.17578f));
		lavaPositions.Add(new Vector2(606.1289f,-83.17578f));
		lavaPositions.Add(new Vector2(598.1289f,-113.1758f));
		lavaPositions.Add(new Vector2(577.1289f,-120.1758f));
		lavaPositions.Add(new Vector2(567.1289f,-125.1758f));
		lavaPositions.Add(new Vector2(608.1289f,-147.1758f));
		lavaPositions.Add(new Vector2(615.1289f,-159.1758f));
		lavaPositions.Add(new Vector2(617.1289f,-164.1758f));
		lavaPositions.Add(new Vector2(629.1289f,-148.1758f));
		lavaPositions.Add(new Vector2(630.1289f,-145.1758f));
		lavaPositions.Add(new Vector2(630.1289f,-184.1758f));
		lavaPositions.Add(new Vector2(630.1289f,-193.1758f));
		lavaPositions.Add(new Vector2(630.1289f,-205.1758f));
		lavaPositions.Add(new Vector2(629.1289f,-250.1758f));
		lavaPositions.Add(new Vector2(629.1289f,-270.1758f));
		lavaPositions.Add(new Vector2(625.1289f,-305.1758f));
		lavaPositions.Add(new Vector2(622.1289f,-317.1758f));
		lavaPositions.Add(new Vector2(612.1289f,-288.1758f));
		lavaPositions.Add(new Vector2(605.1289f,-276.1758f));
		lavaPositions.Add(new Vector2(598.1289f,-300.1758f));
		lavaPositions.Add(new Vector2(593.1289f,-308.1758f));
		lavaPositions.Add(new Vector2(565.1289f,-338.1758f));
		lavaPositions.Add(new Vector2(560.1289f,-323.1758f));
		lavaPositions.Add(new Vector2(560.1289f,-320.1758f));
		lavaPositions.Add(new Vector2(555.1289f,-338.1758f));
		lavaPositions.Add(new Vector2(540.1289f,-356.1758f));
		lavaPositions.Add(new Vector2(529.1289f,-359.1758f));
		lavaPositions.Add(new Vector2(520.1289f,-354.1758f));
		lavaPositions.Add(new Vector2(488.1289f,-352.1758f));
		lavaPositions.Add(new Vector2(488.1289f,-352.1758f));
		lavaPositions.Add(new Vector2(-335.8711f,-344.1758f));
		lavaPositions.Add(new Vector2(-373.9141f,-328.1328f));
		lavaPositions.Add(new Vector2(-412.9141f,-318.1328f));
		lavaPositions.Add(new Vector2(-418.9141f,-321.1328f));
		lavaPositions.Add(new Vector2(-456.9141f,-331.1328f));
		lavaPositions.Add(new Vector2(-471.9141f,-328.1328f));
		lavaPositions.Add(new Vector2(-495.9141f,-321.1328f));
		lavaPositions.Add(new Vector2(-515.9141f,-311.1328f));
		lavaPositions.Add(new Vector2(-540.9141f,-287.1328f));
		lavaPositions.Add(new Vector2(-555.9141f,-282.1328f));
		lavaPositions.Add(new Vector2(-568.9141f,-265.1328f));
		lavaPositions.Add(new Vector2(-577.9141f,-261.1328f));
		lavaPositions.Add(new Vector2(-628.9141f,-255.1328f));
		lavaPositions.Add(new Vector2(-616.9141f,-278.1328f));
		lavaPositions.Add(new Vector2(-600.9141f,-298.1328f));
		lavaPositions.Add(new Vector2(-580.9141f,-315.1328f));
		lavaPositions.Add(new Vector2(-561.9141f,-327.1328f));
		lavaPositions.Add(new Vector2(-549.9141f,-341.1328f));
		lavaPositions.Add(new Vector2(-543.9141f,-350.1328f));
		lavaPositions.Add(new Vector2(-561.9141f,-345.1328f));
		lavaPositions.Add(new Vector2(-582.9141f,-343.1328f));
		lavaPositions.Add(new Vector2(-594.9141f,-323.1328f));
		lavaPositions.Add(new Vector2(-615.9141f,-306.1328f));
		lavaPositions.Add(new Vector2(-625.9141f,-297.1328f));
		lavaPositions.Add(new Vector2(-624.0938f,-318.1758f));
		lavaPositions.Add(new Vector2(-609.0938f,-334.1758f));
		lavaPositions.Add(new Vector2(-601.0938f,-350.1758f));
		lavaPositions.Add(new Vector2(-601.0938f,-352.1758f));
		lavaPositions.Add(new Vector2(-615.0938f,-352.1758f));
		lavaPositions.Add(new Vector2(-625.0938f,-347.1758f));
		lavaPositions.Add(new Vector2(-626.0938f,-347.1758f));
		lavaPositions.Add(new Vector2(-628.0938f,-121.1758f));
		lavaPositions.Add(new Vector2(-624.0938f,-112.1758f));
		lavaPositions.Add(new Vector2(-625.0938f,-96.17578f));
		lavaPositions.Add(new Vector2(-626.0938f,-87.17578f));
		lavaPositions.Add(new Vector2(-588.0938f,12.82422f));
		lavaPositions.Add(new Vector2(-601.0938f,20.82422f));
		lavaPositions.Add(new Vector2(-614.0938f,31.82422f));
		lavaPositions.Add(new Vector2(-618.0938f,38.82422f));
		lavaPositions.Add(new Vector2(-625.0938f,47.82422f));
		lavaPositions.Add(new Vector2(-627.0938f,52.82422f));
		lavaPositions.Add(new Vector2(-626.0938f,74.82422f));
		lavaPositions.Add(new Vector2(-615.0938f,84.82422f));
		lavaPositions.Add(new Vector2(-608.0938f,89.82422f));
		lavaPositions.Add(new Vector2(-589.0938f,97.82422f));
		lavaPositions.Add(new Vector2(-593.0938f,107.8242f));
		lavaPositions.Add(new Vector2(-624.0938f,121.8242f));
		lavaPositions.Add(new Vector2(-627.0938f,134.8242f));
		lavaPositions.Add(new Vector2(-627.0938f,138.8242f));
		lavaPositions.Add(new Vector2(-605.0938f,301.8242f));
		lavaPositions.Add(new Vector2(-588.0938f,317.8242f));
		lavaPositions.Add(new Vector2(-364.0938f,295.8242f));
		lavaPositions.Add(new Vector2(176.9063f,282.8242f));
		lavaPositions.Add(new Vector2(574.9063f,-25.17578f));
		lavaPositions.Add(new Vector2(-319.0938f,-345.1758f));
		lavaPositions.Add(new Vector2(-411.0938f,-321.1758f));
	}

	
	void HandleUpdate ()
	{
		Vector2 lavaPos = lavaPositions[RXRandom.Range(0,lavaPositions.Count-1)];
		
		lavaPD.x = lavaPos.x + RXRandom.Range(-3.0f, 3.0f);
		lavaPD.y = lavaPos.y + RXRandom.Range(-3.0f, 3.0f);
		
		lavaPD.speedX = RXRandom.Range(-10.0f,10.0f);
		lavaPD.speedY = RXRandom.Range(-5.0f,20.0f);
		
		lavaPD.startScale = RXRandom.Range(0.0f,0.1f);
		lavaPD.endScale = RXRandom.Range(0.3f,2.9f);
		
		lavaPD.lifetime = RXRandom.Range(2.0f, 3.0f);
		
		lavaParticles.AddParticle(lavaPD);
	}

}


