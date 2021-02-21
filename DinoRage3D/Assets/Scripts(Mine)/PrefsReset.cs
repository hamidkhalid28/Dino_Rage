using UnityEngine;
using System.Collections;
using Gamelogic;

public class PrefsReset : MonoBehaviour {

	// Use this for initialization
	void Awake () 
	{
		GLPlayerPrefs.DeleteAll();
	}

}
