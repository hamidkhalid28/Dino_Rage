using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuCoinsController : MonoBehaviour 
{

	public Text coinsNum;
	
	public Text CoinsNum {
		get {
			return coinsNum;
		}
		set {
			coinsNum = value;
		}
	}
	
	// Use this for initialization
	void Start () 
	{
		showCoins();
	}
	
	public void showCoins()
	{
		coinsNum.text = Prefs.coins.ToString();
		
	}
}
