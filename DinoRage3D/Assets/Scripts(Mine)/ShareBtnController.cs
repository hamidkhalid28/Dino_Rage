using UnityEngine;
using System.Collections;
using com.aeksaekhow.androidnativeplugin;

public class ShareBtnController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}


	public void onBtnClick()
	{
		if(Application.platform.Equals(RuntimePlatform.Android))
		{
			if(AndroidNativePlugin.IsInternetConnected() || AndroidNativePlugin.IsWifiConnected() || AndroidNativePlugin.IsMobileConnected())
			{
				GameManager.Instance.setGameState(GameState.States.Share);
				GameManager.Instance.soundState.playSound(SoundController.States.BTNCLICKSOUND);
			}
			else
			{
				GameManager.Instance.setGameState(GameState.States.InternetNotConnected);
				GameManager.Instance.soundState.playSound(SoundController.States.BTNCLICKSOUND);

			}
		}
		else
		{
			GameManager.Instance.setGameState(GameState.States.Share);
			GameManager.Instance.soundState.playSound(SoundController.States.BTNCLICKSOUND);
		}

	}
}
