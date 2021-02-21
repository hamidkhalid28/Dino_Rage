using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BackBtnController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void onBtnClick()
	{
		GameManager.Instance.soundState.playSound(SoundController.States.BTNCLICKSOUND);

		Destroy(gameObject);
	}
}
