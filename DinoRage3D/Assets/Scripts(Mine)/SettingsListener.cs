using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SettingsListener : MonoBehaviour 
{
	public Sprite sound_off;
	public Sprite sound_on;
	public GameObject soundBtn;
	private Image soundImage;

	public RectTransform settingsPanel;

	void Awake()
	{
		settingsPanel.localScale = Vector3.zero;
		LeanTween.scale(settingsPanel,Vector3.one,1f).setEase(LeanTweenType.easeOutBack);
	}
	// Use this for initialization
	void Start () 
	{
		setSound();
	}

	// Update is called once per frame
	void Update () {
	
	}

	public void onBtnClick()
	{
		GameManager.Instance.soundState.playSound(SoundController.States.BTNCLICKSOUND);

		Prefs.isSound = !Prefs.isSound;
		Prefs.savePrefs();
		setSound();
	}

	void setSound()
	{
		if(!soundImage)
			soundImage =  soundBtn.GetComponent<Image>();


		if(Prefs.isSound)
		{
			soundImage.sprite = sound_off;
			GameManager.Instance.soundManager.GetComponent<SoundManager>().muted = false;
		}
		else
		{
			soundImage.sprite = sound_on;
			GameManager.Instance.soundManager.GetComponent<SoundManager>().muted = true;
		}
	}

}
