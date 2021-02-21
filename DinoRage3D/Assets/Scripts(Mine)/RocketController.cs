using UnityEngine;
using System.Collections;

public class RocketController : MonoBehaviour 
{
	public float speed = 1.2f;

	// Use this for initialization
	void Start () 
	{
		Destroy(gameObject,7f);

		SharedVariables.rocketSpeed = SharedVariables.speed;

	}
	
	// Update is called once per frame
	void Update () 
	{
		Vector3 vec = new Vector3(Time.deltaTime * SharedVariables.rocketSpeed,0,0);
		transform.Translate(vec);
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		instantiateBlastEffect();
		GameObject.FindGameObjectWithTag(Tags.gameOverManager).GetComponent<GameOverManager>().gameOver();

	}

	void instantiateBlastEffect()
	{
		GameObject effect = (GameObject) Resources.Load(Effects.Blast);
		Vector3 pos = new Vector3(transform.position.x-0.7f,transform.position.y,0);
		GameObject blast_effect = (GameObject) Instantiate(effect,pos,effect.transform.rotation);
		Destroy(blast_effect,2.0f);

		Destroy(gameObject);
	}

}
