using UnityEngine;
using System.Collections;

public class ColumnsController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		if(other.gameObject.tag.Equals(Tags.player))
		{
			GameObject.FindGameObjectWithTag(Tags.gameOverManager).GetComponent<GameOverManager>().gameOver();
		}
	}
}
