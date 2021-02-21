using UnityEngine;
using System.Collections;

public class Caution : MonoBehaviour {

	// Use this for initialization
	void Awake () 
	{

		if(!Prefs.isFirstTime)
		{
			gameObject.SetActive (false);
		}
	}
	

}
