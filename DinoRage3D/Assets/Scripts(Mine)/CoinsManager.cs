using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CoinsManager : MonoBehaviour 
{
	public Text coinsNum;
	private int coins;

	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void updateCoins(int coins)
	{
		if(coinsNum)
		{
			this.coins += coins;
			coinsNum.text = this.coins.ToString();
			Prefs.coins += coins;
		}

	}

}
