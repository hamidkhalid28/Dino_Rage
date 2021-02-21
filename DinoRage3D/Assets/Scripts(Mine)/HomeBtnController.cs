using UnityEngine;
using System.Collections;

public class HomeBtnController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void goToHome()
	{
		GameManager.Instance.setGameState(GameState.States.SwtichScene);
		GameManager.Instance.soundState.playSound(SoundController.States.BTNCLICKSOUND);

	}
}
