using UnityEngine;
using System.Collections;

public class GameFlowController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void GameOver()
	{
		GameManager.Instance.setGameState(GameState.States.GameOver);
	}
}
