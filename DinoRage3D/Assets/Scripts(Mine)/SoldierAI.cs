using UnityEngine;
using System.Collections;
using Lean;

public class SoldierAI : MonoBehaviour 
{
	public float speed;
	public Transform shooting_pos;
	public GameObject bullet;

	Rigidbody rb;
	Transform target;
//	Animation anim;
	Animator anim;
	AudioSource source;

	bool isFiring = false;

	SoldierCollisionController collisionController;
	EffectsController effects_controller;

	// Use this for initialization
	void Start () 
	{
		source = GetComponent<AudioSource> ();

		if(!Prefs.isSound)
		{
			source.mute = true;
		}

		collisionController = GetComponent<SoldierCollisionController>();
		effects_controller = GameObject.FindGameObjectWithTag(Tags.EffectsController).GetComponent<EffectsController>();
		rb = GetComponent<Rigidbody>();
//		anim = GetComponent<Animation>();
		anim = GetComponent<Animator>();

		target = GameObject.FindGameObjectWithTag(Tags.aimPoint).transform;
		transform.LookAt(target);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (SharedVariables.isGamePlay) 
		{
			
			transform.LookAt (target);

			float distance = Vector3.Distance (transform.position, target.position);

			if (distance < 35) 
			{
				if (!isFiring) 
				{
					isFiring = true;
//					anim.CrossFade ("soldierFiring");
					anim.SetBool("Fire",true);
					anim.SetBool("Idle",false);
					anim.SetBool("Run",false);


					rb.isKinematic = true;
//					InvokeRepeating ("Shoot", 0.6f, 0.1f);
				}
			} else {
				if (isFiring) {
					CancelInvoke ("Shoot");
					source.Stop ();

					isFiring = false;
//					anim.CrossFade ("soldierSprint");
					anim.SetBool("Run",true);
					anim.SetBool("Fire",false);
					anim.SetBool("Idle",false);
					rb.isKinematic = false;

				}

			}
		}
		else
		{
			CancelInvoke ("Shoot");
			source.Stop ();

//			anim.CrossFade ("soldierIdle");
			anim.SetBool("Idle",true);
			anim.SetBool("Run",false);
			anim.SetBool("Fire",false);
		}
	}

	void FixedUpdate()
	{
		if(!collisionController.IsDead && !isFiring)
		{
			rb.AddForce(transform.forward * speed);
		}
	}

	public void Shoot()
	{
		if (collisionController.IsDead) {
			CancelInvoke ();
			source.Stop ();

		}

		source.Play ();
		effects_controller.playBulletSparkEffect(shooting_pos);
		LeanPool.Spawn(bullet,shooting_pos.position,shooting_pos.transform.rotation);
	}

}
