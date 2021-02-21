using UnityEngine;
using System.Collections;

public class SwitchBirds : MonoBehaviour 
{
	private BirdSelectionManager manager;

	public GameObject nextBtn;
	public GameObject preBtn;



	// Use this for initialization
	void Start () 
	{
		manager = GetComponent<BirdSelectionManager>();
		setNextAndPreButton();
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	void setNextAndPreButton()
	{
		if(manager.iteration >= 3)
			nextBtn.SetActive(false);
		else
			nextBtn.SetActive(true);


		if(manager.iteration <= 1)
			preBtn.SetActive(false);
		else
			preBtn.SetActive(true);
	}


	public void switchNext()
	{
		GameManager.Instance.soundState.playSound(SoundController.States.BTNCLICKSOUND);

		manager.iteration++;
		manager.showBirds();

		setNextAndPreButton();
	}

	public void swtichback()
	{
		GameManager.Instance.soundState.playSound(SoundController.States.BTNCLICKSOUND);

		manager.iteration--;
		manager.showBirds();

		setNextAndPreButton();


	}
}
