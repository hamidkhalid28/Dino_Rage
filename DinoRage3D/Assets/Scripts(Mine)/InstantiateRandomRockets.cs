using UnityEngine;
using System.Collections;

public class InstantiateRandomRockets : MonoBehaviour {

	public float firstTime = 2;
	public float repeatingTime = 2;
	
	GameObject player;
	
	// Use this for initialization
	void Start () 
	{
		player = GameObject.FindGameObjectWithTag("Player");
		InvokeRepeating("instantiateRocket",firstTime,repeatingTime);
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
	
	void instantiateRocket()
	{
		if(SharedVariables.isPlayerDead)
			Destroy(gameObject);

		if(SharedVariables.isGamePlay)
		{
			GameObject rocket = (GameObject) Resources.Load("Rocket");
			Vector3 pos = new Vector3(player.transform.position.x + 10,Random.Range(1.1f,10.1f),0);
			Instantiate(rocket,pos,rocket.transform.rotation);
		}
	}
}
