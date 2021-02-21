using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Gamelogic;
using UnityEngine.SceneManagement;
using GameAnalyticsSDK;

public class MenuManager : Singleton<MenuManager> 
{
	//private ArrayList menu; 



#region OBSERVER_BEHAVIOR
	
	void OnEnable()
	{
		GameManager.Instance.OnMainMenuState += onMainMenuState;
		GameManager.Instance.OnGamePlayState += onGamePlayState;
		GameManager.Instance.onPurchaseConfirmationState += onPurchaseConfirmationState;
		GameManager.Instance.OnOutOfCoinsState += onOutOfCoinsState;
		GameManager.Instance.OnGameOverState += onGameOverState;
		GameManager.Instance.OnSettingState += onSettingState;
		GameManager.Instance.OnPauseState += OnPauseState;
		GameManager.Instance.OnShareState += OnShareState;
		GameManager.Instance.OnPlayerSelectionState += OnPlayerSelectionState;
		GameManager.Instance.OnSwtchSceneState += OnSwtchSceneState;
		GameManager.Instance.OnRateUsState += OnRateUsState;
		GameManager.Instance.OnAdNotAvaiableState += OnAdNotAvaiableState;
		GameManager.Instance.OnInternetNotConnectedState += OnInternetNotConnectedState;
		GameManager.Instance.OnVideoAdConfirmationState += OnVideoAdConfirmationState;
		GameManager.Instance.OnLockedPlayerState += OnLockedPlayerState;

	}
	
	void OnDisable()
	{
		GameManager.Instance.OnMainMenuState -= onMainMenuState;
		GameManager.Instance.OnGamePlayState -= onGamePlayState;
		GameManager.Instance.onPurchaseConfirmationState -= onPurchaseConfirmationState;
		GameManager.Instance.OnOutOfCoinsState -= onOutOfCoinsState;
		GameManager.Instance.OnGameOverState -= onGameOverState;
		GameManager.Instance.OnSettingState -= onSettingState;
		GameManager.Instance.OnPauseState -= OnPauseState;
		GameManager.Instance.OnShareState -= OnShareState;
		GameManager.Instance.OnPlayerSelectionState -= OnPlayerSelectionState;
		GameManager.Instance.OnSwtchSceneState -= OnSwtchSceneState;
		GameManager.Instance.OnRateUsState -= OnRateUsState;
		GameManager.Instance.OnAdNotAvaiableState -= OnAdNotAvaiableState;
		GameManager.Instance.OnInternetNotConnectedState -= OnInternetNotConnectedState;
		GameManager.Instance.OnVideoAdConfirmationState -= OnVideoAdConfirmationState;
		GameManager.Instance.OnLockedPlayerState -= OnLockedPlayerState;


	}
	
	void onMainMenuState()
	{
//		if(GoogleAnalyticsV3.instance)
//			GoogleAnalyticsV3.instance.LogScreen("Main Menu");

		GameAnalytics.NewDesignEvent ("Menu:Main Menu");

		destroyMenus();
		Instantiate((GameObject) Resources.Load(Menus.MainMenu));
		
	}

	void onPurchaseConfirmationState()
	{
//		if(GoogleAnalyticsV3.instance)
//			GoogleAnalyticsV3.instance.LogScreen("Confirm Purchase");
//
//		GoogleAnalyticsV3.instance.DispatchHits();

		Instantiate((GameObject)Resources.Load(Menus.ConfirmPurchase));
	}

	void onOutOfCoinsState()
	{
//		if(GoogleAnalyticsV3.instance)
//			GoogleAnalyticsV3.instance.LogScreen("Out of Coins");
//
//		GoogleAnalyticsV3.instance.DispatchHits();

		GameAnalytics.NewDesignEvent ("Menu:Out of Coins");

		Instantiate((GameObject)Resources.Load(Menus.OutOfCoins));
	}

	void onGameOverState()
	{
//		if(GoogleAnalyticsV3.instance)
//			GoogleAnalyticsV3.instance.LogScreen("Game Over");
//
//		GoogleAnalyticsV3.instance.DispatchHits();

		GameAnalytics.NewDesignEvent("Gameplay:Revive State");

		destroyMenus();

		StartCoroutine (showReviveMenu(2));

	}

	void onGamePlayState()
	{
//		if(GoogleAnalyticsV3.instance)
//			GoogleAnalyticsV3.instance.LogScreen("Game Play");
//
//		GoogleAnalyticsV3.instance.DispatchHits();

		destroyMenus();
	}

	void onSettingState()
	{
//		if(GoogleAnalyticsV3.instance)
//			GoogleAnalyticsV3.instance.LogScreen("Settings");
//
//		GoogleAnalyticsV3.instance.DispatchHits();

		GameAnalytics.NewDesignEvent ("Menu:Settings");


		if(!GameObject.FindGameObjectWithTag(Tags.Settings))
			Instantiate((GameObject) Resources.Load(Menus.Settings));
	}

	void OnPauseState()
	{
//		if(GoogleAnalyticsV3.instance)
//			GoogleAnalyticsV3.instance.LogScreen("Pause");
//		
//		GoogleAnalyticsV3.instance.DispatchHits();

		GameAnalytics.NewDesignEvent ("Menu:Pause");
		
		destroyMenus();
		Instantiate((GameObject) Resources.Load(Menus.Pause));
		Time.timeScale = 0;
	}

