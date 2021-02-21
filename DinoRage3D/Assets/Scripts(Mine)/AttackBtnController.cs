using UnityEngine;
using System.Collections;

public class AttackBtnController : MonoBehaviour 
{
	Animator player_animator;
	// Use this for initialization
	void Start () 
	{
		player_animator = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void onBtnClick()
	{
		if(!player_animator.GetBool("IsAttack"))
			player_animator.SetBool("IsAttack",true);
	}
}
