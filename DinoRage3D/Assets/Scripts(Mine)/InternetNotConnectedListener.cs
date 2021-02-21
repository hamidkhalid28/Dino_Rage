using UnityEngine;
using System.Collections;

public class InternetNotConnectedListener : MonoBehaviour {

	public RectTransform internetNotConnectedPanel;

	// Use this for initialization
	void Awake () 
	{
		internetNotConnectedPanel.localScale = Vector3.zero;
		LeanTween.scale(internetNotConnectedPanel,Vector3.one,1f).setEase(LeanTweenType.easeOutBack);
	}
	
	public void clickOnBack()
	{
		Destroy(gameObject);
		GameManager.Instance.soundState.playSound(SoundController.States.BTNCLICKSOUND);

	}
}
