using UnityEngine;
using System.Collections;
using GoogleMobileAds.Api;
using GoogleMobileAds;
using System;
using GameAnalyticsSDK;

public class AdsEventHandler : MonoBehaviour 
{
	AdsManager adsManager;


	// Use this for initialization
	void Start () 
	{
		adsManager = GetComponent<AdsManager>();


					// Register banner ads.

//		adsManager.banner.AdLoaded += HandleBannerAdLoaded;
//		// Called when an ad request failed to load.
//		adsManager.banner.AdFailedToLoad += HandleBannerAdFailedToLoad;
//		// Called when an ad is clicked.
//		adsManager.banner.AdOpened += HandleBannerAdOpened;
//		// Called when the user is about to return to the app after an ad click.
//		adsManager.banner.AdClosing += HandleBannerAdClosing;
//		// Called when the user returned from the app after an ad click.
//		adsManager.banner.AdClosed += HandleBannerAdClosed;
//		// Called when the ad click caused the user to leave the application.
//		adsManager.banner.AdLeftApplication += HandleBannerAdLeftApplication;

				// Register interstatial ads.

		if(adsManager.interstitial == null)
		{
			adsManager.RequestInterstitial ();
		}

		if(adsManager.rewardBasedVideo == null)
		{
			adsManager.rewardBasedVideo = RewardBasedVideoAd.Instance;

		}

		adsManager.interstitial.OnAdLoaded += HandleInterstitialAdLoaded;
		// Called when an ad request failed to load.
		adsManager.interstitial.OnAdFailedToLoad += HandleInterstitialAdFailedToLoad;
		// Called when an ad is clicked.
//		adsManager.interstitial.AdOpened += HandleInterstitialAdOpened;
		// Called when the user is about to return to the app after an ad click.
//		adsManager.interstitial.AdClosing += HandleInterstitialAdClosing;
		// Called when the user returned from the app after an ad click.
		adsManager.interstitial.OnAdClosed += HandleInterstitialAdClosed;
		// Called when the ad click caused the user to leave the application.
		adsManager.interstitial.OnAdLeavingApplication += HandleInterstitialAdLeftApplication;

//		AdColony.OnV4VCResult += onRewardedVideoAdResult;

		adsManager.rewardBasedVideo.OnAdRewarded += onRewardedVideoAdResult;
		adsManager.rewardBasedVideo.OnAdFailedToLoad += onRewardedVideoAdFailedToLoad;
		adsManager.rewardBasedVideo.OnAdClosed += onRewardedVideoAdClosed;
	}
	
	public void HandleBannerAdLoaded(object sender, EventArgs args)
	{
		if(GameManager.Instance.getGameState().Equals(GameState.States.MainMenu))
			adsManager.showBanner();

		print("HandleAdLoaded event received.");
		// Handle the ad loaded event.
	}

	public void HandleBannerAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
	{
		print("HandleAdLoaded event received.");
		// Handle the ad loaded event.
	}

	public void HandleBannerAdOpened(object sender, EventArgs args)
	{
		adsManager.Banner.Destroy();
		adsManager.RequestBanner(AdPosition.Top);

		print("HandleAdLoaded event received.");
		// Handle the ad loaded event.
	}

	public void HandleBannerAdClosing(object sender, EventArgs args)
	{
		print("HandleAdLoaded event received.");
		// Handle the ad loaded event.
	}

	public void HandleBannerAdClosed(object sender, EventArgs args)
	{
		print("HandleAdLoaded event received.");
		// Handle the ad loaded event.

		adsManager.Banner.Destroy();
		adsManager.RequestBanner(AdPosition.Top);
	}

	public void HandleBannerAdLeftApplication(object sender, EventArgs args)
	{
		print("HandleAdLoaded event received.");
		// Handle the ad loaded event.
	}

/************************* Interstatial Ads *********************************/

	public void HandleInterstitialAdLoaded(object sender, EventArgs args)
	{
		print("HandleAdLoaded event received.");



		// Handle the ad loaded event.
	}

