using UnityEngine;
using System.Collections;
using com.aeksaekhow.androidnativeplugin;

public class MainMenuController : MonoBehaviour 
{
	public RectTransform mainPanel;	
	public RectTransform playBtn;
	public RectTransform header;


	void Awake()
	{
		LeanTween.move(mainPanel,Vector3.zero,1f).setEase(LeanTweenType.easeOutQuint);
		LeanTween.scale(playBtn,new Vector3(1f,1f,1f),1).setDelay(2.6f).setLoopPingPong();
		LeanTween.scale(header,new Vector3(0.5f,0.5f,0.5f),2.5f).setDelay(2.6f).setEase(LeanTweenType.easeOutElastic).setLoopPingPong();

	}

	// Use this for initialization
	void Start () 
	{
	}

	public void onPlayBtnClick()
	{
		GameManager.Instance.setGameState(GameState.States.PlayerSelection);
		GameManager.Instance.soundState.playSound(SoundController.States.BTNCLICKSOUND);
	}

	public void onSettingsBtnClick()
	{
		GameManager.Instance.setGameState(GameState.States.Settings);
		GameManager.Instance.soundState.playSound(SoundController.States.BTNCLICKSOUND);
	}

	public void onShareBtnClick()
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

	public void onRateUsBtnClick()
	{
		if(Application.platform.Equals(RuntimePlatform.Android))
		{
			if(AndroidNativePlugin.IsInternetConnected() || AndroidNativePlugin.IsWifiConnected() || AndroidNativePlugin.IsMobileConnected())
			{
				GameManager.Instance.setGameState(GameState.States.RateUs);
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
			GameManager.Instance.setGameState(GameState.States.RateUs);
			GameManager.Instance.soundState.playSound(SoundController.States.BTNCLICKSOUND);
		}
		
	}
}
