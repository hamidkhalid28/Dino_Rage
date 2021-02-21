using UnityEngine;
using System.Collections;

public class PauseBtnController : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void onPauseBtnClick()
	{
		GameManager.Instance.setGameState(GameState.States.Pause);
		GameManager.Instance.soundState.playSound(SoundController.States.BTNCLICKSOUND);
	}
}
