using UnityEngine;
using System.Collections;
using GameAnalyticsSDK;

public class VideoAdConfirmationListener : MonoBehaviour 
{
	public RectTransform VideoAdConfirmationPanel;

	// Use this for initialization
	void Awake () 
	{
		VideoAdConfirmationPanel.localScale = Vector3.zero;
		LeanTween.scale(VideoAdConfirmationPanel,Vector3.one,1f).setEase(LeanTweenType.easeOutBack);
	}
	
	public void onCancelBtnClick()
	{
		GameAnalytics.NewDesignEvent ("Menu:VideoAd Cancelled");
		GameManager.Instance.soundState.playSound(SoundController.States.BTNCLICKSOUND);
		Destroy(gameObject);
	}

	public void onYesBtnClick()
	{
		GameAnalytics.NewDesignEvent ("Menu:VideoAd Requested");

		Constants.reward = Constants.REWARD_TYPES.Coins;

		GameManager.Instance.soundState.playSound(SoundController.States.BTNCLICKSOUND);
		GameManager.Instance.adsManager.showRewardedVideo();
	}
}
