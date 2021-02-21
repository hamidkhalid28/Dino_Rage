using UnityEngine;
using System.Collections;

public class ScreamSound : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
		InvokeRepeating("playScreamSound",3,7);
	}


	void playScreamSound()
	{
		GameManager.Instance.soundState.playSound(SoundController.States.SCREAM);
	}
}
