using UnityEngine;
using System.Collections;


public class NavigationTarget : MonoBehaviour 
{
	Transform[] targets;
	int currentTarget = 0;
	float speed = 40;

	Rigidbody rb;

	PreyCollisionController collisionController;

	// Use this for initialization
	void Start () 
	{
		collisionController = GetComponent<PreyCollisionController>();
		rb = GetComponent<Rigidbody>();
		GetComponent<Animator>().SetBool("IsRun",true);
		targets = GameObject.FindGameObjectWithTag(Tags.NavigationPaths).GetComponent<Targets>().targets;
		generateRandomTarget();
		transform.LookAt(targets[currentTarget]);
	}

	void FixedUpdate()
	{
		if(!collisionController.IsDead)
		{
			Vector3 fwd = transform.TransformDirection(Vector3.forward);
			RaycastHit hit;

			transform.LookAt(targets[currentTarget]);


			if (Physics.Raycast(transform.position,fwd,out hit,10)) 
			{
				if(hit.transform.CompareTag(Tags.player) || hit.transform.CompareTag(Tags.Prey))
				{
					generateRandomTarget();
				}
			}

			rb.AddForce(transform.forward * speed);

		}
	}

	void LateUpdate()
	{
		if(Vector3.Distance(transform.position,targets[currentTarget].position) < 7)
		{
			generateRandomTarget();
			transform.LookAt(targets[currentTarget]);
		}
			
	}

  	public void generateRandomTarget()
	{
		currentTarget = Random.Range(0,targets.Length);
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag(Tags.player) || other.tag.Equals(Tags.Prey))
		{
			generateRandomTarget();
		}
	}
}
