using UnityEngine;
using System.Collections;

public class AdDisplay : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void showAd()
	{
		if(GameManager.Instance.adsManager.isInterstatialLoaded())
		{
			GameManager.Instance.adsManager.showInterstatial ();
		}
		else
		{
			GameManager.Instance.adsManager.RequestInterstitial ();
		}
	}

}
