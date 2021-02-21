using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using com.aeksaekhow.androidnativeplugin;
using GameAnalyticsSDK;

[System.Serializable]
public class PlayerData
{
	public Text coinsText;
	public Text player_name;
	public Image coinsImage;
}

public class PlayerSelectionMenuController : MonoBehaviour 
{
	public Button unlockBtn;
	private PlayerTextureController textureController;
	private PlayerEffectController effectController;

	public PlayerData[] data;

	public RectTransform playerSelectionPanel;
	public RectTransform playBtn;

	// Use this for initialization
	void Awake () 
	{
		LeanTween.move(playerSelectionPanel,Vector3.zero,1f).setEase(LeanTweenType.easeOutQuint);
		LeanTween.scale(playBtn,new Vector3(1f,1f,1f),1).setDelay(2.6f).setLoopPingPong();

		unlockBtn.interactable = false;
		effectController = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<PlayerEffectController>();
		textureController = GameObject.FindGameObjectWithTag(Tags.player).GetComponentInChildren<PlayerTextureController>();
		textureController.changeTexture();
		effectController.showEffect();
		setUnlockButton();
		setSelectedText();
		setCoinsUI();
	}

	public void onPlayerBtnClick(int playerNum)
	{
		GameManager.Instance.soundState.playSound(SoundController.States.SELECTIONCLICK);

		Prefs.currentPlayer = playerNum;
		textureController.changeTexture();
		effectController.showEffect();
		setUnlockButton();
		setSelectedText();
		setCoinsUI();
	}

	public void onPlayBtnClick()
	{
		if (Prefs.playerUnlockArray [Prefs.currentPlayer]) 
		{
			GameManager.Instance.soundState.stopBGMusic ();
			GameManager.Instance.soundState.playSound (SoundController.States.LOADING);
			GameManager.Instance.setGameState (GameState.States.SwtichScene);
		}
		else
		{
			GameManager.Instance.setGameState(GameState.States.LockedPlayer);
		}
	}

	public void onUnlockBtnClick()
	{
		GameManager.Instance.soundState.playSound(SoundController.States.BTNCLICKSOUND);

		if(Prefs.coins >= Constants.PlayerPrices[Prefs.currentPlayer])
			GameManager.Instance.setGameState(GameState.States.PurchaseConfirmation);
		else
			GameManager.Instance.setGameState(GameState.States.OutOfCoins);

	}

	public void setUnlockButton()
	{
		if(!Prefs.playerUnlockArray[Prefs.currentPlayer])
		{
			unlockBtn.interactable = true;
		}
		else
		{
			unlockBtn.interactable = false;
		}
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
			GameAnalytics.NewDesignEvent ("Menu:Free Coins Button Click");

			GameManager.Instance.setGameState(GameState.States.VideoAdConfirmation);
			GameManager.Instance.soundState.playSound(SoundController.States.BTNCLICKSOUND);
		}
	}

	public void onBackBtnClick()
	{
		GameManager.Instance.soundState.playSound(SoundController.States.BTNCLICKSOUND);
		GameManager.Instance.setGameState(GameState.States.MainMenu);

	}

	public void setSelectedText()
	{

		for(int i = 0;i < data.Length;i++)
		{
			if(i == Prefs.currentPlayer)
			{
				data [i].player_name.color = Color.red;

			}
			else
			{
				data [i].player_name.color = Color.black;

			}

		}

	}

	public void setCoinsUI()
	{
		for(int i = 0;i < Prefs.playerUnlockArray.Length;i++)
		{
			if(Prefs.playerUnlockArray[i])
			{
				data[i].coinsText.enabled = false;
				data[i].coinsImage.enabled = false;
			}
		}
	}
}
