using UnityEngine;
using System.Collections;


public class PreyCollisionController : MonoBehaviour 
{
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
		GameManager.Instance.soundState.playSound(SoundController.States.HUMANDEATH);
		GetComponent<Rigidbody>().drag = 2;
		isDead = true;
		animator.SetBool("IsDeath",true);
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

		if(tag.Equals(Tags.Prey))
		{
			type = Constants.PREY_TYPES.Human;
		}
		else
		{
			type = Constants.PREY_TYPES.Soldier;
		}

//		SpawnerAttributes obj = new SpawnerAttributes(type,transform.position);

		SpawnManager.Instance.EnQueue(type);

//		Destroy(temp);

		GetComponent<CapsuleCollider>().enabled = false;
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