	public void HandleInterstitialAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
	{
		adsManager.Interstitial.Destroy();
		adsManager.RequestInterstitial();



		print("HandleAdLoaded event received.");
		// Handle the ad loaded event.
	}

	public void HandleInterstitialAdOpened(object sender, EventArgs args)
	{
		print("HandleAdLoaded event received.");
		// Handle the ad loaded event.
	}

	public void HandleInterstitialAdClosing(object sender, EventArgs args)
	{
		print("HandleAdLoaded event received.");
		// Handle the ad loaded event.
	}

	public void HandleInterstitialAdClosed(object sender, EventArgs args)
	{
		print("HandleAdLoaded event received.");
		// Handle the ad loaded event.

		adsManager.Interstitial.Destroy();
		adsManager.RequestInterstitial();
	}

	public void HandleInterstitialAdLeftApplication(object sender, EventArgs args)
	{
		print("HandleAdLoaded event received.");
		// Handle the ad loaded event.
	}

	public void onRewardedVideoAdResult(object sender, Reward args)
	{
		if (Constants.reward.Equals (Constants.REWARD_TYPES.Coins)) 
		{
			GameAnalytics.NewResourceEvent(GAResourceFlowType.Source,Constants.VIRTUAL_CURRENCY,(float) args.Amount,Constants.VIRTUAL_CURRENCY,Constants.VIRTUAL_CURRENCY);

			string type = args.Type;
			double amount = args.Amount;

			Prefs.coins += (int)amount;
			Prefs.savePrefs ();
		}
		else
		{
			GameAnalytics.NewDesignEvent ("Gameplay:Revived");
			GameManager.Instance.setGameState (GameState.States.GamePlay);

			if(GameObject.FindGameObjectWithTag(Tags.player))
			{
				GameObject.FindGameObjectWithTag (Tags.player).GetComponent<PlayerController> ().Revive ();
			}

		}

		if(GameObject.FindGameObjectWithTag(Tags.PlayerSelectionMenu))
		{
			GameObject.FindGameObjectWithTag(Tags.PlayerSelectionMenu).GetComponent<MenuCoinsController>().showCoins();
		}

		if(GameObject.FindGameObjectWithTag(Tags.VideoAdConfirmation))
		{
			Destroy(GameObject.FindGameObjectWithTag(Tags.VideoAdConfirmation));
		}

		if(GameObject.FindGameObjectWithTag(Tags.OutOfCoins))
		{
			Destroy(GameObject.FindGameObjectWithTag(Tags.OutOfCoins));
		}

		adsManager.RequestRewardedVideo ();
			
	}

	public void onRewardedVideoAdFailedToLoad(object sender, EventArgs args)
	{
		adsManager.RequestRewardedVideo ();
	}

	public void onRewardedVideoAdClosed(object sender, EventArgs args)
	{
		adsManager.RequestRewardedVideo ();
	}

//	public void onRewardedVideoAdResult(bool success, string name, int amount)
//	{
//		if(success)
//		{
//			Prefs.coins += amount;
//			Prefs.savePrefs();
//
//			if(GameObject.FindGameObjectWithTag(Tags.PlayerSelectionMenu))
//			{
//				GameObject.FindGameObjectWithTag(Tags.PlayerSelectionMenu).GetComponent<MenuCoinsController>().showCoins();
//			}
//		}
//		else
//		{
//			GameManager.Instance.setGameState(GameState.States.AdNotAvaiableState);
//		}
//
//		if(GameObject.FindGameObjectWithTag(Tags.VideoAdConfirmation))
//		{
//			Destroy(GameObject.FindGameObjectWithTag(Tags.VideoAdConfirmation));
//		}
//
//		if(GameObject.FindGameObjectWithTag(Tags.OutOfCoins))
//		{
//			Destroy(GameObject.FindGameObjectWithTag(Tags.OutOfCoins));
//		}
//	}

}
