using UnityEngine;
using System.Collections;

public class ResumeBtnController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void onResumeBtnClick()
	{
		GameManager.Instance.setGameState(GameState.States.GamePlay);
		GameManager.Instance.soundState.playSound(SoundController.States.BTNCLICKSOUND);

	}

}
