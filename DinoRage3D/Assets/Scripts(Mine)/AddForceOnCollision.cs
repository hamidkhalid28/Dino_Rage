using UnityEngine;
using System.Collections;

public class AddForceOnCollision : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	void OnCollisionEnter(Collision collisioninfo)
	{
		GetComponent<Rigidbody>().AddForce(transform.forward * 20);
	}
}
