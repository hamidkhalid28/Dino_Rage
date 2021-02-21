using UnityEngine;
using System.Collections;

public class AdNotAvailableListener : MonoBehaviour 
{
	public RectTransform adNotAvailablePanel;

	// Use this for initialization
	void Awake () 
	{
		adNotAvailablePanel.localScale = Vector3.zero;
		LeanTween.scale(adNotAvailablePanel,Vector3.one,1f).setEase(LeanTweenType.easeOutBack);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void clickOnBack()
	{
		Destroy(gameObject);
		GameManager.Instance.soundState.playSound(SoundController.States.BTNCLICKSOUND);

	}
}
