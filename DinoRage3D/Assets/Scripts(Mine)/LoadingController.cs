using UnityEngine;
using System.Collections;
using ProgressBar;
using UnityEngine.SceneManagement;

public class LoadingController : MonoBehaviour {

	public ProgressBarBehaviour progressbar;
	public RectTransform mainPanel;


	// Use this for initialization
	void Awake () 
	{
		if(GameObject.FindGameObjectWithTag(Tags.PlayerSelectionMenu))
		{
			CanvasGroup cg = GameObject.FindGameObjectWithTag (Tags.PlayerSelectionMenu).GetComponent<CanvasGroup> ();

			cg.blocksRaycasts = false;
			cg.interactable = false;
		}

//		LeanTween.move(mainPanel,Vector3.zero,1.2f).setEase(LeanTweenType.easeInSine);

	}
	
	// Update is called once per frame
	void Update () 
	{
		progressbar.IncrementValue(1);

		if(SceneManager.GetActiveScene().name.Equals(Constants.GAMEPLAY_SCENE))
			Destroy(gameObject);

	}

	public void OnComplete()
	{
		GameManager.Instance.gameState.allowSceneActivation();
	}
}
