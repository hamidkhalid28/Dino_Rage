using UnityEngine;
using System.Collections;
using ProgressBar;
using UnityEngine.SceneManagement;

public class MainMenuLoadingController : MonoBehaviour 
{
	public ProgressBarBehaviour progressbar;
	AsyncOperation loading_operation;
	public bool isActive;
	public CanvasGroup menu_canvas;

	void Awake()
	{
		isActive = false;
		Clear();
	}

	void Start()
	{
		loading_operation = SceneManager.LoadSceneAsync(Constants.MAINMENU_SCENE);
		loading_operation.allowSceneActivation = false;

	}

	void Update () 
	{
		if(isActive)
			progressbar.IncrementValue(1);
	}

	public void OnComplete()
	{
		loading_operation.allowSceneActivation = true;
	}

	public void Clear()
	{
		menu_canvas.alpha = 0;
	}

	public void Show()
	{
		LeanTween.alphaCanvas(menu_canvas,1,1);
	}
}
