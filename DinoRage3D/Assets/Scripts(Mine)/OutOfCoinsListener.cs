using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using com.aeksaekhow.androidnativeplugin;

public class OutOfCoinsListener : MonoBehaviour 
{
	public RectTransform outOfCoinsPanel;


	// Use this for initialization
	void Awake () 
	{
		outOfCoinsPanel.localScale = Vector3.zero;
		LeanTween.scale(outOfCoinsPanel,Vector3.one,1f).setEase(LeanTweenType.easeOutBack);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void clickOnBack()
	{
		Destroy(gameObject);
		GameManager.Instance.soundState.playSound(SoundController.States.BTNCLICKSOUND);

	}

	public void onFreeCoinsBtnClick()
	{
		if(Application.platform.Equals(RuntimePlatform.Android))
		{
			if(AndroidNativePlugin.IsInternetConnected() || AndroidNativePlugin.IsWifiConnected() || AndroidNativePlugin.IsMobileConnected())
			{
				GameManager.Instance.setGameState(GameState.States.VideoAdConfirmation);
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
			GameManager.Instance.setGameState(GameState.States.VideoAdConfirmation);
			GameManager.Instance.soundState.playSound(SoundController.States.BTNCLICKSOUND);
		}
	}
}
