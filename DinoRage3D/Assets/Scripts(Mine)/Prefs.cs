using UnityEngine;
using System.Collections;
using Gamelogic;
using AvoEx;

public class Prefs : MonoBehaviour
{
	public static bool[] isBirdUnlocked = new bool[12]{true,false,false,false,false,false,false,false,false,false,false,false};
	public static int coins = 100;
	public static int selected_bird = 0;
	public static int highScore = 0;
	public static bool isSound = true;
	public static string notificationTime = null;
	public static bool[] playerUnlockArray = new bool[4]{true,false,false,false};
	public static int currentPlayer = 0;
	public static bool isFirstTime = true;

		
	public static void savePrefs()
	{
		GLPlayerPrefs.SetString(null,PrefKeys.notificationTime,notificationTime);
		GLPlayerPrefs.SetInt(null,PrefKeys.coins,coins);
		GLPlayerPrefs.SetInt(null,PrefKeys.highScore,highScore);
		GLPlayerPrefs.SetBool(null,PrefKeys.isSound,isSound);
		GLPlayerPrefs.SetInt(null,PrefKeys.currentPlayer,currentPlayer);
		GLPlayerPrefs.SetBoolArray(null,PrefKeys.playerUnlockArray,playerUnlockArray);
		GLPlayerPrefs.SetBool(null,PrefKeys.isFirstTime,isFirstTime);


		GLPlayerPrefs.Save();


	}

	public static void loadPrefs()
	{
		notificationTime = GLPlayerPrefs.GetString(null,PrefKeys.notificationTime);
		coins = GLPlayerPrefs.GetInt(null,PrefKeys.coins);
		highScore = GLPlayerPrefs.GetInt(null,PrefKeys.highScore);
		isSound = GLPlayerPrefs.GetBool(null,PrefKeys.isSound);
		currentPlayer =  GLPlayerPrefs.GetInt(null,PrefKeys.currentPlayer);
		playerUnlockArray = GLPlayerPrefs.GetBoolArray(null,PrefKeys.playerUnlockArray);
		isFirstTime = GLPlayerPrefs.GetBool(null,PrefKeys.isFirstTime);

	}

	public void ResetPrefs()
	{
		GLPlayerPrefs.DeleteAll ();
	}

}
