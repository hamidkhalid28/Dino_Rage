using UnityEngine;
using System.Collections;

public class PlayerEffectController : MonoBehaviour 
{
	public GameObject shadow_effect;
	public GameObject fire_effect;
	public GameObject gold_effect;

	public ParticleSystem healing_effect;


	public ParticleSystem right_foot_print;
	public ParticleSystem left_foot_print;




	private PlayerTextureController texture_handler;

	// Use this for initialization
	void Start () 
	{
		texture_handler = GetComponentInChildren<PlayerTextureController>();
		texture_handler.changeTexture();
		showEffect();
	}

	public void showEffect()
	{
		if(Prefs.currentPlayer == 0)
		{
			hideAllEffects();
		}
		else if(Prefs.currentPlayer == 1)
		{
			setGoldEffect();
		}
		else if(Prefs.currentPlayer == 2)
		{
			setShadowEffect();
		}
		else if(Prefs.currentPlayer == 3)
		{
			setFireEffect();
		}

	}
	
	public void setShadowEffect()
	{
		shadow_effect.SetActive(true);
		fire_effect.SetActive(false);
		gold_effect.SetActive(false);
	}

	public void setFireEffect()
	{
//		GameManager.Instance.soundState.playSound(SoundController.States.FIREDINOSOUND);

		shadow_effect.SetActive(false);
		fire_effect.SetActive(true);
		gold_effect.SetActive(false);
	}

	public void setGoldEffect()
	{
//		GameManager.Instance.soundState.playSound(SoundController.States.GOLDENDINOSOUND);

		shadow_effect.SetActive(false);
		fire_effect.SetActive(false);
		gold_effect.SetActive(true);
	}

	public void hideAllEffects()
	{
		shadow_effect.SetActive(false);
		fire_effect.SetActive(false);
		gold_effect.SetActive(false);
	}

	public void playRightFootPrint()
	{
		right_foot_print.Play();
	}

	public void playLeftFoorPrint()
	{
		left_foot_print.Play();

	}

	public void playHealingEffect()
	{
		healing_effect.Play ();
	}
}
