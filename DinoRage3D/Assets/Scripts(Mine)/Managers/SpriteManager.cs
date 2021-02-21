using UnityEngine;
using System.Collections;
using Gamelogic;

public class SpriteManager :  Singleton<SpriteManager>
{
	public Sprite[] sprites;
	GameObject[] sceneries;

	void Start()
	{
		sceneries = GameObject.FindGameObjectsWithTag(Tags.backgroundScenery);
		changeBirdSprite();

	}
			
	void changeBirdSprite()
	{
		int index = Random.Range(0,sprites.Length);

		for(int i = 0;i < sceneries.Length;i++)
		{
			sceneries[i].GetComponent<SpriteRenderer>().sprite = sprites[index];
		}
	}
}
