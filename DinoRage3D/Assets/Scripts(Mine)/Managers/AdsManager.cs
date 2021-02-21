using UnityEngine;
using System.Collections;
using GoogleMobileAds;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using Gamelogic;
using com.aeksaekhow.androidnativeplugin;



public class AdsManager :  Singleton<AdsManager>
{
	public AdRequest adRequester;

	public BannerView banner;

	public InterstitialAd interstitial;

	public RewardBasedVideoAd rewardBasedVideo;

	private static  bool created = false;

	public BannerView Banner 
	{
		get {return banner;}
	}
	
	public InterstitialAd Interstitial 
	{
		get {return interstitial;}
	}

	// Use this for initialization
	void Start () 
	{
//		AdColony.Configure
//		(
//			"version:1.0,store:google", // Arbitrary app version and Android app store declaration.
//			Constants.ADCOLONY_APP_ID,   // ADC App ID from adcolony.com
//			Constants.ADCOLONY_ZONE_ID // A zone ID from adcolony.com
//		);

		rewardBasedVideo = RewardBasedVideoAd.Instance;

//		RequestBanner(AdPosition.Bottom);

	}

#region OBSERVER_BEHAVIOR
	
	void OnEnable()
	{
		GameManager.Instance.OnMainMenuState += onMainMenuState;
		GameManager.Instance.OnGameOverState += OnGameOverState;
		GameManager.Instance.OnGamePlayState += OnGamePlayState;
		GameManager.Instance.OnPlayerSelectionState += OnPlayerSelectionState;

		
	}
	
	void OnDisable()
	{
		GameManager.Instance.OnMainMenuState -= onMainMenuState;
		GameManager.Instance.OnGameOverState -= OnGameOverState;
		GameManager.Instance.OnGamePlayState -= OnGamePlayState;
		GameManager.Instance.OnPlayerSelectionState -= OnPlayerSelectionState;


	}

	void onMainMenuState()
	{
		if(!isInterstatialLoaded())
			RequestInterstitial();
//		else
//			showInterstatial();

//		banner.Show();

	}

	
	void OnGameOverState()
	{
//		if(!isInterstatialLoaded())
//		{
//			RequestInterstitial();
//		}
//		else
//		{
//			showInterstatial();
//		}

	}

	void OnPlayerSelectionState()
	{
//		banner.Hide();
	}

	void OnGamePlayState()
	{
		if(!isInterstatialLoaded())
		{
			RequestInterstitial();
		}

//		banner.Hide();
	}
	
#endregion

	public bool isInterstatialLoaded()
	{
		if(null == interstitial)
		{
			return false;
		}

		if (interstitial.IsLoaded()) 
		{
			return true;
		}

		return false;


	}

	public void RequestBanner(AdPosition position)
	{
		if(position == null)
			position = AdPosition.Top;
//
//		// Create a 320x50 banner at the top of the screen.
		banner = new BannerView(Constants.AndroidHouseAdID, AdSize.Banner, position);
//		// Create an empty ad request.
		adRequester = new AdRequest.Builder().
				Build();

//		// Load the banner with the request.
		banner.LoadAd(adRequester);
	}
	public void RequestInterstitial()
	{
		// Initialize an InterstitialAd.
//		interstitial = new InterstitialAd(Constants.AndroidTestInterstatialID);

		interstitial = new InterstitialAd(Constants.AndroidInterstatialID);

		// Create an empty ad request.
		adRequester = new AdRequest.Builder().
//			AddTestDevice(Constants.ANDROID_TEST_DEVICE_ID).
				Build();


		// Load the interstitial with the request.
		interstitial.LoadAd(adRequester);
	}

	public void RequestRewardedVideo()
	{
		if(rewardBasedVideo == null)
		{
			rewardBasedVideo = RewardBasedVideoAd.Instance;
		}

		if (!rewardBasedVideo.IsLoaded ()) 
		{
			AdRequest request = new AdRequest.Builder ().
				Build ();

//			rewardBasedVideo.LoadAd (request, Constants.AndroidTestRewardedID);

			rewardBasedVideo.LoadAd (request, Constants.AndroidRewardedVideoID);

		}
	}
		

	public void showInterstatial()
	{
		if(isInterstatialLoaded())
			interstitial.Show();
		
	}

	public void showBanner()
	{
		if(null == banner)
			RequestBanner(AdPosition.Top);

		banner.Show();
	}

	public void showRewardedVideo()
	{
		if (rewardBasedVideo.IsLoaded ()) 
		{
			rewardBasedVideo.Show ();
		}
		else
		{
			if(!AndroidNativePlugin.IsInternetConnected() && !AndroidNativePlugin.IsWifiConnected() && !AndroidNativePlugin.IsMobileConnected())
			{
				GameManager.Instance.setGameState(GameState.States.InternetNotConnected);
			}
			else
			{
				GameManager.Instance.setGameState(GameState.States.AdNotAvaiableState);

			}

			RequestRewardedVideo ();


		}
	}

	public void hideBanner()
	{
		banner.Hide();
	}

//	public void showRewardedVideo()
//	{
//		if(AdColony.IsV4VCAvailable(Constants.ADCOLONY_ZONE_ID))
//		{
//			AdColony.ShowV4VC(true,Constants.ADCOLONY_ZONE_ID);
//		}
//		else
//		{
//			if(!AndroidNativePlugin.IsInternetConnected() && !AndroidNativePlugin.IsWifiConnected() && !AndroidNativePlugin.IsMobileConnected())
//			{
//				GameManager.Instance.setGameState(GameState.States.InternetNotConnected);
//			}
//			else
//			{
//				GameManager.Instance.setGameState(GameState.States.AdNotAvaiableState);
//			}
//		}
//	}
}
