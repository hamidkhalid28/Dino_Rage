using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Lean;

public class SplashController : MonoBehaviour 
{
	public RectTransform splash_image;
	AsyncOperation loading_operation;
	public MainMenuLoadingController mainMenu_loading;

	public RectTransform loading_text;

	void Awake()
	{
//		LeanTween.color(splash_image,Color.clear,0);
	}

	// Use this for initialization
	void Start () 
	{
//		LeanTween.color(splash_image,Color.white,2);
//		LeanTween.color(splash_image,Color.clear,2).setDelay(4);
//		Invoke("setLoadingActive",6.5f);


		StartCoroutine(loadScene());
//		SceneManager.LoadScene(Constants.MAINMENU_SCENE);
		LeanTween.textAlpha(loading_text,0,1).setLoopType(LeanTweenType.pingPong);


	}

	IEnumerator loadScene()
	{
		loading_operation = SceneManager.LoadSceneAsync(Constants.MAINMENU_SCENE);
		yield return loading_operation;
	}

	void setLoadingActive()
	{
//		mainMenu_loading.isActive = true;
//		mainMenu_loading.Show();

		Destroy(gameObject,2);
	}
		
}
