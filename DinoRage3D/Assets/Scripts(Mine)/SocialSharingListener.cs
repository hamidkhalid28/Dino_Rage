using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SocialSharingListener : MonoBehaviour 
{
	public RectTransform socialSharingPanel;

	public RectTransform coinsAddedText;

	// Use this for initialization
	void Awake () 
	{
		socialSharingPanel.localScale = Vector3.zero;
		LeanTween.scale(socialSharingPanel,new Vector3(0.8f,0.8f,0.8f),1f).setEase(LeanTweenType.easeOutBack);
	}


	public void showCoinsAddedText()
	{
		LeanTween.alphaText(coinsAddedText,1,0.4f);

		LeanTween.textColor(coinsAddedText,Color.red,0.4f).setDelay(0.4f);
		LeanTween.textColor(coinsAddedText,Color.blue,0.4f).setDelay(0.8f);
		LeanTween.textColor(coinsAddedText,Color.white,0.4f).setDelay(1.2f);
		LeanTween.textColor(coinsAddedText,Color.clear,0.4f).setDelay(1.6f);

	}

}
