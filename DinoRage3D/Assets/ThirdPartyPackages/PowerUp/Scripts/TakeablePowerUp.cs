using UnityEngine;
using System.Collections;
using YounGenTech.HealthScript;

public class TakeablePowerUp : MonoBehaviour {
	CustomizablePowerUp customPowerUp;

	int healing_power;

	void Start() 
	{
		healing_power = 20;

		customPowerUp = (CustomizablePowerUp)transform.parent.gameObject.GetComponent<CustomizablePowerUp>();
		//this.audio.clip = customPowerUp.pickUpSound;
	}

	void OnTriggerEnter (Collider collider) 
	{
		if(collider.tag == Tags.player) 
		{
			GameManager.Instance.soundState.playSound (SoundController.States.HEAL);
			collider.GetComponent<Health> ().Heal (new HealthEvent (transform.parent.gameObject, healing_power));
			collider.GetComponent<PlayerEffectController> ().playHealingEffect ();
			Lean.LeanPool.Despawn(transform.parent.gameObject);
		}
	}
}
