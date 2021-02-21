using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EventHandler : MonoBehaviour 
{
	public CanvasGroup eatBtn;
	public CanvasGroup speech_bubble;
	public Slider score_bar_speech_bubble;
	public Text score;
	public Text animated_score;
	public Text praise;


	private PlayerCollisionController playerCollisionController;
	private ScoreManager score_manager;
	int killCount;


	// Use this for initialization
	void Start () 
	{
		hideEatButton();
		playerCollisionController = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<PlayerCollisionController>();
		score_manager = GetComponent<ScoreManager> ();

		speech_bubble.alpha = 0;
		killCount = 0;
		animated_score.color = Color.clear;

	}
		

	public void showEatButton()
	{
		eatBtn.alpha = 1;
		eatBtn.interactable = true;
	}

	public void hideEatButton()
	{
		eatBtn.alpha = 0;
		eatBtn.interactable = false;
	}

	public void onEatBtnClick()
	{
		GameManager.Instance.soundState.playSound(SoundController.States.EAT);
		hideEatButton();
		playerCollisionController.eatPrey();
	}

	public void UpdateKillStreakStats()
	{
		LeanTween.cancelAll ();
		killCount++;
		score.text = killCount.ToString () + " x " + Constants.REWARD_PER_KILL.ToString () + " = " + (killCount * Constants.REWARD_PER_KILL).ToString ();
		speech_bubble.alpha = 1f;
		score_bar_speech_bubble.value = score_bar_speech_bubble.maxValue;

		praise.text = Constants.PRAISE_TEXT [Random.Range (0, Constants.PRAISE_TEXT.Length)];
		LeanTween.scale (speech_bubble.gameObject, Vector3.one * 0.15f, 0.3f).setEase (LeanTweenType.easeShake);
		LeanTween.rotateZ (speech_bubble.gameObject, Random.Range (-5, 5), 0.2f).setLoopPingPong ().setPeriod (1);
		LeanTween.alphaCanvas (speech_bubble, 0, 4).setDelay(2).onComplete = OnCompleteTween;
		LeanTween.value (score_bar_speech_bubble.gameObject, score_bar_speech_bubble.maxValue, score_bar_speech_bubble.minValue, 6)
			.setOnUpdate((float val) => {score_bar_speech_bubble.value = val; });



	}

	void OnCompleteTween()
	{
		score_manager.setScore (Vector3.zero,killCount * Constants.REWARD_PER_KILL);
		animated_score.text = (killCount * Constants.REWARD_PER_KILL).ToString ();
		animated_score.color = Color.white;
		LeanTween.move (animated_score.GetComponent<RectTransform> (), new Vector2 (2, Screen.height), 3);
		LeanTween.textColor (animated_score.GetComponent<RectTransform> (), Color.clear, 3);
		killCount = 0;
	}

}
