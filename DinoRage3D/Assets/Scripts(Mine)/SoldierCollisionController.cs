using UnityEngine;
using System.Collections;

public class SoldierCollisionController : MonoBehaviour 
{

//	Animation animator;
	Animator animator;
	EventHandler eventHandler;
	PlayerCollisionController playerCollisionController;

	Vector3 originalPos;


	bool isDead;

	public bool IsDead {
		get {
			return isDead;
		}
		set {
			isDead = value;
		}
	}

	// Use this for initialization
	void Awake () 
	{
//		animator = GetComponent<Animation>();
		animator = GetComponent<Animator>();

		eventHandler = GameObject.FindGameObjectWithTag(Tags.Hud).GetComponent<EventHandler>();

		originalPos = transform.position;

	}

	void Start()
	{
		playerCollisionController = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<PlayerCollisionController>();

	}


	void OnCollisionEnter(Collision collision)
	{
		if(collision.gameObject.tag.Equals(Tags.player) && !isDead)
		{
			Death ();
		}
	}

	void Death()
	{
		GetComponent<SoldierAI>().enabled = false;
		GetComponent<CapsuleCollider>().enabled = false;
		GameManager.Instance.soundState.playSound(SoundController.States.HUMANDEATH);
		GetComponent<Rigidbody>().drag = 2;
		isDead = true;
//		animator.CrossFade("soldierDieBack");
		animator.SetBool("Death",true);
		animator.SetBool("Run",false);
		animator.SetBool("Fire",false);
		animator.SetBool("Idle",false);

		eventHandler.showEatButton();
		playerCollisionController.setPreyToEat(gameObject);

		Invoke ("die", 4);

	}

	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag.Equals(Tags.player) && isDead && SharedVariables.isGamePlay)
		{
			eventHandler.showEatButton();
			playerCollisionController.setPreyToEat(gameObject);		
		}
	}

	void OnTriggerExit(Collider other)
	{
		if(other.gameObject.tag.Equals(Tags.player) && isDead && SharedVariables.isGamePlay)
		{
			eventHandler.hideEatButton();
		}
	}

	public void die()
	{
		if(IsInvoking())
			CancelInvoke ("die");

//		GameObject temp = new GameObject();
//		temp.transform.position = originalPos;
//		temp.transform.rotation = transform.rotation;

		Constants.PREY_TYPES type;


		type = Constants.PREY_TYPES.Soldier;
		

//		SpawnerAttributes obj = new SpawnerAttributes(type,temp.transform);

		SpawnManager.Instance.EnQueue(type);

//		Destroy(temp);

		Invoke("destroyObject",2);


	}

	void destroyObject()
	{
		Destroy(gameObject);
	}

	void playScreamSound()
	{
		if(!IsDead)
		{
			GameManager.Instance.soundState.playSound(SoundController.States.SCREAM);
		}
	}
}
