using UnityEngine;
using System.Collections;
using Gamelogic;

public class PrefsLoader : MonoBehaviour {

	// Use this for initialization
	void Awake () 
	{
		LoadPrefs ();
	}

	void LoadPrefs()
	{
		if(GLPlayerPrefs.HasKey(null,PrefKeys.playerUnlockArray))
		{
			Prefs.loadPrefs();
		}
		else
		{
			if(Prefs.isFirstTime)
			{
				Prefs.isFirstTime = false;
				Debug.LogError ("Here");
			}

			Prefs.savePrefs();
		}
	}


}
