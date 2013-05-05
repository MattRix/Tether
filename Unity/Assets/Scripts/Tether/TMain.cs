using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum TPageType
{
	PageNone,
	PagePlayerSelect,
	PageInGame,
	PageResults
}

public class TMain : MonoBehaviour
{	
	public static TMain instance;

	private TPageType _currentPageType = TPageType.PageNone;
	private TPage _currentPage = null;
	
	private FStage _stage;

	private Background _background;
	
	private void Start()
	{
		instance = this; 
		
		Go.defaultEaseType = EaseType.Linear;
		Go.duplicatePropertyRule = DuplicatePropertyRuleType.RemoveRunningProperty;

		//Time.timeScale = 0.1f;
		
		bool landscape = true;
		bool portrait = false;
		
		bool isIPad = SystemInfo.deviceModel.Contains("iPad");
		bool shouldSupportPortraitUpsideDown = isIPad && portrait; //only support portrait upside-down on iPad
		
		FutileParams fparams = new FutileParams(landscape, landscape, portrait, shouldSupportPortraitUpsideDown);
		
		fparams.backgroundColor = RXUtils.GetColorFromHex(0xE7F5FF); //light blue 0x94D7FF or 0x74CBFF
		
		fparams.AddResolutionLevel(2560.0f,	2.0f,	2.0f,	""); //1280x720
		fparams.AddResolutionLevel(1280.0f,	1.0f,	2.0f,	""); //1280x720
		
		fparams.origin = new Vector2(0.5f,0.5f);
		
		Futile.instance.Init (fparams);
		
		Futile.atlasManager.LoadAtlas("Atlases/UIAtlas");
		Futile.atlasManager.LoadAtlas("Atlases/UIFonts");
		Futile.atlasManager.LoadAtlas("Atlases/BackgroundAtlas");
		// TEMP: Load in all atlases for GDC to keep load times small
		Futile.atlasManager.LoadAtlas("Atlases/GameAtlas");
		
		FTextParams textParams;
		
		textParams = new FTextParams();
		textParams.lineHeightOffset = -8.0f;
		Futile.atlasManager.LoadFont("Franchise","FranchiseFont", "Atlases/FranchiseFont", -2.0f,-5.0f,textParams);
		
		textParams = new FTextParams();
		textParams.kerningOffset = -0.5f;
		textParams.lineHeightOffset = -8.0f;
		Futile.atlasManager.LoadFont("CubanoInnerShadow","Cubano_InnerShadow", "Atlases/CubanoInnerShadow", 0.0f,2.0f,textParams);
		
		textParams = new FTextParams();
		textParams.lineHeightOffset = -8.0f;
		textParams.kerningOffset = -0.5f;
		Futile.atlasManager.LoadFont("CubanoBig","Cubano136", "Atlases/Cubano136", 0.0f,2.0f,textParams);

		GamepadManager.Init();
		GameManager.Init();

        _stage = Futile.stage;

		_stage.AddChild(_background = new Background());

        GoToPage(TPageType.PagePlayerSelect);
       
		_stage.ListenForUpdate (HandleUpdate);

		FSoundManager.isMuted = !GameConfig.IS_SOUND_ON;
	}

	void HandleUpdate ()
	{
        GamepadManager.instance.Update();

		if (Input.GetKeyDown (KeyCode.Escape)) 
		{
			Application.Quit();
		}
       
	}

	public void GoToPage (TPageType pageType)
	{
		if(_currentPageType == pageType) return; //we're already on the same page, so don't bother doing anything
		
		TPage pageToCreate = null;
		
		if(pageType == TPageType.PagePlayerSelect)
		{
			pageToCreate = new PlayerSelectPage();
		}
		else if (pageType == TPageType.PageInGame)
		{
			pageToCreate = new InGamePage();
		}  
        else if (pageType == TPageType.PageResults)
        {
			pageToCreate = new PlayerSelectPage();
        }  
		
		if(pageToCreate != null) //destroy the old page and create a new one
		{
			_currentPageType = pageType;	
			
			if(_currentPage != null)
			{
				_currentPage.Destroy();
				_stage.RemoveChild(_currentPage);
			}
			 
			_currentPage = pageToCreate;
			_stage.AddChild(_currentPage);
			_currentPage.Start();
		}
		
	}
	
}









