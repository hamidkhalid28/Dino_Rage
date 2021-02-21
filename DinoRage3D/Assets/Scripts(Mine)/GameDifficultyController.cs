using UnityEngine;
using System.Collections;

public class GameDifficultyController : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
		SharedVariables.rocketSpeed = SharedVariables.speed;
	}
	
	// Update is called once per frame
	void Update () 
	{
		SharedVariables.rocketSpeed += Time.deltaTime;
	}
}
