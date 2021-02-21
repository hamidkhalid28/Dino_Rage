using UnityEngine;
using System.Collections;
using CnControls;

public class PlayerAnimationDecider : MonoBehaviour 
{
	private SimpleJoystick MovementJoystick;
	private Animator animator;
	private EffectsController effects_controller;
	private ScoreManager score_manager;
	private CoinsManager coins_manager;

	GameObject target;

	// Use this for initialization
	void Awake () 
	{
		if(!MovementJoystick)
		{
			MovementJoystick = GameObject.FindGameObjectWithTag(Tags.Joystick).GetComponent<SimpleJoystick>();
			animator = GetComponent<Animator>();
		}

		effects_controller = GameObject.FindGameObjectWithTag(Tags.EffectsController).GetComponent<EffectsController>();
		score_manager = GameObject.FindGameObjectWithTag(Tags.Hud).GetComponent<ScoreManager>();
		coins_manager = GameObject.FindGameObjectWithTag(Tags.Hud).GetComponent<CoinsManager>();


	}

	void OnEnable()
	{
		GameManager.Instance.OnGamePlayState += OnGamePlayState;
		GameManager.Instance.OnGameOverState += OnGameOverState;
	}

	void OnDisable()
	{
		if (GameManager.Instance) 
		{
			GameManager.Instance.OnGamePlayState -= OnGamePlayState;
			GameManager.Instance.OnGameOverState -= OnGameOverState;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(MovementJoystick.isJoystickAlive)
		{
			if(!animator.GetBool("IsRun"))
				animator.SetBool("IsRun",true);
		}
		else 
		{
			if(!animator.GetBool("IsAttack") && !animator.GetBool("IsIdle") && !animator.GetBool("IsEat"))
				animator.SetBool("IsIdle",true);
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.tag.Equals(Tags.Prey))
		{
			target = other.gameObject;
		}
	}

	void OnTriggerExit(Collider other)
	{
		if(other.tag.Equals(Tags.Prey))
		{
			target = null;
		}
	}


	public void onAttackEvent()
	{
		if(target != null)
		{
			effects_controller.playPreyHitEffect(target.transform);

			if(target.CompareTag(Tags.Prey))
			{
				if(!target.transform.GetComponent<PreyCollisionController>().IsDead)
				{
					score_manager.setScore(target.transform.position,SCORES.prey_score);
					coins_manager.updateCoins(1);

				}
			}
			else
			{
				if(!target.transform.GetComponent<SoldierCollisionController>().IsDead)
				{
					score_manager.setScore(target.transform.position,SCORES.soldier_score);
					coins_manager.updateCoins(1);

				}
			}
		}
	}

	public void OnGamePlayState()
	{
		animator.SetBool ("IsWakeUp", true);

	}

	public void OnGameOverState()
	{
		animator.SetBool ("IsDie", true);
	}
}
