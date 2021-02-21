using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using GameAnalyticsSDK;

public class ConfirmPurchaseListener : MonoBehaviour 
{
	public RectTransform confirmPurchasePanel;

	// Use this for initialization
	void Awake() 
	{
		confirmPurchasePanel.localScale = Vector3.zero;
		LeanTween.scale(confirmPurchasePanel,Vector3.one,1f).setEase(LeanTweenType.easeOutBack);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void clickOnOk()
	{
		GameManager.Instance.soundState.playSound(SoundController.States.PURCHASESOUND);

		GameAnalytics.NewResourceEvent (GAResourceFlowType.Sink,Constants.VIRTUAL_CURRENCY, Constants.PlayerPrices [Prefs.currentPlayer],Constants.VIRTUAL_CURRENCY,Constants.VIRTUAL_CURRENCY);

		PlayerSelectionMenuController controller =  GameObject.FindGameObjectWithTag(Tags.PlayerSelectionMenu).GetComponent<PlayerSelectionMenuController>();
		MenuCoinsController coinsController = GameObject.FindGameObjectWithTag(Tags.PlayerSelectionMenu).GetComponent<MenuCoinsController>();

		Prefs.coins -= Constants.PlayerPrices[Prefs.currentPlayer];
		Prefs.playerUnlockArray[Prefs.currentPlayer] = true;

		controller.setCoinsUI();
		controller.setSelectedText();
		controller.setUnlockButton();
		coinsController.showCoins();

		Prefs.savePrefs();


		Destroy(gameObject);

	}

	public void clickOnCancel()
	{
		Destroy(gameObject);
		GameManager.Instance.soundState.playSound(SoundController.States.BTNCLICKSOUND);
	}
}
