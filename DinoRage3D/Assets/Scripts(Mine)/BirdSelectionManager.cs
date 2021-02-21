using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BirdSelectionManager : MonoBehaviour 
{
	public Sprite[] birds;
	public RuntimeAnimatorController[] bird_controller;

	public GameObject[] bird_sprites;

	public GameObject[] prices;

	public GameObject[] selected_panels;

	private int bird_to_purchase_index;


	public int iteration = 1;
	private int numOfBirds = 4;
	GameObject player;

	// Use this for initialization
	void Start () 
	{
		player = GameObject.FindGameObjectWithTag(Tags.player);
		showBirds();
			
		player.GetComponent<SpriteRenderer>().sprite = birds[Prefs.selected_bird];
		player.GetComponent<Animator>().runtimeAnimatorController = bird_controller[Prefs.selected_bird];

	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public void clickOnBird(int number)
	{
		int num = number + ((iteration-1) * numOfBirds);		//Get the number of index to change the appearence of player.

		if(checkBirdUnlocked(num-1))
		{
			prices[number-1].gameObject.SetActive(false);
			selected_panels[number-1].gameObject.SetActive(true);

			for(int i = 0;i < numOfBirds;i++)
			{
				if(i != number-1)
					selected_panels[i].gameObject.SetActive(false);
			}

			Prefs.selected_bird = num-1;
			player.GetComponent<SpriteRenderer>().sprite = birds[num-1];
			player.GetComponent<Animator>().runtimeAnimatorController = bird_controller[num-1];
			Debug.LogError("fafas");
			Prefs.savePrefs();
		}
		else
		{
			if(Prefs.coins >= Constants.BirdPrice)
			{
				bird_to_purchase_index = num-1;
				Instantiate(Resources.Load(Menus.ConfirmPurchase));			
				GetComponent<GraphicRaycaster>().enabled = false;

			}
			else
			{
				Instantiate(Resources.Load(Menus.OutOfCoins));
				GetComponent<GraphicRaycaster>().enabled = false;

			}
		}
	}

	public void showBirds()
	{
		int index = numOfBirds * (iteration-1);

		for(int i = index;i<index+numOfBirds;i++)
		{
			bird_sprites[i-index].GetComponent<Image>().sprite = birds[i]; 
			selected_panels[i-index].gameObject.SetActive(false);
			prices[i-index].gameObject.SetActive(false);

			
			if(checkBirdUnlocked(i))
			{
				prices[i-index].gameObject.SetActive(false);


				if(Prefs.selected_bird == i)
				{
					selected_panels[i-index].gameObject.SetActive(true);
				}
			}
			else
			{
				prices[i-index].gameObject.SetActive(true);
				
			}
		}
	}

	public void unlockBird()
	{
		Prefs.coins -= Constants.BirdPrice;
		Prefs.isBirdUnlocked[bird_to_purchase_index] = true;
		Prefs.selected_bird = bird_to_purchase_index;
		Prefs.savePrefs();
		player.GetComponent<SpriteRenderer>().sprite = birds[bird_to_purchase_index];
		player.GetComponent<Animator>().runtimeAnimatorController = bird_controller[bird_to_purchase_index];
		GetComponent<MenuCoinsController>().coinsNum.text = Prefs.coins.ToString();
		showBirds();
	}

	private bool checkBirdUnlocked(int index)
	{
		if(Prefs.isBirdUnlocked[index])
			return true;

		return false;
	}

}
