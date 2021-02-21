using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerSoundsController : MonoBehaviour 
{
	
	public void playRunSound()
	{
		GameManager.Instance.soundState.playSound(SoundController.States.RUNSOUND);
	}

	public void playRoarSound()
	{
//		if(SceneManager.GetActiveScene().buildIndex == 0)
//		{
//			Camera.main.GetComponent<MainMenuCameraController>().shakeCamera();
//		}
		
		GameManager.Instance.soundState.playSound(SoundController.States.ROAR);
	}

	public void playJumpSound()
	{
		GameManager.Instance.soundState.playSound(SoundController.States.JUMP);
	}
}
