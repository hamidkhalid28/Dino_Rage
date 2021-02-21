using UnityEngine;
using System.Collections;
using Gamelogic;

public class GameOverManager : Singleton<GameOverManager>
{
	private GameObject player;


	void Start()
	{
		player = GameObject.FindGameObjectWithTag(Tags.player);
	}

	public void gameOver()
	{
		if(SharedVariables.isGamePlay)
		{
			player.GetComponent<InstantiateStarsEffect>().instantiateStarEffect();
			//Camera.main.GetComponent<ChaseCamera>().stopCamera();
			player.GetComponent<Animator>().enabled = false;
			GameManager.Instance.setGameState(GameState.States.GameOver);
		}

	}
}
