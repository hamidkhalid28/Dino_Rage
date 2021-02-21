using UnityEngine;
using System.Collections;
using YounGenTech.HealthScript;

public class PlayerCollisionController : MonoBehaviour 
{
	private ScoreManager score_manager;
	private CoinsManager coins_manager;
	private GameObject preyToEat;
	private Animator animator;
	private PlayerController player_controller;
	private EffectsController effects_controller;
	private EventHandler event_handler;
	private Health player_health;

	void Awake()
	{
		score_manager = GameObject.FindGameObjectWithTag(Tags.Hud).GetComponent<ScoreManager>();
		coins_manager = GameObject.FindGameObjectWithTag(Tags.Hud).GetComponent<CoinsManager>();
		effects_controller = GameObject.FindGameObjectWithTag(Tags.EffectsController).GetComponent<EffectsController>();
		player_controller = GetComponent<PlayerController>();
		event_handler = GameObject.FindGameObjectWithTag (Tags.Hud).GetComponent<EventHandler> ();
		animator = GetComponent<Animator>();
		player_health = GetComponent<Health> ();


	}

	void Start()
	{
		
	}

	void Update()
	{

	}


	void OnCollisionEnter (Collision col)
	{

		if(col.gameObject.tag.Equals(Tags.Prop))
		{
			GameManager.Instance.soundState.playSound(SoundController.States.METALHURDLEHIT);
			score_manager.setScore(col.transform.position,SCORES.prop_score);
			coins_manager.updateCoins(1);
		}
		else if(col.gameObject.tag.Equals(Tags.Statue))
		{
			GameManager.Instance.soundState.playSound(SoundController.States.METALHURDLEHIT);

			score_manager.setScore(col.transform.position,SCORES.statue_score);
			coins_manager.updateCoins(1);

		}
		else if(col.gameObject.tag.Equals(Tags.House))
		{
			GameManager.Instance.soundState.playSound(SoundController.States.WOODHURDLEHIT);

			score_manager.setScore(col.transform.position,SCORES.house_score);
			coins_manager.updateCoins(1);

		}
		else if(col.gameObject.tag.Equals(Tags.Tree))
		{
			effects_controller.playLeavesFallingEffect(col.transform);

			GameManager.Instance.soundState.playSound(SoundController.States.WOODHURDLEHIT);

			score_manager.setScore(col.transform.position,SCORES.tree_score);
			coins_manager.updateCoins(1);

		}
		else if(col.gameObject.tag.Equals(Tags.Prey))
		{
			effects_controller.playPreyHitEffect(col.transform);

			if(!col.transform.GetComponent<PreyCollisionController>().IsDead)
			{
				score_manager.setScore(col.transform.position,SCORES.prey_score);
				coins_manager.updateCoins(1);

			}
		}
		else if(col.gameObject.tag.Equals(Tags.Soldier))
		{
			effects_controller.playPreyHitEffect(col.transform);

			if(!col.transform.GetComponent<SoldierCollisionController>().IsDead)
			{
				score_manager.setScore(col.transform.position,SCORES.soldier_score);
				coins_manager.updateCoins(1);

			}
		}
	}

	public void setPreyToEat(GameObject prey)
	{
		preyToEat = prey;
	}

	public void eatPrey()
	{
		player_controller.isBusy = true;
		preyToEat.transform.position = transform.position-(transform.forward*5);
//		transform.LookAt(preyToEat.transform);
//		float yRotation = preyToEat.transform.rotation.y - transform.rotation.y;
//		transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x,(transform.rotation.y-yRotation),transform.rotation.z));
		animator.SetBool("IsEat",true);
//		Invoke("freePrey",1.0f);

	}

	public void freePrey()
	{
		player_health.Heal (new HealthEvent (preyToEat, 2));

		if(preyToEat.CompareTag(Tags.Prey))
			preyToEat.GetComponent<PreyCollisionController>().die();
		else
			preyToEat.GetComponent<SoldierCollisionController>().die();

		if(!event_handler)
		{
			event_handler = GameObject.FindGameObjectWithTag (Tags.Hud).GetComponent<EventHandler> ();
		}


		event_handler.UpdateKillStreakStats ();
		score_manager.setScore(preyToEat.transform.position,SCORES.eat_score);
		player_controller.isBusy = false;

	}
}
