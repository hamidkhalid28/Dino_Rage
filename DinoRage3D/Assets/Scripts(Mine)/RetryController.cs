using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class RetryController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public void onRetryBtnClick()
	{
		SceneManager.LoadScene(Constants.GAMEPLAY_SCENE);
		GameManager.Instance.soundState.playSound(SoundController.States.BTNCLICKSOUND);

	}
}
