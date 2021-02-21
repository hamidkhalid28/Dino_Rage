using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using GameAnalyticsSDK;


public class LevelEndMenuController : MonoBehaviour 
{
	private GameObject gamePlayCanvas;
	public Text resultScoreText;
	public Text resultHighScoreText;
	public Text resultCoinsText;
	
	private Text gameplayScore;

	// Use this for initialization
	void Start () 
	{
		getAllRefrences();

		if(GameObject.FindGameObjectWithTag(Tags.Hud))
			Destroy(GameObject.FindGameObjectWithTag(Tags.Hud));

		setAllValues();
	}


	void getAllRefrences()
	{
		gameplayScore = GameObject.FindGameObjectWithTag(Tags.ScoreNum).GetComponent<Text>();

	}

	void setAllValues()
	{
		if(Prefs.highScore <  int.Parse(gameplayScore.text))
		{
			Prefs.highScore = int.Parse(gameplayScore.text);
			GameAnalytics.NewDesignEvent ("Gameplay:High Score", (float)Prefs.highScore);

		}

		resultHighScoreText.text = Prefs.highScore.ToString();
		resultScoreText.text = gameplayScore.text;
		resultCoinsText.text = Prefs.coins.ToString();

		Prefs.savePrefs();
	}
}