	void OnShareState()
	{
//		if(GoogleAnalyticsV3.instance)
//			GoogleAnalyticsV3.instance.LogScreen("Share");
//		
//		GoogleAnalyticsV3.instance.DispatchHits();

//		if(!GameObject.FindGameObjectWithTag(Tags.SocialSharing))
//			Instantiate(Resources.Load(Menus.SocialSharing));

		GameAnalytics.NewDesignEvent ("Menu:Share");


		string text = "Hey! I have scored " + Prefs.highScore + " in Dino Rage 3D. Challenge me!";
		text += "\n" + Constants.ANDROID_TWEET_BITLY_LINK;

		ShareBunch.GetInstance().ShareText(text);
	}

	void OnPlayerSelectionState()
	{
//		if(GoogleAnalyticsV3.instance)
//			GoogleAnalyticsV3.instance.LogScreen("Player Selection");
//
//		GoogleAnalyticsV3.instance.DispatchHits();

		destroyMenus();

		GameAnalytics.NewDesignEvent ("Menu:Player Selection");


		GameManager.instance.adsManager.RequestRewardedVideo ();
		
		if(!GameObject.FindGameObjectWithTag(Tags.PlayerSelectionMenu))
			Instantiate((GameObject) Resources.Load(Menus.PlayerSelectionMenu));
	}

	void OnSwtchSceneState()
	{
		if (SceneManager.GetActiveScene ().name.Equals (Constants.MAINMENU_SCENE)) 
		{
			Instantiate ((GameObject) Resources.Load (Menus.Loading));
		}
	}

	void OnRateUsState()
	{
//		if(GoogleAnalyticsV3.instance)
//			GoogleAnalyticsV3.instance.LogScreen("Rate Us");
//		
//		GoogleAnalyticsV3.instance.DispatchHits();

		GameAnalytics.NewDesignEvent ("Menu:Rate Us");

		Application.OpenURL(Constants.ANDROID_APP_URL);
	}

	void OnAdNotAvaiableState()
	{
		if(!GameObject.FindGameObjectWithTag(Tags.AdNotAvailable))
		{
			GameAnalytics.NewDesignEvent ("Ad not available");

			Instantiate((GameObject) Resources.Load(Menus.AdNotAvailable));
		}
	}

	void OnInternetNotConnectedState()
	{
		if(!GameObject.FindGameObjectWithTag(Tags.InternetNotConnected))
		{
			Instantiate((GameObject) Resources.Load(Menus.InternetNotConnected));
		}
	}

	void OnVideoAdConfirmationState()
	{
		if(!GameObject.FindGameObjectWithTag(Tags.VideoAdConfirmation))
		{
			Instantiate((GameObject) Resources.Load(Menus.VideoAdConfirmation));
		}
	}

	void OnLockedPlayerState()
	{
		if(!GameObject.FindGameObjectWithTag(Tags.LockedPlayerPopup))
		{
			Instantiate((GameObject) Resources.Load(Menus.LockedPlayerPopup));
		}
	}
	
#endregion

	
	void destroyMenus()
	{
		if(GameObject.FindGameObjectWithTag(Tags.MainMenu))
			Destroy(GameObject.FindGameObjectWithTag(Tags.MainMenu));

		if(GameObject.FindGameObjectWithTag(Tags.ConfirmPurchase))
			Destroy(GameObject.FindGameObjectWithTag(Tags.ConfirmPurchase));

		if(GameObject.FindGameObjectWithTag(Tags.PlayerSelectionMenu))
			Destroy(GameObject.FindGameObjectWithTag(Tags.PlayerSelectionMenu));

		if(GameObject.FindGameObjectWithTag(Tags.OutOfCoins))
			Destroy(GameObject.FindGameObjectWithTag(Tags.OutOfCoins));

		if(GameObject.FindGameObjectWithTag(Tags.TutorialCanvas))
			Destroy(GameObject.FindGameObjectWithTag(Tags.TutorialCanvas));

		if(GameObject.FindGameObjectWithTag(Tags.LevelEndMenu))
			Destroy(GameObject.FindGameObjectWithTag(Tags.LevelEndMenu));

		if(GameObject.FindGameObjectWithTag(Tags.SocialSharing))
			Destroy(GameObject.FindGameObjectWithTag(Tags.SocialSharing));

		if(GameObject.FindGameObjectWithTag(Tags.Revive))
			Destroy(GameObject.FindGameObjectWithTag(Tags.Revive));

	}

	public IEnumerator showReviveMenu(float wait)
	{
		yield return new WaitForSeconds (wait);

		if(!GameObject.FindGameObjectWithTag(Tags.Revive))
			Instantiate((GameObject) Resources.Load(Menus.Revive));

		yield return null;
	}

	public void showGameOverMenu()
	{
		StartCoroutine (showLevelEndMenu (0));
	}

	public IEnumerator showLevelEndMenu(float wait)
	{
		yield return new WaitForSeconds (wait);

		GameAnalytics.NewProgressionEvent (GAProgressionStatus.Complete, "Game World");

		if(!GameObject.FindGameObjectWithTag(Tags.LevelEndMenu))
			Instantiate((GameObject) Resources.Load(Menus.LevelEndMenu));
		
		GameManager.instance.soundState.playSound(SoundController.States.GAMEOVERSOUND);

		yield return null;
	}

	
}
