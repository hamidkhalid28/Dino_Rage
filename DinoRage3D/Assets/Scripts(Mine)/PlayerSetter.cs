using UnityEngine;
using System.Collections;

public class PlayerSetter : MonoBehaviour 
{
	public Sprite[] birds;
	public RuntimeAnimatorController[] bird_controller;

	// Use this for initialization
	void Start () 
	{
		Invoke("setBird",0.01f);

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void setBird()
	{
		GetComponent<SpriteRenderer>().sprite = birds[Prefs.selected_bird];
		GetComponent<Animator>().runtimeAnimatorController = bird_controller[Prefs.selected_bird];
	}
}
