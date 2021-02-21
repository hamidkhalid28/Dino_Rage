using UnityEngine;
using System.Collections;
using Gamelogic;

public class SoundController : Singleton<SoundController> 
{
	public enum States
	{
		BTNCLICKSOUND,
		GAMEOVERSOUND,
		GAMEPLAYSOUND,
		MENUSOUND,
		COINSPICKUPSOUND,
		BIRDSLYINGSOUND,
		PURCHASESOUND,
		MENUAPPEARSOUND,
		LOADING,
		HUMANDEATH,
		RUNSOUND,
		METALHURDLEHIT,
		WOODHURDLEHIT,
		FIREDINOSOUND,
		ROAR,
		SELECTIONCLICK,
		GOLDENDINOSOUND,
		SCREAM,
		HEAL,
		EAT,
		JUMP
	}

	AudioSource fireAudioSource;


	public void playSound(SoundController.States state)
	{
		if(state.Equals(SoundController.States.BTNCLICKSOUND))
		{
			SoundManager.PlaySFX("Click");
		}
		else if(state.Equals(SoundController.States.COINSPICKUPSOUND))
		{
			SoundManager.PlaySFX("Coin_Pick_Up_03");
		}
		else if(state.Equals(SoundController.States.PURCHASESOUND))
		{
			SoundManager.PlaySFX("Buy");
		}
		else if(state.Equals(SoundController.States.LOADING))
		{
			SoundManager.PlaySFX("Loading");
		}
		else if(state.Equals(SoundController.States.HUMANDEATH))
		{
			SoundManager.PlaySFX("MaleDeath");
		}
		else if(state.Equals(SoundController.States.RUNSOUND))
		{
			SoundManager.PlaySFX("Run");
		}
		else if(state.Equals(SoundController.States.METALHURDLEHIT))
		{
			SoundManager.PlaySFX("MetalHurdleHit");
		}
		else if(state.Equals(SoundController.States.FIREDINOSOUND))
		{
			SoundManager.PlaySFX("FireDino");
		}
		else if(state.Equals(SoundController.States.ROAR))
		{
			SoundManager.PlaySFX("Roar");
		}
		else if(state.Equals(SoundController.States.SELECTIONCLICK))
		{
			SoundManager.PlaySFX("SelectionClick");
		}
		else if(state.Equals(SoundController.States.GOLDENDINOSOUND))
		{
			SoundManager.PlaySFX("GoldenDino");
		}
		else if(state.Equals(SoundController.States.MENUAPPEARSOUND))
		{
			SoundManager.PlaySFX("MenuAppear");
		}
		else if(state.Equals(SoundController.States.GAMEOVERSOUND))
		{
			SoundManager.PlaySFX("GameOver");
		}
		else if(state.Equals(SoundController.States.WOODHURDLEHIT))
		{
			SoundManager.PlaySFX("WoodHit");
		}
		else if(state.Equals(SoundController.States.SCREAM))
		{
//			SoundManager.PlaySFX("MaleScream");
		}
		else if(state.Equals(SoundController.States.EAT))
		{
			SoundManager.PlaySFX("Eat");
		}
		else if(state.Equals(SoundController.States.HEAL))
		{
			SoundManager.PlaySFX("Heal");
		}
		else if(state.Equals(SoundController.States.JUMP))
		{
			SoundManager.PlaySFX("Jump");
		}
	}

	public void stopBGMusic()
	{
		SoundManager.StopMusic();
	}

}
