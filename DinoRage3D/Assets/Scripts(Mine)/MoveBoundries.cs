using UnityEngine;
using System.Collections;

public class MoveBoundries : MonoBehaviour 
{

	
	// Update is called once per frame
	void Update () 
	{
		if(!SharedVariables.isPlayerDead)
       		this.transform.position = new Vector3(transform.position.x + (SharedVariables.speed * Time.smoothDeltaTime), transform.position.y, transform.position.z);
	}

}
