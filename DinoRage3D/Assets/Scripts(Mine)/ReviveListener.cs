using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using GameAnalyticsSDK;

public class ReviveListener : MonoBehaviour 
{
	public RectTransform RevivePanel;
	public Slider progressBar;

	// Use this for initialization
	void Awake () 
	{
		RevivePanel.localScale = Vector3.zero;
		LeanTween.scale(RevivePanel,Vector3.one,1f).setEase(LeanTweenType.easeOutBack);

		LeanTween.value (RevivePanel.gameObject, progressBar.maxValue, progressBar.minValue, 6)
			.setOnUpdate((float val) => {progressBar.value = val; }).onComplete = OnCompleteTween;

	}

	void OnCompleteTween()
	{
		GameManager.Instance.menuManager.showGameOverMenu ();

		Destroy(gameObject);

	}
	
	public void onCancelBtnClick()
	{
		GameAnalytics.NewDesignEvent ("Gameplay:ReviveVideoAd:Cancelled");
		GameManager.Instance.soundState.playSound(SoundController.States.BTNCLICKSOUND);

		if(GameObject.FindGameObjectWithTag(Tags.AdNotAvailable))
		{
			Destroy (GameObject.FindGameObjectWithTag (Tags.AdNotAvailable));
		}

		GameManager.Instance.menuManager.showGameOverMenu ();

		Destroy(gameObject);

	}

	public void onYesBtnClick()
	{
		GameAnalytics.NewDesignEvent ("Gameplay:ReviveVideoAd:Requested");

		Constants.reward = Constants.REWARD_TYPES.Revival;

		GameManager.Instance.soundState.playSound(SoundController.States.BTNCLICKSOUND);
		GameManager.Instance.adsManager.showRewardedVideo();


	}
}
