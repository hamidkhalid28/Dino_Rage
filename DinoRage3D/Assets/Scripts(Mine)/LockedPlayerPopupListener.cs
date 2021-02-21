using UnityEngine;
using System.Collections;

public class LockedPlayerPopupListener : MonoBehaviour 
{
	public RectTransform tween_panel;

	// Use this for initialization
	void Awake () 
	{
		tween_panel.localScale = Vector3.zero;
		LeanTween.scale(tween_panel,Vector3.one,1f).setEase(LeanTweenType.easeOutBack);
	}
	
	public void onCancelBtnClick()
	{
		GameManager.Instance.soundState.playSound(SoundController.States.BTNCLICKSOUND);
		Destroy(gameObject);
	}
}
