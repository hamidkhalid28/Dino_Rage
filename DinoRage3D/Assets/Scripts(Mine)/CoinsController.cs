using UnityEngine;
using System.Collections;

public class CoinsController : MonoBehaviour 
{
	private SpriteRenderer spriteRenderer;
	private CoinsManager coinsManager; 
	// Use this for initialization
	void Start () 
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		coinsManager = GameObject.FindGameObjectWithTag(Tags.CoinsManager).GetComponent<CoinsManager>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.gameObject.tag.Equals(Tags.player))
		{
			GameObject effect = (GameObject) Instantiate(Resources.Load(Effects.CoinEffect),transform.position,Quaternion.identity);
			spriteRenderer.enabled = false;
			coinsManager.updateCoins(1);
			effect.transform.parent = transform;
			effect.transform.localPosition = Vector3.zero;

			GameManager.Instance.soundState.playSound(SoundController.States.COINSPICKUPSOUND);

		}
	}
}
