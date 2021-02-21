using UnityEngine;
using System.Collections;
using GameAnalyticsSDK;

public class TutorialController : MonoBehaviour 
{
	public GameObject gamePlayCanvas;

	public RectTransform tutorialText;

	void Awake()
	{
		LeanTween.textColor(tutorialText,Color.clear,1).setLoopClamp();
	}

	// Use this for initialization
	void Start () 
	{
		gamePlayCanvas.SetActive(false);
		SharedVariables.isGamePlay = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.anyKeyDown)
		{
			GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start,"Game World");

			gamePlayCanvas.SetActive(true);
			GameManager.Instance.setGameState(GameState.States.GamePlay);
			SpawnManager.Instance.spawnEnemies ();
			SpawnManager.Instance.InitPowerups ();

			Destroy(gameObject);
		}
	}

}
