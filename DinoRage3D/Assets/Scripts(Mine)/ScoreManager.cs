using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour 
{
	public Text score_text;
	public Text score_effect_text;
	public Animator score_effect_animator;
	public RectTransform score_effect_pos;

	private int score;

	void Start()
	{
		score = 0;
	}


	public void setScore(Vector3 position,int score)
	{
		if (position != Vector3.zero) {
			showScoreEffect (position, score);
		}
		this.score += score;
		score_text.text = this.score.ToString();
	}

	private void showScoreEffect(Vector3 position,int score)
	{
		score_effect_pos.position = position;

		if(!score_effect_animator.GetBool("showScore"))
		{
			score_effect_text.enabled = true;
			score_effect_animator.SetBool("showScore",true);
			score_effect_text.text = "+" + score;
		}
	}

}
