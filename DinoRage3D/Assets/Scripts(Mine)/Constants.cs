using UnityEngine;
using System.Collections;

public class Constants : MonoBehaviour 
{
	public static bool isDebug = true;
	public static float cameraDistanceFromPlayer = 10.05f;
	public static float cameraHeightFromPlayer = 4.8f;
	public const int REWARD_PER_KILL = 12;

	public const int BirdPrice = 200;
	public const int UPDATE_STATUS_REWARD = 200;
	public static int[] PlayerPrices = new int[] {0,5000,12000,20000};
	public const string ANDROID_APP_URL = "";
	public const string iOSApp_URL = null;
	public const string AndroidBannerID = "";
	public const string AndroidHouseAdID = "";
	public const string AndroidInterstatialID = "";
	public const string AndroidRewardedVideoID = "";
	public const string Game_Name = "Dino Rage 3D";
	public const string NotificationMessage = "Come Back!! Dino is ready for action!!";
	public const string GAME_BUNDLE_ID = "";
	public const string ADCOLONY_APP_ID = "";
	public const string ADCOLONY_ZONE_ID = "";
	public const string ANDROID_APP_WEB_URL = "";
	public const string ANDROID_TWEET_BITLY_LINK = "";
	public const string ANDROID_TEST_DEVICE_ID = "";
	public const string SPLASH_SCENE = "Splash";
	public const string MAINMENU_SCENE = "MainMenu";
	public const string GAMEPLAY_SCENE = "Environment";
	public const string VIRTUAL_CURRENCY = "Coin";

	public static readonly string[] PRAISE_TEXT = {"Excellent!!","Awesome!!!","Fantastic!!","Magnificent!!","Exquisite!!!"};

	public enum PREY_TYPES
	{
		Human,
		Soldier,
	};

	public enum REWARD_TYPES
	{
		Coins,
		Revival,
	};

	public static REWARD_TYPES reward;


	#region TEST
	public const string AndroidTestInterstatialID = "";
	public const string AndroidTestRewardedID = "";
	#endregion
}
